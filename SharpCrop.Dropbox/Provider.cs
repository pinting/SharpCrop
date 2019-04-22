using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Dropbox.Api;
using Dropbox.Api.Files;
using SharpCrop.Dropbox.Properties;
using SharpCrop.Provider;
using SharpCrop.Provider.Forms;
using SharpCrop.Provider.Utils;

namespace SharpCrop.Dropbox
{
    /// <summary>
    /// IProvider implementation for Dropbox.
    /// </summary>
    public class Provider : IProvider
    {
        private DropboxClient client;

        public string Id => Config.ProviderId;

        public string Name => Resources.ProviderName;

        /// <summary>
        /// Create a new DropboxClient and test it.
        /// </summary>
        /// <param name="savedState"></param>
        /// <returns></returns>
        private async Task<bool> ClientFactory(string savedState)
        {
            try
            {
                client = new DropboxClient(savedState);

                // Test the token validation with a simple action, like get user space usage - totally harmless
                await client.Users.GetSpaceUsageAsync();

                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Get a working provider (so, an access token) from Dropbox.
        /// </summary>
        /// <param name="savedState"></param>
        /// <param name="showForm"></param>
        public async Task<string> Register(string savedState = null, bool showForm = true)
        {
            var result = new TaskCompletionSource<string>();

            // Check if the saved token is still usable
            if (await ClientFactory(savedState))
            {
                result.SetResult(savedState);
                return await result.Task;
            }

            // If the saved token was not usable and showForm is false, return with failure
            if (!showForm)
            {
                result.SetResult(null);
                return await result.Task;
            }

            // If it is not, try to get a new one
            var url = DropboxOAuth2Helper.GetAuthorizeUri(OAuthResponseType.Code, Obscure.CaesarDecode(Config.AppKey), (string)null);
            var form = new CodeForm(url.ToString(), 43);
            var success = false;

            form.OnResult += async code =>
            {
                success = true;
                form.Close();

                var response = await DropboxOAuth2Helper.ProcessCodeFlowAsync(code, Obscure.CaesarDecode(Config.AppKey), Obscure.CaesarDecode(Config.AppSecret));

                if (response?.AccessToken != null && await ClientFactory(response.AccessToken))
                {
                    result.SetResult(response.AccessToken);
                }
                else
                {
                    result.SetResult(null);
                }
            };

            form.FormClosed += (sender, args) =>
            {
                if (!success)
                {
                    result.SetResult(null);
                }
            };

            Process.Start(url.ToString());
            form.Show();

            return await result.Task;
        }

        /// <summary>
        /// Upload the given memory stream with the attached filename to Dropbox, share it and return its URL.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="stream"></param>
        /// <returns></returns>
        public async Task<string> Upload(string name, MemoryStream stream)
        {
            var path = "/" + name;

            // This is needed, because Dropbox API (and OneDrive and FTP aswell) will not work otherwise - I do not know why
            using (var newStream = new MemoryStream(stream.ToArray()))
            {
                await client.Files.UploadAsync(path, WriteMode.Overwrite.Instance, body: newStream);
            }

            var meta = await client.Sharing.CreateSharedLinkWithSettingsAsync(path);

            return meta.Url;
        }
    }
}
