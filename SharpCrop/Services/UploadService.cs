using Dropbox.Api;
using Dropbox.Api.Files;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Net.Http;

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
            using (var stream = new MemoryStream())
            {
                bitmap.Save(stream, ImageFormat.Png);
                bitmap.Save(DateTime.Now.Ticks.ToString() + ".png", ImageFormat.Png);

                var response = client.Files.UploadAsync("/" + DateTime.Now.Ticks.ToString() + ".png", WriteMode.Overwrite.Instance, body: stream);

                bitmap.Dispose();
            }
        }
    }
}
