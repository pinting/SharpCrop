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

        public Task<string> Register(string oldCreds, bool showForm = true)
        {
            var result = new TaskCompletionSource<string>();

            // Try to use the old credentials
            if (!string.IsNullOrEmpty(oldCreds))
            {
                creds = JsonConvert.DeserializeObject<LoginCreds>(Obscure.Base64Decode(oldCreds));

                if(!string.IsNullOrEmpty(creds.Username) && !string.IsNullOrEmpty(creds.Password) && !string.IsNullOrEmpty(creds.RemotePath) && !string.IsNullOrEmpty(creds.CopyPath))
                {
                    result.SetResult(oldCreds);

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

            form.OnResult(creds =>
            {
                this.creds = creds;
                success = true;

                result.SetResult(Obscure.Base64Encode(JsonConvert.SerializeObject(creds)));
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
        
        public Task<string> Upload(string name, MemoryStream stream)
        {
            // This is needed, it will not work otherwise - I do not know why
            using (var newStream = new MemoryStream(stream.ToArray()))
            {
                FTPUploader.Upload(newStream, $"{creds.RemotePath}/{name}", creds.Username, creds.Password);
            }

            return Task.FromResult($"{creds.CopyPath}{name}");
        }
    }
}
