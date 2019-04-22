using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth.OAuth2.Requests;
using Google.Apis.Auth.OAuth2.Responses;
using SharpCrop.Provider.Forms;

namespace SharpCrop.GoogleDrive.Utils
{
    /// <summary>
    /// Custom ICodeReceiver implementation which uses the CodeForm to wait for the API code.
    /// </summary>
    public class CodeReceiver : ICodeReceiver
    {
        private readonly bool showForm;
        private bool executed;
        
        public CodeReceiver(bool silent = false)
        {
            showForm = !silent;
        }
        
        public Task<AuthorizationCodeResponseUrl> ReceiveCodeAsync(AuthorizationCodeRequestUrl authUrl, CancellationToken taskCancellationToken)
        {
            var result = new TaskCompletionSource<AuthorizationCodeResponseUrl>();
            
            if (showForm == false)
            {
                result.SetResult(null);
                return result.Task;
            }

            var url = authUrl.Build().ToString();
            var form = new CodeForm(url, 45);
            var success = false;

            form.OnResult += code =>
            {
                success = true;

                form.Close();
                result.SetResult(new AuthorizationCodeResponseUrl { Code = code });
            };

            form.FormClosed += (sender, e) =>
            {
                if (!success)
                {
                    result.SetResult(null);
                }
            };

            Process.Start(url);
            form.Show();

            executed = true;

            return result.Task;
        }

        /// <summary>
        /// Return true if the CodeReceiver was used to get a new access token.
        /// </summary>
        public bool Executed => executed;

        public string RedirectUri => GoogleAuthConsts.InstalledAppRedirectUri;

    }
}
