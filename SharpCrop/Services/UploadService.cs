using Dropbox.Api;
using Dropbox.Api.Files;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Net.Http;
using System.Windows.Forms;

namespace SharpCrop.Services
{
    class UploadService
    {
        private DropboxClient client;

        public UploadService(string accessToken)
        {
            var httpClient = new HttpClient(new WebRequestHandler { ReadWriteTimeout = 10 * 1000 }) { Timeout = TimeSpan.FromMinutes(20) };
            var config = new DropboxClientConfig("SharpCrop") { HttpClient = httpClient };

            client = new DropboxClient(accessToken, config);
        }

        public void UploadBitmap(Bitmap bitmap)
        {
            var path = "/" + DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss") + ".png";
            byte[] byteArray;

            using (var stream = new MemoryStream())
            {
                bitmap.Save(stream, ImageFormat.Png);
                stream.Close();

                byteArray = stream.ToArray();
            }

            using (var stream = new MemoryStream(byteArray))
            {
                client.Files.UploadAsync(path, WriteMode.Overwrite.Instance, body: stream).Wait();
            }

            var meta = client.Sharing.CreateSharedLinkWithSettingsAsync(path).Result;
            
            Clipboard.SetText(meta.Url);
        }
    }
}
