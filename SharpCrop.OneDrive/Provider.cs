using SharpCrop.Provider;
using System;
using System.IO;
using SharpCrop.Provider.Models;
using System.Threading.Tasks;
using Microsoft.OneDrive.Sdk;
using Newtonsoft.Json;
using Microsoft.Graph;
using SharpCrop.Provider.Forms;
using SharpCrop.OneDrive.Utils;
using SharpCrop.OneDrive.Models;

namespace SharpCrop.OneDrive
{
    public class Provider : IProvider
    {
        private OneDriveClient client;

        /// <summary>
        /// Try to create a new OneDriveClient with the given OneDrive auth provider.
        /// </summary>
        /// <param name="provider"></param>
        /// <returns></returns>
        private async Task<bool> ClientFactory(IAuthenticationProvider provider)
        {
            try
            {
                client = new OneDriveClient("https://api.onedrive.com/v1.0", provider);

                await client.Drive.Request().GetAsync();

                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Get an access token from OneDrive.
        /// </summary>
        /// <param name="token">Serialized TokenResponse from OneDrive Api.</param>
        /// <param name="onResult"></param>
        /// <returns></returns>
        public async Task Register(string token, Action<string, ProviderState> onResult)
        {
            var provider = new AuthProvider();

            if(!string.IsNullOrEmpty(token))
            {
                provider.Session = JsonConvert.DeserializeObject<TokenResponse>(token);

                if (await ClientFactory(provider))
                {
                    onResult(token, ProviderState.RefreshToken);
                    return;
                }
            }

            var form = new CodeForm(provider.Url);
            var success = false;

            form.OnCode(async code =>
            {
                success = true;

                form.Close();
                provider.ProcessCode(code);

                var newToken = JsonConvert.SerializeObject(provider.Session);

                if (await ClientFactory(provider))
                {
                    onResult(newToken, ProviderState.NewToken);
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

            System.Diagnostics.Process.Start(provider.Url);
            form.Show();
        }

        /// <summary>
        /// Upload the given memory stream with the attached filename to OneDrive.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="stream"></param>
        /// <returns></returns>
        public async Task<string> Upload(string name, MemoryStream stream)
        {
            await client.Drive.Special.AppRoot.Request().GetAsync();

            // This is needed, it will not work otherwise - I do not know why
            using (var newStream = new MemoryStream(stream.ToArray()))
            {
                await client.Drive.Special.AppRoot.Children[name].Content.Request().PutAsync<Item>(newStream);
            }

            var result = await client.Drive.Special.AppRoot.Children[name].CreateLink("view").Request().PostAsync();

            return result.Link.WebUrl;
        }
    }
}
