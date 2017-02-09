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
        private LoginCreds creds;

        /// <summary>
        /// Check if creds are valid.
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        private bool IsValid(LoginCreds c)
        {
            if (string.IsNullOrEmpty(c.Username) || string.IsNullOrEmpty(c.Password) || string.IsNullOrEmpty(c.RemotePath) || string.IsNullOrEmpty(c.CopyPath))
            {
                return false;
            }

            Uri url;

            if (!Uri.TryCreate(c.RemotePath, UriKind.Absolute, out url) || !Uri.TryCreate(c.CopyPath, UriKind.Absolute, out url))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Register an FTP connection - or try to restore one from the past.
        /// </summary>
        /// <param name="savedState"></param>
        /// <param name="showForm"></param>
        /// <returns></returns>
        public Task<string> Register(string savedState, bool showForm = true)
        {
            var result = new TaskCompletionSource<string>();

            // Try to use the old credentials
            if (!string.IsNullOrEmpty(savedState))
            {
                var oldCreds = JsonConvert.DeserializeObject<LoginCreds>(Obscure.Base64Decode(savedState));

                if(IsValid(oldCreds))
                {
                    result.SetResult(savedState);
                    creds = oldCreds;

                    return result.Task;
                }
            }

            // If the saved creds were not usable and showForm is false, return failure
            if (!showForm)
            {
                result.SetResult(null);
                return result.Task;
            }

            // If it fails, try to get new ones
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
        /// Upload the given memory stream with the attached filename.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="stream"></param>
        /// <returns></returns>
        public Task<string> Upload(string name, MemoryStream stream)
        {
            try
            {
                // This is needed, it will not work otherwise - I do not know why
                using (var newStream = new MemoryStream(stream.ToArray()))
                {
                    FtpUploader.Upload(newStream, $"{creds.RemotePath}/{name}", creds.Username, creds.Password);
                }

                return Task.FromResult($"{creds.CopyPath}{name}");
            }
            catch
            {
                return Task.FromResult((string)null);
            }
        }
    }
}
