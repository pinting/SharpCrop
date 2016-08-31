using SharpCrop.Forms;
using SharpCrop.Provider;
using SharpCrop.Utils;
using SharpCrop.Utils.NGif;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        private IProvider provider;
        private Form clickForm;
        private Form mainForm;

        /// <summary>
        /// Construct a new Controller class.
        /// </summary>
        public Controller()
        {
            // Needed to construct these here to be in the right SynchronizationContext
            clickForm = new ClickForm(this);
            clickForm.FormClosed += (s, e) => Application.Exit();

            LoadProvider(ConfigHelper.Memory.Provider);
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
                clickForm.Show();
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
            IProvider provider;

            // Translate name into a real instance.
            switch (name)
            {
                case "Dropbox":
                    provider = new Dropbox.Provider();
                    break;
                case "GoogleDrive":
                    provider = new GoogleDrive.Provider();
                    break;
                case "OneDrive":
                    provider = new OneDrive.Provider();
                    break;
                case "LocalFile":
                    provider = new LocalFile.Provider();
                    break;
                default:
                    return null;
            }

            // Try to register Provider
            var token = await provider.Register(ConfigHelper.Memory.Token);

            if (token == null)
            {
                ToastFactory.CreateToast("Failed to register provider!");
                return null;
            }

            // If the token is not changed, there was no new registration
            if (token != ConfigHelper.Memory.Token)
            {
                ConfigHelper.Memory.Provider = name;
                ConfigHelper.Memory.Token = token;
                ToastFactory.CreateToast("Successfully registered provider!");
            }
            
            return provider;
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
        /// REALLY-REALLY SLOW!
        /// </summary>
        /// <param name="rect"></param>
        public async void CaptureGif(Rectangle rect)
        {
            var stream = new MemoryStream();
            var run = true;

            ToastFactory.CreateToast("Click here to stop!", 1000 * 60, () => run = false);
            
            await Task.Run(async () =>
            {
                var gif = new AnimatedGifEncoder();
                var frames = new List<Bitmap>();
                var duration = Stopwatch.StartNew();

                Stopwatch delay = null;

                while (run)
                {
                    var wait = 0;

                    if (delay != null)
                    {
                        wait = ConfigHelper.Memory.RealGifFps - (int)delay.ElapsedMilliseconds;
                        wait = wait < 0 ? 0 : wait;
                    }

                    delay = Stopwatch.StartNew();

                    frames.Add(CaptureHelper.GetBitmap(rect));
                    await Task.Delay(wait);

                    delay.Stop();
                }

                duration.Stop();

                gif.Start(stream);
                gif.SetQuality(Constants.GifQuality);
                gif.SetRepeat(ConfigHelper.Memory.GifRepeat ? 0 : 1);

                for (var i = 0; i < frames.Count; i++)
                {
                    gif.AddFrame(frames[i]);
                }

                gif.SetDelay((int)duration.ElapsedMilliseconds / frames.Count);
                gif.Finish();
            });

            ToastFactory.CreateToast(string.Format("Uploading... ({0:0.00} MB)", (double)stream.Length / (1024 * 1024)));

            var name = DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss") + ".gif";
            var url = await provider.Upload(name, stream);

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

            ToastFactory.CreateToast("Uploaded successfully!", 3000, () =>
            {
#if !__MonoCS__
                Application.Exit();
#endif
            });
        }
    }
}
