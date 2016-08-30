using System;
using System.IO;
using System.Threading.Tasks;
using SharpCrop.Provider;
using SharpCrop.Provider.Models;
using SharpCrop.Provider.Forms;

namespace SharpCrop.LocalFile
{
    public class Provider : IProvider
    {
        private string path;

        public Task Register(string oldPath, Action<string, ProviderState> onResult)
        {
            if(oldPath != null && Directory.Exists(oldPath))
            {
                path = oldPath;

                onResult(oldPath, ProviderState.RefreshToken);
                return Task.Delay(0);
            }

            var success = false;
            var form = new FolderForm();

            form.OnResult(newPath =>
            {
                success = true;
                path = newPath;

                onResult(newPath, ProviderState.NewToken);
                form.Close();
            });

            form.FormClosed += (sender, e) =>
            {
                if(success == false)
                {
                    onResult(null, ProviderState.UserError);
                }
            };

            form.Show();
            return Task.Delay(0);
        }

        public Task<string> Upload(string name, MemoryStream stream)
        {
            var url = Path.Combine(path, name);

            File.WriteAllBytes(url, stream.ToArray());

            return Task.FromResult(string.Format("file://{0}", url));
        }
    }
}
