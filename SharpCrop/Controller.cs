using SharpCrop.Forms;
using SharpCrop.Provider;
using SharpCrop.Utils;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SharpCrop
{
    /// <summary>
    /// Controller is responsible for the loading of the application. It will also
    /// manage image capturing and uploading.
    /// </summary>
    public class Controller : ApplicationContext
    {
        private readonly Dictionary<string, IProvider> loadedProviders = new Dictionary<string, IProvider>();
        private readonly List<Form> cropForms = new List<Form>();
        private Form configForm;

        /// <summary>
        /// Construct a new Controller class.
        /// </summary>
        public Controller()
        {
            // Create ConfigForm
            configForm = new ConfigForm(this);
            configForm.FormClosed += (s, e) => Application.Exit();
            configForm.FormClosing += (s, e) =>
            {
                // If there are any loaded providers, the config will be hidden
                // Else the config will be closed (along with the whole application)
                if (e.CloseReason == CloseReason.UserClosing && loadedProviders.Count > 0)
                {
                    e.Cancel = true;
                    configForm.Hide();
                }
            };

            // Create a CropForm for every screen
            for (var i = 0; i < Screen.AllScreens.Length; i++)
            {
                var screen = Screen.AllScreens[i];
                var form = new CropForm(this, screen.Bounds, i);

                form.FormClosed += (s, e) => Application.Exit();
                cropForms.Add(form);
            }

            // Init providers
            InitProviders();
        }

        /// <summary>
        /// Init providers with the previously saved states.
        /// </summary>
        private async void InitProviders()
        {
            foreach (var e in ConfigHelper.Memory.SafeProviders)
            {
                await LoadProvider(e.Key, e.Value);
            }

            // If at least one provider is loaded, show the crop windows
            if (loadedProviders.Count > 0)
            {
                cropForms.ForEach(form => form.Show());
            }
            else
            {
                configForm.Show();
            }
        }

        /// <summary>
        /// Register the given provider with a registration form and load it.
        /// </summary>
        /// <param name="name"></param>
        public async void RegisterProvider(string name)
        {
            var provider = await GetProvider(name, null, true);

            if (provider != null)
            {
                loadedProviders[name] = provider;
            }
        }

        /// <summary>
        /// Try to load the given provider with the saved state silently - if it failes, there will
        /// be no registration form.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="savedState"></param>
        public async Task<bool> LoadProvider(string name, string savedState)
        {
            var provider = await GetProvider(name, savedState, false);

            if (provider != null)
            {
                loadedProviders[name] = provider;
                return true;
            }

            return false;
        }

        /// <summary>
        /// Unregister a provider.
        /// </summary>
        /// <param name="name"></param>
        public void ClearProvider(string name)
        {
            // Remove from the loaded providers
            if (loadedProviders.ContainsKey(name))
            {
                loadedProviders.Remove(name);
            }

            // Remove from the configuration file
            if (ConfigHelper.Memory.SafeProviders.ContainsKey(name))
            {
                ConfigHelper.Memory.SafeProviders.Remove(name);
            }
        }

        /// <summary>
        /// Get a provider.
        /// </summary>
        /// <param name="name">The name of the provider, defined in the Constants.</param>
        /// <param name="savedState">A saved state which can be null.</param>
        /// <param name="showForm">Show the registration form or not.</param>
        /// <returns>Return a Provider state (usually json in base64), if the was an error, the result will be null.</returns>
        private async Task<IProvider> GetProvider(string name, string savedState = null, bool showForm = true)
        {
            if(!Constants.AvailableProviders.ContainsKey(name))
            {
                return null;
            }

            // Translate name into a real instance and try to load the provider form the given saved state
            var provider = (IProvider)Activator.CreateInstance(Constants.AvailableProviders[name]);
            var state = await provider.Register(savedState, showForm);

            if (state == null)
            {
                ToastFactory.Create($"Failed to register \"{name}\" provider!");
                return null;
            }

            // If the token is not changed, there was no new registration
            if (state != savedState)
            {
                ConfigHelper.Memory.SafeProviders[name] = state;
                ToastFactory.Create($"Successfully registered \"{name}\" provider!");
            }
            
            return provider;
        }

        private async Task<string> UploadAll(string name, MemoryStream stream)
        {
            string result = null;
            string url = null;

            foreach (var provider in loadedProviders)
            {
                url = await provider.Value.Upload(name, stream);

                if (provider.Key == ConfigHelper.Memory.ProviderToCopy)
                {
                    result = url;
                }
            }

            // If the searched provider was not found, use the URL of the last one
            return result == null ? url : result;
        }

        /// <summary>
        /// Capture one Bitmap.
        /// </summary>
        /// <param name="region"></param>
        public async void CaptureImage(Rectangle region, Point offset)
        {
            using (var stream = new MemoryStream())
            {
                using (var bitmap = CaptureHelper.GetBitmap(region, offset))
                {
                    bitmap.Save(stream, ConfigHelper.Memory.ImageFormatType);
                }

                var name = $"{DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss")}.{ConfigHelper.Memory.SafeImageFormat}";
                var url = await UploadAll(name, stream);

                Success(url);
            }
        }

        /// <summary>
        /// Capture a lot of Bitmaps and convert them to Gif.
        /// </summary>
        /// <param name="region"></param>
        public async void CaptureGif(Rectangle region, Point offset)
        {
            MemoryStream stream;
            var toast = -1;

            // Create a new toast which closing event gonna stop the recording
            toast = ToastFactory.Create("Click here to stop!", Color.OrangeRed, 0, () => 
            {
                toast = ToastFactory.Create("Encoding...", 0);
                VideoFactory.Stop();
            });

            // Use Mpeg if enabled
            if (ConfigHelper.Memory.EnableMpeg)
            {
                stream = await VideoFactory.RecordMpeg(region, offset);
            }
            else
            {
                stream = await VideoFactory.RecordGif(region, offset);
            }
            
            ToastFactory.Remove(toast);
            
            toast = ToastFactory.Create($"Uploading... ({(double) stream.Length/(1024*1024):0.00} MB)", 0);

            var name = $"{DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss")}.{(ConfigHelper.Memory.EnableMpeg ? "mp4" : "gif")}";
            var url = await UploadAll(name, stream);

            ToastFactory.Remove(toast);
            stream.Dispose();

            Success(url);
        }

        /// <summary>
        /// Show the end notifcation on successful upload.
        /// </summary>
        /// <param name="url"></param>
        private void Success(string url = null)
        {
            if (!ConfigHelper.Memory.NoCopy && !string.IsNullOrEmpty(url))
            {
                Clipboard.SetText(url);

#if __MonoCS__
                var form = new CopyForm(url);
                form.FormClosed += (object sender, FormClosedEventArgs e) => Application.Exit();
                form.Show();
#endif
            }

            ToastFactory.Create("Uploaded successfully!", 3000, () =>
            {
#if !__MonoCS__
                Application.Exit();
#endif
            });
        }

        /// <summary>
        /// Protect list from external modification.
        /// </summary>
        public IReadOnlyDictionary<string, IProvider> LoadedProviders
        {
            get
            {
                return loadedProviders;
            }
        }

        /// <summary>
        /// Protect list from external modification.
        /// </summary>
        public IReadOnlyList<Form> CropForms
        {
            get
            {
                return cropForms;
            }
        }

        /// <summary>
        /// Protect variable from external modification.
        /// </summary>
        public Form ConfigForm
        {
            get
            {
                return configForm;
            }
        }
    }
}
