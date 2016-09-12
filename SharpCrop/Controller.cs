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
        private readonly List<Form> cropForms = new List<Form>();
        private IProvider provider;
        private Form mainForm;
        
        /// <summary>
        /// Construct a new Controller class.
        /// </summary>
        public Controller()
        {
            foreach (var screen in Screen.AllScreens)
            {
                var form = new CropForm(this, screen.Bounds);

                form.FormClosed += (s, e) => Application.Exit();
                cropForms.Add(form);
            }

            LoadProvider(ConfigHelper.Memory.Provider);
        }

        /// <summary>
        /// Protect cropForms from external modification.
        /// </summary>
        public IReadOnlyList<Form> CropForms
        {
            get
            {
                return cropForms;
            }
        }

        /// <summary>
        /// Load the required form for the given Provider.
        /// </summary>
        /// <param name="name"></param>
        public async void LoadProvider(string name)
        {
            provider = await GetProvider(name);

            if (provider != null)
            {
                cropForms.ForEach(form => form.Show());
                return;
            }

            if (mainForm == null)
            {
                mainForm = new MainForm(this);
                mainForm.FormClosed += (s, e) => Application.Exit();
            }

            mainForm.Show();
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
            var token = await newProvider.Register(ConfigHelper.Memory.Token);

            if (token == null)
            {
                ToastFactory.Create("Failed to register provider!");
                return null;
            }

            // If the token is not changed, there was no new registration
            if (token != ConfigHelper.Memory.Token)
            {
                ConfigHelper.Memory.Provider = name;
                ConfigHelper.Memory.Token = token;
                ToastFactory.Create("Successfully registered provider!");
            }
            
            return newProvider;
        }

        /// <summary>
        /// Capture one Bitmap.
        /// </summary>
        /// <param name="rect"></param>
        public async void CaptureImage(Rectangle rect)
        {
            var bitmap = CaptureHelper.GetBitmap(rect);
            var stream = new MemoryStream();

            bitmap.Save(stream, ConfigHelper.Memory.FormatType);

            var name = DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss") + "." + ConfigHelper.Memory.FormatExt;
            var url = await provider.Upload(name, stream);

            stream.Dispose();
            Success(url);
        }

        /// <summary>
        /// Capture a lot of Bitmaps and convert them to Gif.
        /// </summary>
        /// <param name="rect"></param>
        public async void CaptureGif(Rectangle rect)
        {
            var toast = -1;

            ToastFactory.Create("Click here to stop!", Color.OrangeRed, 0, () => 
            {
                toast = ToastFactory.Create("Encoding...", 0);
                GifFactory.Stop();
            });

            var stream = await GifFactory.Record(rect);

            ToastFactory.Remove(toast);

            toast = ToastFactory.Create($"Uploading... ({(double) stream.Length/(1024*1024):0.00} MB)", 0);

            var name = DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss") + ".gif";
            var url = await provider.Upload(name, stream);
            
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
            if (ConfigHelper.Memory.Copy && url != null)
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
