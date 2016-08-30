using SharpCrop.Provider;
using System;
using Dropbox.Api;
using Dropbox.Api.Files;
using System.IO;
using SharpCrop.Provider.Models;
using SharpCrop.Provider.Utils;
using System.Windows.Forms;
using System.Threading.Tasks;
using SharpCrop.Provider.Forms;

namespace SharpCrop.Dropbox
{
    public class Provider : IProvider
    {
        private DropboxClient client;
        
        /// <summary>
        /// Create a new DropboxClient and test it.
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        private async Task<bool> ClientFactory(string token)
        {
            try
            {
                client = new DropboxClient(token);

                // Test if the token is valid with a simple action
                await client.Users.GetSpaceUsageAsync();

                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Register for the service. If an old token was given, it gonna try to use it. If it was not given or it
        /// was expired, it will try to request a new one. Eventully onResult will be called with the something.
        /// </summary>
        /// <param name="token"></param>
        /// <param name="onResult"></param>
        public async Task Register(string token, Action<string, ProviderState> onResult)
        {
            if(await ClientFactory(token))
            {
                onResult(token, ProviderState.RefreshToken);
                return;
            }

            var url = DropboxOAuth2Helper.GetAuthorizeUri(
                    OAuthResponseType.Code,
                    Obscure.Decode(Constants.AppKey),
                    (string)null);

            var form = new CodeForm(url.ToString());
            var success = false;

            form.OnCode(async code =>
            {
                success = true;

                form.Close();
                Application.DoEvents();

                var result = await DropboxOAuth2Helper.ProcessCodeFlowAsync(
                    code,
                    Obscure.Decode(Constants.AppKey),
                    Obscure.Decode(Constants.AppSecret));

                if (result != null && result.AccessToken != null && await ClientFactory(result.AccessToken))
                {
                    onResult(result.AccessToken, ProviderState.NewToken);
                }
                else
                {
                    onResult(null, ProviderState.UnknownError);
                }
            });

            form.FormClosed += (sender, e) =>
            {
                if (!success)
                {
                    onResult(null, ProviderState.UserError);
                }
            };

            System.Diagnostics.Process.Start(url.ToString());
            form.Show();
        }

        /// <summary>
        /// Upload the given memory stream with the attached filename to Dropbox.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="stream"></param>
        /// <returns></returns>
        public async Task<string> Upload(string name, MemoryStream stream)
        {
            var path = "/" + name;

            // This is needed, because Dropbox API will not work otherwise - I do not know why
            using (var newStream = new MemoryStream(stream.ToArray()))
            {
                await client.Files.UploadAsync(path, WriteMode.Overwrite.Instance, body: newStream);
            }

            var meta = await client.Sharing.CreateSharedLinkWithSettingsAsync(path);

            return meta.Url;
        }
    }
}
