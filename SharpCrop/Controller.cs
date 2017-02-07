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
        public readonly List<IProvider> Providers = new List<IProvider>();
        public readonly List<Form> CropForms = new List<Form>();
        public Form ConfigForm;
        
        /// <summary>
        /// Construct a new Controller class.
        /// </summary>
        public Controller()
        {
            // Open ConfigForm
            ConfigForm = new ConfigForm(this);
            ConfigForm.FormClosed += (s, e) => Application.Exit();

            // Open a CropForm for every screen
            for (var i = 0; i < Screen.AllScreens.Length; i++)
            {
                var screen = Screen.AllScreens[i];
                var form = new CropForm(this, screen.Bounds, i);

                form.FormClosed += (s, e) => Application.Exit();
                CropForms.Add(form);
            }

            // Load providers
            foreach (var name in ConfigHelper.Memory.SafeProvider.Keys)
            {
                var provider = GetProvider(name).Result;

                if (provider != null)
                {
                    Providers.Add(provider);
                }
            }

            if(Providers.Count > 0)
            {
                CropForms.ForEach(form => form.Show());
            }
            else
            {
                ConfigForm.Show();
            }
        }

        /// <summary>
        /// Load the required form for the given Provider.
        /// </summary>
        /// <param name="name"></param>
        public async void LoadProvider(string name)
        {
            var provider = await GetProvider(name);

            if (provider == null)
            {
                ConfigForm.Show();
            }
            else
            {
                Providers.Add(provider);
                CropForms.ForEach(form => form.Show());
            }
        }

        /// <summary>
        /// Get a Provider by name and give it back with a callback function.
        /// </summary>
        /// <param name="name"></param>
        private async Task<IProvider> GetProvider(string name)
        {
            IProvider newProvider;

            // Translate name into a real instance.
            switch (name)
            {
                case "Dropbox":
                    newProvider = new Dropbox.Provider();
                    break;
                case "GoogleDrive":
                    newProvider = new GoogleDrive.Provider();
                    break;
                case "OneDrive":
                    newProvider = new OneDrive.Provider();
                    break;
                case "LocalFile":
                    newProvider = new LocalFile.Provider();
                    break;
                default:
                    return null;
            }

            // Try to register Provider
            var savedState = ConfigHelper.Memory.SafeProvider.ContainsKey(name) ? ConfigHelper.Memory.SafeProvider[name] : null;
            var state = await newProvider.Register(savedState);

            if (state == null)
            {
                ToastFactory.Create("Failed to register provider!");
                return null;
            }

            // If the token is not changed, there was no new registration
            if (state != savedState)
            {
                ConfigHelper.Memory.SafeProvider[name] = state;
                ToastFactory.Create("Successfully registered provider!");
            }
            
            return newProvider;
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

                Providers.ForEach(async delegate(IProvider provider) {
                    url = await provider.Upload(name, stream);
                });
                
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

            Providers.ForEach(async delegate (IProvider provider) {
                url = await provider.Upload(name, stream);
            });
            
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
