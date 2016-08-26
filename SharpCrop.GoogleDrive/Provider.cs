using SharpCrop.Provider;
using System;
using System.IO;
using SharpCrop.Provider.Models;
using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using System.Threading;
using SharpCrop.Provider.Utils;
using SharpCrop.GoogleDrive.Utils;
using Google.Apis.Auth.OAuth2.Responses;
using Google.Apis.Auth.OAuth2.Requests;
using SharpCrop.Provider.Forms;
using System.Windows.Forms;

namespace SharpCrop.GoogleDrive
{
    public class Provider : IProvider
    {
        private DriveService service;

        /// <summary>
        /// Code receiver class for Google Drive.
        /// </summary>
        private sealed class CodeReceiver : ICodeReceiver
        {
            public string RedirectUri
            {
                get
                {
                    return GoogleAuthConsts.InstalledAppRedirectUri;
                }
            }

            /// <summary>
            /// Waiting for Google Drive code.
            /// </summary>
            /// <param name="authUrl"></param>
            /// <param name="taskCancellationToken"></param>
            /// <returns></returns>
            public Task<AuthorizationCodeResponseUrl> ReceiveCodeAsync(AuthorizationCodeRequestUrl authUrl, CancellationToken taskCancellationToken)
            {
                var result = new TaskCompletionSource<AuthorizationCodeResponseUrl>();
                var url = authUrl.Build().ToString();

                var form = new CodeForm(url);
                var success = false;

                form.OnCode(code =>
                {
                    success = true;

                    form.Close();
                    result.SetResult(new AuthorizationCodeResponseUrl() { Code = code });
                });

                form.FormClosed += (sender, e) =>
                {
                    if (!success)
                    {
                        result.SetResult(null);
                    }
                };

                System.Diagnostics.Process.Start(url);
                form.Show();

                return result.Task;
            }
        }

        /// <summary>
        /// Get an access token from Google Drive.
        /// </summary>
        /// <param name="savedState">Serialized TokenResponse from Google Api.</param>
        /// <param name="onResult"></param>
        /// <returns></returns>
        public async Task Register(string savedState, Action<string, ProviderState> onResult)
        {
            try
            {
                var store = new MemoryStore(savedState);
                var receiver = new CodeReceiver();

                var oldToken = await store.GetAsync<TokenResponse>("token");

                var secret = new ClientSecrets() { ClientId = Obscure.Decode(Constants.AppKey), ClientSecret = Obscure.Decode(Constants.AppSecret) };
                var credential = await GoogleWebAuthorizationBroker.AuthorizeAsync(secret, Constants.Scopes, "token", CancellationToken.None, store, receiver);

                service = new DriveService(new BaseClientService.Initializer()
                {
                    HttpClientInitializer = credential,
                    ApplicationName = "SharpCrop",
                });

                if (oldToken != null && credential.Token != null && oldToken.AccessToken == credential.Token.AccessToken)
                {
                    onResult(store.Export(), ProviderState.RefreshToken);
                }
                else
                {
                    onResult(store.Export(), ProviderState.NewToken);
                }
            }
            catch
            {
                onResult(null, ProviderState.UnknownError);
            }
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
            var type = "image/" + Path.GetExtension(name).Substring(1);
            var request = service.Files.Create(body, stream, type);

            await request.UploadAsync();

            var permission = service.Permissions.Create(new Google.Apis.Drive.v3.Data.Permission() { Type = "anyone", Role = "reader" }, request.ResponseBody.Id);

            await permission.ExecuteAsync();

            return string.Format("https://drive.google.com/open?id={0}", request.ResponseBody.Id);
        }
    }
}
