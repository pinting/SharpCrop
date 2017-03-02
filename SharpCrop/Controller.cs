using SharpCrop.Forms;
using SharpCrop.Provider;
using SharpCrop.Utils;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using SharpCrop.Models;
using SharpCrop.Properties;

namespace SharpCrop
{
    /// <summary>
    /// Controller is responsible for the loading of the application. It will also
    /// manage image capturing and uploading.
    /// </summary>
    public class Controller : ApplicationContext
    {
        private readonly Dictionary<string, IProvider> registeredProviders = new Dictionary<string, IProvider>();
        private readonly List<LoadedProvider> loadedProviders = new List<LoadedProvider>();
        private readonly List<Form> cropForms = new List<Form>();
        private readonly Form configForm;

        /// <summary>
        /// Construct a new Controller class.
        /// </summary>
        public Controller()
        {
            // Create a new ConfigForm
            configForm = new ConfigForm(this);
            configForm.FormClosed += (s, e) => Application.Exit();
            configForm.Load += (s, e) => RestoreProviders();

            // If there are any registered providers, the config will be hidden
            // Else the config will be closed (along with the whole application)
            configForm.FormClosing += (s, e) =>
            {
                if (e.CloseReason == CloseReason.UserClosing && registeredProviders.Count > 0)
                {
                    e.Cancel = true;
                    configForm.Hide();
                }
            };

            // Show crop forms if the config is hidden
            configForm.VisibleChanged += (s, e) =>
            {
                if (!configForm.Visible)
                {
                    ShowCrop();
                }
            };

            // Create a CropForm for every screen and show them
            for (var i = 0; i < Screen.AllScreens.Length; i++)
            {
                var screen = Screen.AllScreens[i];
                var form = new CropForm(this, screen.Bounds, i);

                form.FormClosed += (s, e) => Application.Exit();

                cropForms.Add(form);
            }

            // If StartupRegister is enabled, init providers on the load of the first CropForm
            if(ConfigHelper.Current.StartupRegister)
            {
                cropForms[0].Load += (s, e) => RestoreProviders();
            }

            // Show settings if no providers gonna be loaded
            if(ConfigHelper.Current.SafeProviders.Count > 0)
            {
                ShowCrop();
            }
            else
            {
                configForm.Show();
            }

            // Show welcome message if this is the first launch of the app
            if (!ConfigHelper.Current.NoWelcome)
            {
                // ReSharper disable once LocalizableElement
                MessageBox.Show(Resources.WelcomeMessage, "SharpCrop");
                ConfigHelper.Current.NoWelcome = true;
            }
        }

        /// <summary>
        /// Capture one Bitmap.
        /// </summary>
        /// <param name="region"></param>
        /// <param name="offset"></param>
        public async void CaptureImage(Rectangle region, Point offset)
        {
            using (var stream = new MemoryStream())
            {
                // Capture the frame
                using (var bitmap = CaptureHelper.GetBitmap(region, offset))
                {
                    bitmap.Save(stream, ConfigHelper.Current.ImageFormat);
                }

                // Generate filename and start the upload(s)
                var name = $"{DateTime.Now:yyyy_MM_dd_HH_mm_ss}.{ConfigHelper.Current.SafeImageExt}";
                var url = await UploadAll(name, stream);

                CompleteCapture(url);
            }
        }

        /// <summary>
        /// Capture a lot of bitmaps and convert them to gif.
        /// </summary>
        /// <param name="region"></param>
        /// <param name="offset"></param>
        public async void CaptureGif(Rectangle region, Point offset)
        {
            MemoryStream stream;
            var toast = -1;

            // Create a new toast which closing event gonna stop the recording
            toast = ToastFactory.Create(Resources.StopRecording, Color.OrangeRed, 0, () =>
            {
                toast = ToastFactory.Create(Resources.Encoding, 0);
                VideoFactory.Stop();
            });

            // Use Mpeg if enabled
            if (ConfigHelper.Current.EnableMpeg)
            {
                stream = await VideoFactory.RecordMpeg(region, offset);
            }
            else
            {
                stream = await VideoFactory.RecordGif(region, offset);
            }

            ToastFactory.Remove(toast);

            // Generate filename and start the upload(s)
            toast = ToastFactory.Create(string.Format(Resources.Uploading, $"{(double)stream.Length / (1024 * 1024):0.00} MB"), 0);

            var name = $"{DateTime.Now:yyyy_MM_dd_HH_mm_ss}.{(ConfigHelper.Current.EnableMpeg ? "mp4" : "gif")}";
            var url = await UploadAll(name, stream);

            ToastFactory.Remove(toast);
            stream.Dispose();

            CompleteCapture(url);
        }

        /// <summary>
        /// Upload the given stream with all loaded providers.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="stream"></param>
        /// <returns></returns>
        private async Task<string> UploadAll(string name, MemoryStream stream)
        {
            // Try to load the saved providers (if load on startup is disabled)
            if (!ConfigHelper.Current.StartupRegister)
            {
                RestoreProviders();
            }

            // Save file locally if no providers were registered
            if (registeredProviders.Count == 0)
            {
                File.WriteAllBytes(name, stream.ToArray());
                return null;
            }

            var uploads = new Dictionary<string, Task<string>>();

            string result = null;
            string last = null;

            // Run the uploads async
            foreach (var p in registeredProviders)
            {
                if (p.Value != null)
                {
                    uploads[p.Key] = p.Value.Upload(name, stream);
                }
            }

            // Wait for the uploads to finish and get the chosen URL
            foreach (var p in uploads)
            {
                var url = await p.Value;

                if (string.IsNullOrEmpty(url))
                {
                    ToastFactory.Create(string.Format(Resources.ProviderUploadFailed, p.Key));
                }
                else
                {
                    last = url;

                    if (p.Key == ConfigHelper.Current.CopyProvider)
                    {
                        result = url;
                    }
                }
            }

            // If the chosen URL was not found (or null), use the URL of the last successful one
            return string.IsNullOrEmpty(result) ? last : result;
        }

        /// <summary>
        /// Init loaded providers list.
        /// </summary>
        public void LoadProviders()
        {
            foreach (var type in Constants.Providers)
            {
                // I know, I know, this looks bad
                // I create a new Instance to write down the Name and Id
                // To help the bellow elsewere, to create another new instance
                var provider = (IProvider)Activator.CreateInstance(type);

                loadedProviders.Add(new LoadedProvider()
                {
                    Id = provider.Id,
                    Name = provider.Name,
                    ProviderType = type
                });
            }
        }

        /// <summary>
        /// Register providers from USER SETTINGS, where they were previously saved!
        /// </summary>
        private void RestoreProviders()
        {
            // Load available IProvider types into memory
            if (loadedProviders.Count == 0)
            {
                LoadProviders();
            }

            var tasks = new List<Task>();

            // ToList() is needed because the original Dictionary is changing while we iterating
            ConfigHelper.Current.SafeProviders
                .ToList()
                .ForEach(p => tasks.Add(
                    RegisterProvider(GetProviderById(p.Value.Id), p.Key, p.Value.State, false)));

            // Wait for tasks to finish
            tasks.ForEach(async p => await p);
        }

        /// <summary>
        /// Unregister a provider.
        /// </summary>
        /// <param name="name"></param>
        public void ClearProvider(string name)
        {
            // Remove from the registered providers
            if (registeredProviders.ContainsKey(name))
            {
                registeredProviders.Remove(name);
            }

            // Remove from the configuration file
            if (ConfigHelper.Current.SafeProviders.ContainsKey(name))
            {
                ConfigHelper.Current.SafeProviders.Remove(name);
            }
        }

        /// <summary>
        /// Get a new instance of a IProvider class by name.
        /// </summary>
        /// <param name="name">Name of the class, can be language specific.</param>
        /// <returns></returns>
        public IProvider GetProviderByName(string name)
        {
            var loaded = loadedProviders.FirstOrDefault(e => e.Name == name);

            if (loaded != null)
            {
                return (IProvider)Activator.CreateInstance(loaded.ProviderType);
            }
            
            return null;
        }

        /// <summary>
        /// Get a new instance of a IProvider class by id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IProvider GetProviderById(string id)
        {
            var loaded = loadedProviders.FirstOrDefault(e => e.Id == id);

            if (loaded != null)
            {
                return (IProvider)Activator.CreateInstance(loaded.ProviderType);
            }

            return null;
        }

        /// <summary>
        /// Try to register a provider.
        /// </summary>
        /// <param name="provider">An IProvider class.</param>
        /// <param name="nick">The nick of the provider - CAN BE ANYTHING!</param>
        /// <param name="savedState">A saved state that the Provider will try to interpret - if it fails, the registration form will be used.</param>
        /// <param name="showForm">Enable or disable registration form.</param>
        /// <returns></returns>
        public async Task<bool> RegisterProvider(IProvider provider, string nick, string savedState = null, bool showForm = true)
        {
            // If there is already a REGISTERED PROVIDER with this (exact) nick, return with false
            if (provider == null || registeredProviders.ContainsKey(nick))
            {
                return false;
            }

            // Try to register the provider form the savedState
            var state = await provider.Register(savedState, showForm);

            if (showForm && state == null)
            {
                ToastFactory.Create(string.Format(Resources.ProviderRegistrationFailed, provider.Name));
            }
            else if (showForm && state != savedState)
            {
                // If the token is not changed, there was no new registration
                ToastFactory.Create(string.Format(Resources.ProviderRegistrationSucceed, provider.Name));
            }

            // If the the registration failed, return with false
            if (state == null)
            {
                return false;
            }

            // Save the provider to the config
            ConfigHelper.Current.Providers[nick] = new SavedProvider()
            {
                Id = provider.Id,
                State = state
            };

            // Save the provider as registered
            registeredProviders[nick] = provider;

            return true;
        }

        /// <summary>
        /// Show the end notifcation. If the url is null, the text gonna tell the user, that the upload has failed.
        /// </summary>
        /// <param name="url"></param>
        private void CompleteCapture(string url = null)
        {
            if (!ConfigHelper.Current.NoCopy && !string.IsNullOrEmpty(url))
            {
                Clipboard.SetText(url);

#if __MonoCS__
                var form = new CopyForm(url);
                form.FormClosed += (object sender, FormClosedEventArgs e) => Application.Exit();
                form.Show();
#endif
            }

            ToastFactory.Create(string.IsNullOrEmpty(url) ? Resources.UploadFailed : Resources.UploadCompleted, 3000, () =>
            {
#if !__MonoCS__
                Application.Exit();
#endif
            });
        }

        /// <summary>
        /// Hide every CropForm - if more than one monitor is used.
        /// </summary>
        public void HideCrop() => cropForms.ForEach(e => e.Hide());

        /// <summary>
        /// Show every CropForm.
        /// </summary>
        public void ShowCrop() => cropForms.ForEach(e => e.Show());

        /// <summary>
        /// Currently registered (working) providers.
        /// </summary>
        public IReadOnlyDictionary<string, IProvider> RegisteredProviders => registeredProviders;

        /// <summary>
        /// Currently loaded (available) providers.
        /// </summary>
        public IReadOnlyList<LoadedProvider> LoadedProviders => loadedProviders;

        /// <summary>
        /// Protect list from external modification.
        /// </summary>
        public IReadOnlyList<Form> CropForms => cropForms;

        /// <summary>
        /// Protect variable from external modification.
        /// </summary>
        public Form ConfigForm => configForm;
    }
}