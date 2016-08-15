using SharpCrop.Provider;
using System;
using System.Drawing;
using Dropbox.Api;
using Dropbox.Api.Files;
using System.Drawing.Imaging;
using System.IO;
using System.Net.Http;

namespace SharpCrop.Dropbox
{
    public class Provider : IProvider
    {
        private DropboxClientConfig config;
        private HttpClient httpClient;
        private DropboxClient client;

        public Provider()
        {
            httpClient = new HttpClient(new WebRequestHandler { ReadWriteTimeout = 10 * 1000 }) { Timeout = TimeSpan.FromMinutes(20) };
            config = new DropboxClientConfig("SharpCrop") { HttpClient = httpClient };
        }

        public void Register(string token, Action<string> onResult)
        {
            try
            {
                // Create a new client object and test if the AccessToken is valid
                client = new DropboxClient(token, config);
                client.Users.GetSpaceUsageAsync().Wait();

                onResult(token);
            }
            catch
            {
                var form = new Forms.MainForm(newToken =>
                {
                    if(newToken == null)
                    {
                        onResult(null);
                        return;
                    }

                    client = new DropboxClient(newToken, config);
                    onResult(newToken);
                });

                form.Show();
            }
        }
        
        /// <summary>
        /// Upload the given bitmap to Dropbox.
        /// </summary>
        /// <param name="bitmap"></param>
        /// <returns>URL of the uploaded file.</returns>
        public string Upload(Bitmap bitmap)
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
