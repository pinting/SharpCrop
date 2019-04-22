using System;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SharpCrop.FTP.Forms;
using SharpCrop.FTP.Models;
using SharpCrop.FTP.Properties;
using SharpCrop.FTP.Utils;
using SharpCrop.Provider;
using SharpCrop.Provider.Utils;

namespace SharpCrop.FTP
{
    public class Provider : IProvider
    {
        private LoginCredentials credentials;

        public string Id => Config.ProviderId;

        public string Name => Resources.ProviderName;

        /// <summary>
        /// Register an FTP connection from saved or new credentials.
        /// </summary>
        /// <param name="savedState">The login credentials of the server.</param>
        /// <param name="silent"></param>
        /// <returns></returns>
        public Task<string> Register(string savedState = null, bool silent = false)
        {
            var result = new TaskCompletionSource<string>();

            // Try to use the old credentials
            try
            {
                var oldCredentials = JsonConvert.DeserializeObject<LoginCredentials>(Obscure.Base64Decode(savedState));

                result.SetResult(savedState);
                credentials = oldCredentials;

                return result.Task;
            }
            catch
            {
                // Ignored
            }

            // If the saved credentials were not usable and silent is true, return with failure
            if (silent)
            {
                result.SetResult(null);
                return result.Task;
            }

            // If the old ones failed, try to get new ones
            var form = new LoginForm();
            var success = false;

            form.OnResult += newCredentials =>
            {
                credentials = newCredentials;
                success = true;

                result.SetResult(Obscure.Base64Encode(JsonConvert.SerializeObject(newCredentials)));
                form.Close();
            };

            form.FormClosed += (sender, e) =>
            {
                if (!success)
                {
                    result.SetResult(null);
                }
            };

            form.Show();

            return result.Task;
        }

        /// <summary>
        /// Upload the given memory stream with the attached filename to the FTP server and return
        /// a link with the registered CopyPath prefix.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="stream"></param>
        /// <returns></returns>
        public Task<string> Upload(string name, MemoryStream stream)
        {
            try
            {
                var remote = new Uri(credentials.RemotePath, name);
                var copy = new Uri(credentials.CopyPath, name);

                // This is needed, it will not work otherwise - I do not know why
                // TODO: Why?
                using (var newStream = new MemoryStream(stream.ToArray()))
                {
                    FtpUploader.Upload(newStream, remote, credentials.Username, credentials.Password);
                }

                return Task.FromResult(copy.ToString());
            }
            catch
            {
                return Task.FromResult((string)null);
            }
        }
    }
}
