using SharpCrop.Provider;
using System.IO;
using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using System.Threading;
using SharpCrop.Provider.Utils;
using SharpCrop.GoogleDrive.Utils;

namespace SharpCrop.GoogleDrive
{
    /// <summary>
    /// An IProvider implementation for Google Drive. 
    /// </summary>
    public class Provider : IProvider
    {
        private DriveService service;
        
        /// <summary>
        /// Get an access token from Google Drive.
        /// </summary>
        /// <param name="token">Serialized TokenResponse from Google Api.</param>
        /// <returns></returns>
        public async Task<string> Register(string token)
        {
            var result = new TaskCompletionSource<string>();

            try
            {
                // Try to use the previously saved TokenResponse
                var store = new MemoryStore(token);
                var receiver = new CodeReceiver();
                var secret = new ClientSecrets() { ClientId = Obscure.Decode(Constants.AppKey), ClientSecret = Obscure.Decode(Constants.AppSecret) };
                var credential = await GoogleWebAuthorizationBroker.AuthorizeAsync(secret, Constants.Scopes, "token", CancellationToken.None, store, receiver);

                service = new DriveService(new BaseClientService.Initializer()
                {
                    HttpClientInitializer = credential,
                    ApplicationName = "SharpCrop",
                });

                // Export the serialized TokenResponse which gonna be saved into the config
                result.SetResult(store.Export());
            }
            catch
            {
                result.SetResult(null);
            }

            return await result.Task;
        }

        /// <summary>
        /// Upload a stream with the given name to Google Drive.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="stream"></param>
        /// <returns></returns>
        public async Task<string> Upload(string name, MemoryStream stream)
        {
            var body = new Google.Apis.Drive.v3.Data.File() { Name = name };
            var type = $"image/{MimeLookup.Find(Path.GetExtension(name))}";
            var request = service.Files.Create(body, stream, type);

            await request.UploadAsync();

            var permission = service.Permissions.Create(new Google.Apis.Drive.v3.Data.Permission() { Type = "anyone", Role = "reader" }, request.ResponseBody.Id);

            await permission.ExecuteAsync();

            return $"https://drive.google.com/open?id={request.ResponseBody.Id}";
        }
    }
}
