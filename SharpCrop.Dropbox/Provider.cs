using SharpCrop.Provider;
using Dropbox.Api;
using Dropbox.Api.Files;
using System.IO;
using SharpCrop.Provider.Utils;
using System.Windows.Forms;
using System.Threading.Tasks;
using SharpCrop.Provider.Forms;

namespace SharpCrop.Dropbox
{
    /// <summary>
    /// IProvider implementation for Dropbox.
    /// </summary>
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
        /// Get a token from Dropbox.
        /// </summary>
        /// <param name="token"></param>
        public async Task<string> Register(string token)
        {
            var result = new TaskCompletionSource<string>();

            // Check if the saved token is still usable
            if (await ClientFactory(token))
            {
                result.SetResult(token);
                return await result.Task;
            }

            // If it is not, try to get another one
            var url = DropboxOAuth2Helper.GetAuthorizeUri(
                    OAuthResponseType.Code,
                    Obscure.Decode(Constants.AppKey),
                    (string)null);

            var form = new CodeForm(url.ToString(), 43);
            var success = false;

            form.OnCode(async code =>
            {
                success = true;
                form.Close();

                var response = await DropboxOAuth2Helper.ProcessCodeFlowAsync(
                    code,
                    Obscure.Decode(Constants.AppKey),
                    Obscure.Decode(Constants.AppSecret));

                if (response != null && response.AccessToken != null && await ClientFactory(response.AccessToken))
                {
                    result.SetResult(response.AccessToken);
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

            System.Diagnostics.Process.Start(url.ToString());
            form.Show();

            return await result.Task;
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
