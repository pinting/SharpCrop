using SharpCrop.Provider;
using System;
using Dropbox.Api;
using Dropbox.Api.Files;
using System.IO;
using System.Net.Http;
using SharpCrop.Provider.Models;

namespace SharpCrop.Dropbox
{
    public class Provider : IProvider
    {
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

        public void Register(string token, Action<string, ProviderState> onResult)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Upload the given memory stream with the attached filename to Dropbox.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="stream"></param>
        /// <returns></returns>
        public string Upload(string name, MemoryStream stream)
        {
            var path = "/" + name;

            // This is needed, because Dropbox API will not work otherwise - I do not know why
            using (var newStream = new MemoryStream(stream.ToArray()))
            {
                client.Files.UploadAsync(path, WriteMode.Overwrite.Instance, body: newStream).Wait();
            }

            var meta = client.Sharing.CreateSharedLinkWithSettingsAsync(path).Result;

            return meta.Url;
        }
    }
}
