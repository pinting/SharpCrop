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

        /// <summary>
        /// Upload service which communicates with Dropbox through the API.
        /// </summary>
        /// <param name="accessToken">AccessToken which is obtained by Auth namespace.</param>
        public UploadService(string accessToken)
        {
            var httpClient = new HttpClient(new WebRequestHandler { ReadWriteTimeout = 10 * 1000 }) { Timeout = TimeSpan.FromMinutes(20) };
            var config = new DropboxClientConfig("SharpCrop") { HttpClient = httpClient };

            // Create a new client object and test if the AccessToken is valid
            client = new DropboxClient(accessToken, config);
            client.Users.GetSpaceUsageAsync().Wait();
        }

        /// <summary>
        /// Upload the given bitmap to Dropbox.
        /// </summary>
        /// <param name="bitmap"></param>
        /// <returns>URL of the uploaded file.</returns>
        public string UploadBitmap(Bitmap bitmap)
        {
            var path = "/" + DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss") + ".png";
            byte[] byteArray;

            // ByteArray is needed, because Dropbox API will not work with Bitmap
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
            
            return meta.Url;
        }
    }
}
