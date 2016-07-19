using Dropbox.Api;
using SharpCrop.Auth;
using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SharpCrop.Auth
{
    public partial class TokenForm : Form, IToken
    {
        private readonly string redirectUrl = "http://localhost/";

        private Action<OAuth2Response> onToken;
        private string authState;
        private bool success;

        public TokenForm()
        {
            InitializeComponent();
            authState = Guid.NewGuid().ToString("N");

            var url = DropboxOAuth2Helper.GetAuthorizeUri(OAuthResponseType.Token, Settings.Default.ClientId, new Uri(redirectUrl), authState);

            webBrowser.Navigate(url);
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            if(!success)
            {
                Task.Run(() => onToken(null));
            }
        }

        public void OnToken(Action<OAuth2Response> onToken)
        {
            this.onToken = onToken;
        }

        private void OnResponse(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            if (!e.Url.ToString().StartsWith(redirectUrl, StringComparison.OrdinalIgnoreCase))
            {
                return;
            }

            try
            {
                OAuth2Response result = DropboxOAuth2Helper.ParseTokenFragment(e.Url);

                if (result.State == authState)
                {
                    success = true;
                    Task.Run(() => onToken(result));
                }
            }
            catch (ArgumentException ev)
            {
                Console.WriteLine(ev.Message);
            }

            Close();
        }
    }
}
