using SharpCrop.Provider;
using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using System.Threading;
using SharpCrop.Provider.Utils;
using SharpCrop.GoogleDrive.Utils;
using System.Collections.Generic;
using Google.Apis.Drive.v3.Data;

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
        /// <param name="savedState">Serialized TokenResponse from Google Api.</param>
        /// <param name="showForm"></param>
        /// <returns></returns>
        public async Task<string> Register(string savedState, bool showForm = true)
        {
            var result = new TaskCompletionSource<string>();

            try
            {
                // Try to use the previously saved TokenResponse
                var receiver = new CodeReceiver();
                var store = new MemoryStore(Obscure.Base64Decode(savedState));
                var secret = new ClientSecrets() { ClientId = Obscure.Decode(Constants.AppKey), ClientSecret = Obscure.Decode(Constants.AppSecret) };
                var credential = await GoogleWebAuthorizationBroker.AuthorizeAsync(secret, Constants.Scopes, "token", CancellationToken.None, store, receiver);

                service = new DriveService(new BaseClientService.Initializer()
                {
                    HttpClientInitializer = credential,
                    ApplicationName = "SharpCrop",
                });

                // Export the serialized TokenResponse which gonna be saved into the config
                result.SetResult(Obscure.Base64Encode(store.Export()));
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
        public async Task<string> Upload(string name, System.IO.MemoryStream stream)
        {
            var folderBody = new File() { Name = Constants.FolderName, Description = Constants.FolderDescription, MimeType = "application/vnd.google-apps.folder" };
            var listRequest = service.Files.List();

            File folder;

            listRequest.Q = $"name = '{folderBody.Name}' and fullText contains '{folderBody.Description}' and mimeType = '{folderBody.MimeType}' and trashed = false";
            listRequest.Spaces = "drive";
            listRequest.Fields = "files(id)";

            var result = listRequest.Execute();

            if (result.Files.Count > 0)
            {
                folder = result.Files[0];
            }
            else
            {
                var createRequest = service.Files.Create(folderBody);

                createRequest.Fields = "id";
                folder = await createRequest.ExecuteAsync();
            }

            var uploadBody = new File() { Name = name,  Parents = new List<string> { folder.Id } };
            var uploadType = $"image/{MimeLookup.Find(System.IO.Path.GetExtension(name))}";
            var uploadRequest = service.Files.Create(uploadBody, stream, uploadType);

            await uploadRequest.UploadAsync();

            var uploadPermission = service.Permissions.Create(new Permission() { Type = "anyone", Role = "reader" }, uploadRequest.ResponseBody.Id);

            await uploadPermission.ExecuteAsync();

            return $"https://drive.google.com/open?id={uploadRequest.ResponseBody.Id}";
        }
    }
}
