using SharpCrop.Forms;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using SharpCrop.Models;
using SharpCrop.Modules;
using SharpCrop.Properties;

namespace SharpCrop
{
    /// <summary>
    /// Controller is responsible for the loading of the application. It will also
    /// manage image capturing and uploading.
    /// </summary>
    public class Controller : ApplicationContext
    {
        /// <summary>
        /// Construct a new Controller class.
        /// </summary>
        public Controller()
        {
            FormManager.Init(this);
            Run();
        }

        /// <summary>
        /// Start the application async.
        /// </summary>
        private async void Run()
        {
            // If StartupRegister is enabled, init providers on the load of the first CropForm
            if (ConfigHelper.Current.StartupRegister)
            {
                await RestoreProviders();

                if (ProviderManager.RegisteredProviders.Count > 0)
                {
                    FormManager.ShowCropForms();
                }
                else
                {
                    FormManager.ConfigForm.Show();
                }
            }
            else if (ConfigHelper.Current.SafeProviders.Count > 0)
            {
                // Show settings if no providers gonna be loaded - not safe, this is a prediction
                FormManager.ShowCropForms();
            }
            else
            {
                FormManager.ConfigForm.Show();
            }
        }

        /// <summary>
        /// Register providers from user settings, where they were previously saved.
        /// </summary>
        public async Task RestoreProviders()
        {
            // Load available IProvider types into memory
            if (ProviderManager.LoadedProviders.Count == 0)
            {
                ProviderManager.LoadProviders();
            }

            var tasks = new Dictionary<string, Task<bool>>();

            // ToList() is needed because the original Dictionary is changing while we iterating
            ConfigHelper.Current.SafeProviders
                .ToList()
                .ForEach(p => tasks[p.Key] = ProviderManager.RegisterProvider(
                    ProviderManager.GetProviderById(p.Value.Id), p.Key, p.Value.State, false));

            // Wait for tasks to finish and remove unloaded providers
            foreach (var task in tasks)
            {
                if (!ProviderManager.RegisteredProviders.ContainsKey(task.Key) && !await task.Value)
                {
                    ToastFactory.Create(string.Format(Resources.ProviderRegistrationFailed, task.Key));
                    ProviderManager.ClearProvider(task.Key);
                }
            }
        }

        /// <summary>
        /// Capture one Bitmap.
        /// </summary>
        /// <param name="region"></param>
        /// <param name="offset"></param>
        public async void CaptureImage(Rectangle region, Point offset)
        {
            var stream = new MemoryStream();
            var bitmap = CaptureHelper.GetBitmap(region, offset);
            
            bitmap.Save(stream, ConfigHelper.Current.ImageFormat);

            // Copy bitmap to the clipboard
            if (!ConfigHelper.Current.NoImageCopy)
            {
                CopyImage(bitmap);
            }

            // Generate filename and start the upload(s)
            var url = await UploadAll(stream, ConfigHelper.Current.SafeImageExt);

            // Try to copy URL 
            if(ConfigHelper.Current.NoImageCopy)
            {
                CopyUrl(url); 
            }

            bitmap.Dispose();
            stream.Dispose();

            Complete(url != null); // Exits the application
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
                CaptureHelper.StopRecording();
            });

            // Use Mpeg if enabled
            if (ConfigHelper.Current.EnableMpeg)
            {
                stream = await CaptureHelper.RecordMpeg(region, offset);
            }
            else
            {
                stream = await CaptureHelper.RecordGif(region, offset);
            }

            ToastFactory.Remove(toast);

            // Generate filename and start the upload(s)
            var url = await UploadAll(stream, ConfigHelper.Current.EnableMpeg ? "mp4" : "gif");

            stream.Dispose();

            CopyUrl(url);
            Complete(url != null); // Exits the application
        }

        /// <summary>
        /// Upload the given stream with all loaded providers.
        /// </summary>
        /// <param name="ext"></param>
        /// <param name="stream"></param>
        /// <returns></returns>
        private async Task<string> UploadAll(MemoryStream stream, string ext)
        {
            var name = $"{DateTime.Now:yyyy_MM_dd_HH_mm_ss}.{ext}";
            var toast = ToastFactory.Create(string.Format(Resources.Uploading, $"{(double)stream.Length / (1024 * 1024):0.00} MB"), 0);

            // Try to load the saved providers (if load on startup is disabled)
            if (!ConfigHelper.Current.StartupRegister)
            {
                await RestoreProviders();
            }

            // Save file locally if no providers were registered
            if (ProviderManager.RegisteredProviders.Count == 0)
            {
                File.WriteAllBytes(name, stream.ToArray());
                ToastFactory.Remove(toast);
                return null;
            }

            var uploads = new Dictionary<string, Task<string>>();

            string result = null;
            string last = null;

            // Run the uploads async
            foreach (var p in ProviderManager.RegisteredProviders)
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
                    ToastFactory.Remove(toast);
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

            ToastFactory.Remove(toast);

            // If the chosen URL was not found (or null), use the URL of the last successful one
            return string.IsNullOrEmpty(result) ? last : result;
        }

        /// <summary>
        /// Copy the given link the the clipboard.
        /// </summary>
        /// <param name="url"></param>
        private void CopyUrl(string url = null)
        {
            if (!ConfigHelper.Current.NoUrlCopy && !string.IsNullOrEmpty(url))
            {
                Clipboard.SetText(url);

                if (VersionHelper.GetSystemType() != SystemType.Windows)
                {
                    var form = new CopyForm(url);

                    form.FormClosed += (s, e) => Application.Exit();
                    form.Show();
                }
            }
        }

        /// <summary>
        /// Copy the given image to the clipboard if this feature is enabled in the config.
        /// </summary>
        /// <param name="image"></param>
        private void CopyImage(Image image)
        {
            if (!ConfigHelper.Current.NoImageCopy)
            {
                Clipboard.SetImage(image);
            }
        }

        /// <summary>
        /// Show the end notifcation.
        /// </summary>
        /// <param name="success">If success is false, the text gonna tell the user, that the upload has failed.</param>
        private void Complete(bool success)
        {
            ToastFactory.Create(success ? Resources.UploadCompleted : Resources.UploadFailed, 3000, () =>
            {
                if (VersionHelper.GetSystemType() == SystemType.Windows)
                {
                    Application.Exit();
                }
            });
        }
    }
}