using SharpCrop.Provider;
using System;
using System.Drawing;
using Dropbox.Api;
using Dropbox.Api.Files;
using System.Drawing.Imaging;
using System.IO;
using System.Net.Http;
using SharpCrop.Provider.Models;

namespace SharpCrop.Dropbox
{
    public class Provider : IProvider
    {
        public static readonly string ClientId = "cou3krww0do592i";

        private DropboxClientConfig config;
        private HttpClient httpClient;
        private DropboxClient client;

        /// <summary>
        /// Provider is responsible for handling the communication with a service - like Dropbox.
        /// </summary>
        public Provider()
        {
            httpClient = new HttpClient(new WebRequestHandler { ReadWriteTimeout = 10 * 1000 }) { Timeout = TimeSpan.FromMinutes(20) };
            config = new DropboxClientConfig("SharpCrop") { HttpClient = httpClient };
        }

        /// <summary>
        /// Register for the service. If an old token was given, it gonna try to use it. If it was not given or it
        /// was expired, it will try to request a new one. Eventully onResult will be called with the something.
        /// </summary>
        /// <param name="token"></param>
        /// <param name="onResult"></param>
        public void Register(string token, Action<string, ProviderState> onResult)
        {
            try
            {
                if(token == null)
                {
                    // Goto catch phrase
                    throw new Exception();
                }

                // Create a new client object and test if the AccessToken is valid
                client = new DropboxClient(token, config);
                client.Users.GetSpaceUsageAsync().Wait();

                onResult(token, ProviderState.Normal);
            }
            catch
            {
                var form = new Forms.MainForm((newToken, state) =>
                {
                    if(newToken != null)
                    {
                        client = new DropboxClient(newToken, config);
                        onResult(newToken, ProviderState.Renewed);
                    }
                    else
                    {
                        onResult(newToken, state);
                    }
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
