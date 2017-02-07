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
            // Open ConfigForm
            configForm = new ConfigForm(this);
            configForm.FormClosed += (s, e) => Application.Exit();

            // Open a CropForm for every screen
            for (var i = 0; i < Screen.AllScreens.Length; i++)
            {
                var screen = Screen.AllScreens[i];
                var form = new CropForm(this, screen.Bounds, i);

                form.FormClosed += (s, e) => Application.Exit();
                cropForms.Add(form);
            }

            // Add an advanced closing event for ConfigForm
            // - If there are any loaded providers, the config will be hidden
            // - Else the config will be closed (along with the whole application)
            configForm.FormClosing += (s, e) =>
            {
                if(e.CloseReason == CloseReason.UserClosing && loadedProviders.Count > 0)
                {
                    e.Cancel = true;
                    configForm.Hide();
                }
            };

            // Load previously saved providers
            LoadSavedProviders();
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

        /// <summary>
        /// Load the required Provider with the registration form.
        /// </summary>
        /// <param name="name"></param>
        public async void LoadProvider(string name)
        {
            var provider = await GetProvider(name, null);

            if (provider == null)
            {
                configForm.Show();
            }
            else
            {
                loadedProviders[name] = provider;
            }
        }

        /// <summary>
        /// Remove the given provider.
        /// </summary>
        /// <param name="name"></param>
        public void ClearProvider(string name)
        {
            if (loadedProviders.ContainsKey(name))
            {
                loadedProviders.Remove(name);
            }

            if (ConfigHelper.Memory.SafeProvider.ContainsKey(name))
            {
                ConfigHelper.Memory.SafeProvider.Remove(name);
            }
        }

        /// <summary>
        /// Load saved providers from the config.
        /// </summary>
        private async void LoadSavedProviders()
        {
            var remove = new List<string>();

            foreach (var e in ConfigHelper.Memory.SafeProvider)
            {
                var provider = await GetProvider(e.Key, e.Value, false);

                if (provider != null)
                {
                    loadedProviders[e.Key] = provider;
                }
                else
                {
                    remove.Add(e.Key);
                }
            }

            remove.ForEach(provider => ClearProvider(provider));

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
        /// Get a Provider by name and give it back with a callback function.
        /// </summary>
        /// <param name="name"></param>
        private async Task<IProvider> GetProvider(string name, string savedState, bool showForm = true)
        {
            if(!Constants.Providers.ContainsKey(name))
            {
                return null;
            }

            // Translate name into a real instance and try to load the provider form saved state
            var provider = (IProvider)Activator.CreateInstance(Constants.Providers[name]);
            var state = await provider.Register(savedState, showForm);

            if (state == null)
            {
                ToastFactory.Create($"Failed to register \"{name}\" provider!");
                return null;
            }

            // If the token is not changed, there was no new registration
            if (state != savedState)
            {
                ConfigHelper.Memory.SafeProvider[name] = state;
                ToastFactory.Create($"Successfully registered \"{name}\" provider!");
            }
            
            return provider;
        }

        /// <summary>
        /// Capture one Bitmap.
        /// </summary>
        /// <param name="region"></param>
        public async void CaptureImage(Rectangle region, Point offset)
        {
            var name = DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss") + "." + ConfigHelper.Memory.FormatExt;

            using (var stream = new MemoryStream())
            {
                using (var bitmap = CaptureHelper.GetBitmap(region, offset))
                {
                    bitmap.Save(stream, ConfigHelper.Memory.FormatType);
                }

                var url = "";

                foreach(var provider in loadedProviders.Values)
                {
                    url = await provider.Upload(name, stream);
                }
                
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
            var url = "";

            foreach (var provider in loadedProviders.Values)
            {
                url = await provider.Upload(name, stream);
            }

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
            if (!ConfigHelper.Memory.NoCopy && url != null)
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
    }
}
