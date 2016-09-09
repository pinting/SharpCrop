using System.IO;
using System.Threading.Tasks;
using SharpCrop.Provider;
using SharpCrop.Provider.Forms;

namespace SharpCrop.LocalFile
{
    /// <summary>
    /// This is another IProvider implemantation which writes the output to
    /// the local disk with File.IO.
    /// </summary>
    public class Provider : IProvider
    {
        private string path;

        /// <summary>
        /// Register a path which will be saved as a "token" for this provider.
        /// </summary>
        /// <param name="oldPath"></param>
        /// <returns></returns>
        public Task<string> Register(string oldPath)
        {
            var result = new TaskCompletionSource<string>();

            // Check if given oldPath is still exists - if it is, use it
            if (oldPath != null && Directory.Exists(oldPath))
            {
                path = oldPath;

                result.SetResult(oldPath);
                return result.Task;
            }

            // Get a newPath with FolderForm
            var success = false;
            var form = new FolderForm();

            form.OnResult(newPath =>
            {
                success = true;
                path = newPath;

                result.SetResult(newPath);
                form.Close();
            });

            form.FormClosed += (sender, e) =>
            {
                if(success == false)
                {
                    result.SetResult(null);
                }
            };

            form.Show();

            return result.Task;
        }

        /// <summary>
        /// Write a MemoryStream with the given name to disk.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="stream"></param>
        /// <returns></returns>
        public Task<string> Upload(string name, MemoryStream stream)
        {
            var url = Path.Combine(path, name);

            File.WriteAllBytes(url, stream.ToArray());

            return Task.FromResult($"{Constants.UrlPrefix}{url}");
        }
    }
}
