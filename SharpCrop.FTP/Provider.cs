using System.IO;
using System.Threading.Tasks;
using SharpCrop.Provider;
using System;
using SharpCrop.FTP.Models;
using SharpCrop.FTP.Utils;
using SharpCrop.FTP.Forms;
using Newtonsoft.Json;
using SharpCrop.Provider.Utils;

namespace SharpCrop.FTP
{
    public class Provider : IProvider
    {
        private LoginCredentials creds;
        
        /// <summary>
        /// Register an FTP connection from saved or new credentials.
        /// </summary>
        /// <param name="savedState">The login informations of the FTP.</param>
        /// <param name="showForm">Show the registration form, if the saved state did not work.</param>
        /// <returns></returns>
        public Task<string> Register(string savedState, bool showForm = true)
        {
            var result = new TaskCompletionSource<string>();

            // Try to use the old credentials
            try
            {
                var oldCreds = JsonConvert.DeserializeObject<LoginCredentials>(Obscure.Base64Decode(savedState));

                result.SetResult(savedState);
                creds = oldCreds;

                return result.Task;
            }
            catch
            {
                // Ignored
            }

            // If the saved creds were not usable and showForm is false, return with failure
            if (!showForm)
            {
                result.SetResult(null);
                return result.Task;
            }

            // If the old ones failed, try to get new ones
            var form = new LoginForm();
            var success = false;

            form.OnResult(newCreds =>
            {
                creds = newCreds;
                success = true;

                result.SetResult(Obscure.Base64Encode(JsonConvert.SerializeObject(newCreds)));
                form.Close();
            });

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
                var remote = new Uri(creds.RemotePath, name);
                var copy = new Uri(creds.CopyPath, name);

                // This is needed, it will not work otherwise - I do not know why
                using (var newStream = new MemoryStream(stream.ToArray()))
                {
                    FtpUploader.Upload(newStream, remote, creds.Username, creds.Password);
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
