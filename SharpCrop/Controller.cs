using SharpCrop.Forms;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using SharpCrop.Models;
using SharpCrop.Services;
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
            FormService.Init(this);
            Run();
        }

        /// <summary>
        /// Start the application async.
        /// </summary>
        private async void Run()
        {
            // If StartupRegister is enabled, init providers on the load of the first CropForm
            if (ConfigService.Current.StartupRegister)
            {
                await RestoreProviders();

                if (ProviderService.RegisteredProviders.Count > 0)
                {
                    FormService.ShowCropForms();
                }
                else
                {
                    FormService.ConfigForm.Show();
                }
            }
            else if (ConfigService.Current.SafeProviders.Count > 0)
            {
                // Show settings if no providers gonna be loaded - not safe, this is a prediction
                FormService.ShowCropForms();
            }
            else
            {
                FormService.ConfigForm.Show();
            }
        }

        /// <summary>
        /// Register providers from user settings, where they were previously saved.
        /// </summary>
        public async Task RestoreProviders()
        {
            // Load available IProvider types into memory
            if (ProviderService.LoadedProviders.Count == 0)
            {
                ProviderService.LoadProviders();
            }

            var tasks = new Dictionary<string, Task<bool>>();

            // ToList() is needed because the original Dictionary is changing while we iterating
            ConfigService.Current.SafeProviders
                .ToList()
                .ForEach(p => tasks[p.Key] = ProviderService.RegisterProvider(
                    ProviderService.GetProviderById(p.Value.Id), p.Key, p.Value.State, false));

            // Wait for tasks to finish and remove unloaded providers
            foreach (var task in tasks)
            {
                if (!ProviderService.RegisteredProviders.ContainsKey(task.Key) && !await task.Value)
                {
                    ToastService.Create(string.Format(Resources.ProviderRegistrationFailed, task.Key));
                    ProviderService.ClearProvider(task.Key);
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
            var bitmap = CaptureService.GetBitmap(region, offset);
            
            bitmap.Save(stream, ConfigService.Current.ImageFormat);

            // Copy bitmap to the clipboard
            if (!ConfigService.Current.NoImageCopy)
            {
                CopyImage(bitmap);
            }

            // Generate filename and start the upload(s)
            var url = await UploadAll(stream, ConfigService.Current.SafeImageExt);

            // Try to copy URL 
            if(ConfigService.Current.NoImageCopy)
            {
                CopyUrl(url); 
            }

            bitmap.Dispose();
            stream.Dispose();

            Complete(url != null); // Exits the application
        }

        /// <summary>
        /// Capture a lot of bitmaps and convert them to video.
        /// </summary>
        /// <param name="region"></param>
        /// <param name="offset"></param>
        public async void CaptureVideo(Rectangle region, Point offset)
        {
            MemoryStream stream;
            var toast = -1;

            // Create a new toast which closing event gonna stop the recording
            toast = ToastService.Create(Resources.StopRecording, Color.OrangeRed, 0, () =>
            {
                toast = ToastService.Create(Resources.Encoding, 0);
                CaptureService.StopRecording();
            });

            // Use Mpeg if enabled
            if (ConfigService.Current.EnableMpeg)
            {
                stream = await CaptureService.RecordMpeg(region, offset);
            }
            else
            {
                stream = await CaptureService.RecordGif(region, offset);
            }

            ToastService.Remove(toast);

            // Generate filename and start the upload(s)
            var url = await UploadAll(stream, ConfigService.Current.EnableMpeg ? "mp4" : "gif");

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
            var toast = ToastService.Create(string.Format(Resources.Uploading, $"{(double)stream.Length / (1024 * 1024):0.00} MB"), 0);

            // Try to load the saved providers (if load on startup is disabled)
            if (!ConfigService.Current.StartupRegister)
            {
                await RestoreProviders();
            }

            // Save file locally if no providers were registered
            if (ProviderService.RegisteredProviders.Count == 0)
            {
                File.WriteAllBytes(name, stream.ToArray());
                ToastService.Remove(toast);
                return null;
            }

            var uploads = new Dictionary<string, Task<string>>();

            string result = null;
            string last = null;

            // Run the uploads async
            foreach (var p in ProviderService.RegisteredProviders)
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
                    ToastService.Remove(toast);
                    ToastService.Create(string.Format(Resources.ProviderUploadFailed, p.Key));
                }
                else
                {
                    last = url;

                    if (p.Key == ConfigService.Current.CopyProvider)
                    {
                        result = url;
                    }
                }
            }

            ToastService.Remove(toast);

            // If the chosen URL was not found (or null), use the URL of the last successful one
            return string.IsNullOrEmpty(result) ? last : result;
        }

        /// <summary>
        /// Copy the given link the the clipboard.
        /// </summary>
        /// <param name="url"></param>
        private void CopyUrl(string url = null)
        {
            if (!ConfigService.Current.NoUrlCopy && !string.IsNullOrEmpty(url))
            {
                Clipboard.SetText(url);

                if (VersionService.GetPlatform() != PlatformType.Windows)
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
            if (!ConfigService.Current.NoImageCopy)
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
            ToastService.Create(success ? Resources.UploadCompleted : Resources.UploadFailed, 3000, () =>
            {
                if (VersionService.GetPlatform() == PlatformType.Windows)
                {
                    Application.Exit();
                }
            });
        }
    }
}