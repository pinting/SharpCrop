using Dropbox.Api;
using SharpCrop.Provider.Utils;
using System;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SharpCrop.Dropbox.Auth
{
    class TokenServer : IToken
    {
        private readonly string serverPath = Application.StartupPath + "/www";
        private readonly string redirectUrl = "http://localhost/";

        private Action<OAuth2Response> onToken;
        private HttpServer server;
        private string authState;

        /// <summary>
        /// TokenServer can create a HTTP server on port 80 and listen for a Dropbox AccessToken.
        /// </summary>
        public TokenServer()
        {
            server = new HttpServer(serverPath, 80, OnRequest);
            authState = Guid.NewGuid().ToString("N");

            var url = DropboxOAuth2Helper.GetAuthorizeUri(OAuthResponseType.Token, Provider.ClientId, new Uri(redirectUrl), authState);

            System.Diagnostics.Process.Start(url.ToString());
        }

        /// <summary>
        /// Callback function setter. The callback will be executed when an AccessToken is found.
        /// </summary>
        /// <param name="onToken"></param>
        public void OnToken(Action<OAuth2Response> onToken)
        {
            this.onToken = new Action<OAuth2Response>(t1 =>
            {
                this.onToken = new Action<OAuth2Response>(t2 => { });
                onToken(t1);
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

                if (result.State == authState)
                {
                    Task.Run(() => onToken(result));
                }
                else
                {
                    Task.Run(() => onToken(null));
                }
                
                Close();
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
            onToken(null);
            server.Stop();
        }
    }
}
