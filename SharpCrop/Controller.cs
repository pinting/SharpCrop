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
                await RestoreProviders();
            }

            // Save file locally if no providers were registered
            if (ProviderManager.RegisteredProviders.Count == 0)
            {
                File.WriteAllBytes(name, stream.ToArray());
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
        /// Show the end notifcation. If the url is null, the text gonna tell the user, that the upload has failed.
        /// </summary>
        /// <param name="url"></param>
        private static void CompleteCapture(string url = null)
        {
            if (!ConfigHelper.Current.NoCopy && !string.IsNullOrEmpty(url))
            {
                Clipboard.SetText(url);

                if (VersionHelper.GetSystemType() != SystemType.Windows)
                {
                    var form = new CopyForm(url);
                    form.FormClosed += (s, e) => Application.Exit();
                    form.Show();
                }
            }

            ToastFactory.Create(string.IsNullOrEmpty(url) ? Resources.UploadFailed : Resources.UploadCompleted, 3000, () =>
            {
                if (VersionHelper.GetSystemType() != SystemType.Windows)
                {
                    Application.Exit();
                }
            });
        }
    }
}