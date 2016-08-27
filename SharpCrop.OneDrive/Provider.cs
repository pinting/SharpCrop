using SharpCrop.Provider;
using System;
using System.IO;
using SharpCrop.Provider.Models;
using System.Threading.Tasks;
using Microsoft.OneDrive.Sdk.Authentication;
using Microsoft.OneDrive.Sdk;
using SharpCrop.Provider.Utils;
using Newtonsoft.Json;
using Microsoft.Graph;

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
        /// <param name="savedState">Serialized TokenResponse from Google Api.</param>
        /// <param name="onResult"></param>
        /// <returns></returns>
        public async Task Register(string token, Action<string, ProviderState> onResult)
        {
            try
            {
                var result = ProviderState.NewToken;
                var cache = new CredentialCache();
                var provider = new MsaAuthenticationProvider(
                    Obscure.Decode(Constants.AppKey),
                    Obscure.Decode(Constants.AppSecret),
                    Constants.RedirectUrl,
                    Constants.Scopes,
                    cache);

                if (string.IsNullOrEmpty(token))
                {
                    await provider.AuthenticateUserAsync();
                }
                else
                {
                    provider.CurrentAccountSession = JsonConvert.DeserializeObject<AccountSession>(token);
                    result = ProviderState.RefreshToken;
                }

                if(await ClientFactory(provider))
                {
                    onResult(JsonConvert.SerializeObject(provider.CurrentAccountSession), result);
                }
                else
                {
                    await Register(null, onResult);
                }
            }
            catch
            {
                onResult(null, ProviderState.UnknownError);
            }
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
