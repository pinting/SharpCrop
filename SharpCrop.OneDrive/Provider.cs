using SharpCrop.Provider;
using System.IO;
using System.Threading.Tasks;
using Microsoft.OneDrive.Sdk;
using Newtonsoft.Json;
using Microsoft.Graph;
using SharpCrop.Provider.Forms;
using SharpCrop.OneDrive.Utils;
using SharpCrop.OneDrive.Models;
using SharpCrop.Provider.Utils;

namespace SharpCrop.OneDrive
{
    /// <summary>
    /// This is a IProvider implementation for Microsoft OneDrive.
    /// </summary>
    public class Provider : IProvider
    {
        private OneDriveClient client;
        
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
        /// Get an AccessToken from OneDrive.
        /// </summary>
        /// <param name="savedState">Previous serialized TokenResponse from OneDrive.</param>
        /// <returns></returns>
        public async Task<string> Register(string savedState, bool showForm = true)
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

            // If the saved token was not usable and showForm is false, return failure
            if(!showForm)
            {
                result.SetResult(null);
                return await result.Task;
            }
            
            // Create a CodeForm and open OneDrive token request link to obtain a new token
            var form = new CodeForm(provider.Url, 37);
            var success = false;

            form.OnResult(async code =>
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
            });

            form.FormClosed += (sender, e) =>
            {
                if (!success)
                {
                    result.SetResult(null);
                }
            };

            System.Diagnostics.Process.Start(provider.Url);
            form.Show();

            return await result.Task;
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
