using System;
using System.Net;
using System.Windows.Forms;

namespace SharpCrop.Util
{
    class TokenGrabber
    {
        private readonly string clientId = "cou3krww0do592i";
        private readonly string redirect = "http://localhost";

        private Action<string> onToken;
        private SimpleHTTPServer server;
        private string token;

        public TokenGrabber(Action<string> onToken)
        {
            this.server = new SimpleHTTPServer(Application.StartupPath + "/www", 80, OnRequest);
            this.onToken = onToken;

            System.Diagnostics.Process.Start("https://www.dropbox.com/oauth2/authorize?client_id=" + clientId + "&response_type=token&redirect_uri=" + redirect);
        }

        public void OnRequest(HttpListenerRequest request)
        {
            var query = request.Url.Query;

            if (query.Length < 2)
            {
                return;
            }

            var args = query.Substring(1).Split('&');

            if (query == null)
            {
                return;
            }

            foreach (var e in args)
            {
                if (e.IndexOf("access_token=") >= 0)
                {
                    token = e.Substring("access_token=".Length);
                    onToken(token);
                    server.Stop();
                }
            }
        }
    }
}
