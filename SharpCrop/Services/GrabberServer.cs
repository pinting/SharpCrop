using Dropbox.Api;
using SharpCrop.Interfaces;
using SharpCrop.Util;
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SharpCrop.Services
{
    class GrabberServer : IGrabber
    {
        private readonly string redirectUrl = "http://localhost/";

        private Action<OAuth2Response> onToken;
        private HttpServer server;
        private string authState;

        public GrabberServer()
        {
            this.server = new HttpServer(Application.StartupPath + "/www", 80, OnRequest);
            this.authState = Guid.NewGuid().ToString("N");

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

                Thread.Sleep(1000);
                Close();
            }
            catch (ArgumentException)
            {
                return;
            }
        }

        public void Close()
        {
            server.Stop();
        }
    }
}
