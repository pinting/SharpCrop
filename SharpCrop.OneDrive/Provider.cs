using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Graph;
using Microsoft.OneDrive.Sdk;
using Newtonsoft.Json;
using SharpCrop.OneDrive.Models;
using SharpCrop.OneDrive.Properties;
using SharpCrop.OneDrive.Utils;
using SharpCrop.Provider;
using SharpCrop.Provider.Forms;
using SharpCrop.Provider.Utils;

namespace SharpCrop.OneDrive
{
    /// <summary>
    /// This is a IProvider implementation for Microsoft OneDrive.
    /// </summary>
    public class Provider : IProvider
    {
        private OneDriveClient client;

        public string Id => Config.ProviderId;

        public string Name => Resources.ProviderName;

        /// <summary>
        /// Try to create a new OneDriveClient.
        /// </summary>
        /// <param name="provider">Auth provider which signs every request.</param>
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
        /// Get an access token from OneDrive - or try to use an existing one.
        /// </summary>
        /// <param name="savedState">Previous serialized TokenResponse from OneDrive.</param>
        /// <param name="silent"></param>
        /// <returns></returns>
        public async Task<string> Register(string savedState = null, bool silent = false)
        {
            var result = new TaskCompletionSource<string>();
            var provider = new AuthProvider();

            // Check if there is a saved token and try to use it
            if (!string.IsNullOrEmpty(savedState))
            {
                provider.Session = JsonConvert.DeserializeObject<TokenResponse>(Obscure.Base64Decode(savedState));

                if (await ClientFactory(provider))
                {
                    result.SetResult(savedState);
                    return await result.Task;
                }
            }

            // If the saved state was not usable and silent is true, return with failure
            if(silent)
            {
                result.SetResult(null);
                return await result.Task;
            }
            
            // Create a CodeForm and open OneDrive token request link to obtain a new token
            var form = new CodeForm(provider.Url, 37);
            var success = false;

            form.OnResult += async code =>
            {
                success = true;

                form.Close();
                provider.ProcessCode(code);

                if (await ClientFactory(provider))
                {
                    result.SetResult(Obscure.Base64Encode(JsonConvert.SerializeObject(provider.Session)));
                }
                else
                {
                    result.SetResult(null);
                }
            };

            form.FormClosed += (sender, e) =>
            {
                if (!success)
                {
                    result.SetResult(null);
                }
            };

            Process.Start(provider.Url);
            form.Show();

            return await result.Task;
        }

        /// <summary>
        /// Upload the given memory stream with the attached filename to OneDrive.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="stream"></param>
        /// <returns>Shared link.</returns>
        public async Task<string> Upload(string name, MemoryStream stream)
        {
            await client.Drive.Special.AppRoot.Request().GetAsync();

            // This is needed, it will not work otherwise - I do not know why
            // TODO: Why?
            using (var newStream = new MemoryStream(stream.ToArray()))
            {
                await client.Drive.Special.AppRoot.Children[name].Content.Request().PutAsync<Item>(newStream);
            }

            var result = await client.Drive.Special.AppRoot.Children[name].CreateLink("view").Request().PostAsync();

            return result.Link.WebUrl;
        }
    }
}
