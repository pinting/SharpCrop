using Dropbox.Api;
using SharpCrop.Auth;
using SharpCrop.Utils;
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SharpCrop.Auth
{
    class TokenServer : IToken
    {
        private readonly string serverPath = Application.StartupPath + "/www";
        private readonly string redirectUrl = "http://localhost/";

        private Action<OAuth2Response> onToken;
        private HttpServer server;
        private string authState;

        public TokenServer()
        {
            server = new HttpServer(serverPath, 80, OnRequest);
            authState = Guid.NewGuid().ToString("N");

            var url = DropboxOAuth2Helper.GetAuthorizeUri(OAuthResponseType.Token, Settings.Default.ClientId, new Uri(redirectUrl), authState);

            System.Diagnostics.Process.Start(url.ToString());
        }

        public void OnToken(Action<OAuth2Response> onToken)
        {
            this.onToken = onToken;
        }

        public void OnRequest(HttpListenerRequest request)
        {
            var url = new Uri(request.Url.ToString().Replace('?', '#'));

            try
            {
                OAuth2Response result = DropboxOAuth2Helper.ParseTokenFragment(url);

                if (result.State != authState)
                {
                    Task.Run(() => onToken(null));
                }
                else
                {
                    Task.Run(() => onToken(result));
                }
                
                Close();
            }
            catch (ArgumentException e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public void Close()
        {
            server.Stop();
        }
    }
}
