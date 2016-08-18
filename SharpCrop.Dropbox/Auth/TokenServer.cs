using Dropbox.Api;
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
        private HttpServer server;
        private string authState;

        /// <summary>
        /// TokenServer can create a HTTP server on port 80 and listen for a Dropbox AccessToken.
        /// </summary>
        public TokenServer()
        {
            server = new HttpServer(Application.StartupPath + Constants.ServerPath, 80, OnRequest);
            authState = Guid.NewGuid().ToString("N");

            var url = DropboxOAuth2Helper.GetAuthorizeUri(OAuthResponseType.Token, Constants.ClientId, new Uri(Constants.RedirectUrl), authState);

			#if __MonoCS__
			System.Diagnostics.Process.Start("xdg-open", url.ToString());
			#else
			System.Diagnostics.Process.Start(url.ToString());
			#endif
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
						Task.Delay(2000);
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
            onToken(null, ProviderState.UserError);
            server.Stop();
        }
    }
}
