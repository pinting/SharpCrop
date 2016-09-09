using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2;
using System.Threading;
using Google.Apis.Auth.OAuth2.Responses;
using Google.Apis.Auth.OAuth2.Requests;
using SharpCrop.Provider.Forms;

namespace SharpCrop.GoogleDrive.Utils
{
    /// <summary>
    /// Custom ICodeReceiver implementation which uses the CodeForm to wait for the API code.
    /// </summary>
    public class CodeReceiver : ICodeReceiver
    {
        public string RedirectUri => GoogleAuthConsts.InstalledAppRedirectUri;

        /// <summary>
        /// Waiting for a Google API code.
        /// </summary>
        /// <param name="authUrl"></param>
        /// <param name="taskCancellationToken"></param>
        /// <returns></returns>
        public Task<AuthorizationCodeResponseUrl> ReceiveCodeAsync(AuthorizationCodeRequestUrl authUrl, CancellationToken taskCancellationToken)
        {
            var result = new TaskCompletionSource<AuthorizationCodeResponseUrl>();
            var url = authUrl.Build().ToString();

            var form = new CodeForm(url, 45);
            var success = false;

            form.OnCode(code =>
            {
                success = true;

                form.Close();
                result.SetResult(new AuthorizationCodeResponseUrl() { Code = code });
            });

            form.FormClosed += (sender, e) =>
            {
                if (!success)
                {
                    result.SetResult(null);
                }
            };

            System.Diagnostics.Process.Start(url);
            form.Show();

            return result.Task;
        }
    }
}
