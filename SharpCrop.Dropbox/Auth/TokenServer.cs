using Dropbox.Api;
using SharpCrop.Dropbox.Forms;
using SharpCrop.Provider.Models;
using SharpCrop.Provider.Utils;
using System;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SharpCrop.Dropbox.Auth
{
    public class TokenServer : IToken
    {
        private Action<string, ProviderState> onToken;
        private WaitForm waitForm;
        private HttpServer server;
        private string authState;
        private bool closed = false;

        /// <summary>
        /// TokenServer can create a HTTP server on port 80 and listen for a Dropbox AccessToken.
        /// </summary>
        public TokenServer()
        {
            server = new HttpServer(Application.StartupPath + Constants.ServerPath, 80, OnRequest);
            authState = Guid.NewGuid().ToString("N");

            var url = DropboxOAuth2Helper.GetAuthorizeUri(OAuthResponseType.Token, Constants.ClientId, new Uri(Constants.RedirectUrl), authState);

            System.Diagnostics.Process.Start(url.ToString());

            waitForm = new WaitForm(url.ToString());
            waitForm.FormClosed += (object sender, FormClosedEventArgs e) =>
            {
                if(!closed)
                {
                    Close();
                }
            };
            waitForm.Show();
        }

        /// <summary>
        /// Callback function setter. The callback will be executed when an AccessToken is found.
        /// </summary>
        /// <param name="onToken"></param>
        public void OnToken(Action<string, ProviderState> onToken)
        {
            this.onToken = new Action<string, ProviderState>((t1, e1) =>
            {
                this.onToken = new Action<string, ProviderState>((t2, e2) => { });
                onToken(t1, e1);
            });
        }

        /// <summary>
        /// Callback for HttpServer. This function will be called on every request.
        /// </summary>
        /// <param name="request"></param>
        private void OnRequest(HttpListenerRequest request)
        {
            var url = new Uri(request.Url.ToString().Replace('?', '#'));

            try
            {
                OAuth2Response result = DropboxOAuth2Helper.ParseTokenFragment(url);

                if (result != null && result.AccessToken != null && result.State == authState)
                {
					Task.Run(() => 
					{
						onToken(result.AccessToken, ProviderState.Normal);
						Task.Delay(2000).Wait();
						Close();
					});
                }
                else
                {
                    Task.Run(() => onToken(null, ProviderState.ServiceError));
                }
            }
            catch (ArgumentException e)
            {
                Console.WriteLine(e.Message);
            }
        }

        /// <summary>
        /// Stop the server.
        /// </summary>
        public void Close()
        {
            closed = true;

            if(waitForm.InvokeRequired)
            {
                waitForm.Invoke(new Action(() => waitForm.Close()));
            }
            else
            {
                waitForm.Close();
            }

            server.Stop();
            onToken(null, ProviderState.UserError);
        }
    }
}
