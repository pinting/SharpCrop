using Dropbox.Api;
using SharpCrop.Interfaces;
using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SharpCrop.Services
{
    public partial class GrabberForm : Form, IGrabber
    {
        private readonly string redirectUrl = "http://localhost/";

        private Action<OAuth2Response> onToken;
        private string authState;

        public GrabberForm()
        {
            InitializeComponent();
            authState = Guid.NewGuid().ToString("N");

            var url = DropboxOAuth2Helper.GetAuthorizeUri(OAuthResponseType.Token, Settings.Default.ClientId, new Uri(redirectUrl), authState);

            webBrowser.Navigate(url);
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
            catch (ArgumentException)
            {
                onToken(null);
                Close();
            }
        }
    }
}
