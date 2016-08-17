using Dropbox.Api;
using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.ComponentModel;

namespace SharpCrop.Dropbox.Auth
{
    public partial class TokenForm : Form, IToken
    {
        private readonly string redirectUrl = "http://localhost/";
        private readonly string disposeUrl = "about:blank";

        private Action<OAuth2Response> onToken;
        private string authState;

        /// <summary>
        /// TokenForm is an internal implemantaion of the IToken interface. It works like the TokenServer,
        /// but requires less privilege. It gonna listen for the AccessToken through a WebBrowser element.
        /// </summary>
        public TokenForm()
        {
            InitializeComponent();
            authState = Guid.NewGuid().ToString("N");

            var url = DropboxOAuth2Helper.GetAuthorizeUri(OAuthResponseType.Token, Provider.ClientId, new Uri(redirectUrl), authState);

            webBrowser.Navigate(url);
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
        /// Called by the internal WebBrowser after the current page is loaded.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnResponse(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            if(e.Url.ToString() == disposeUrl)
            {
                webBrowser.Stop();
                Close();
            }

            if (!e.Url.ToString().StartsWith(redirectUrl, StringComparison.OrdinalIgnoreCase))
            {
                return;
            }

            try
            {
                OAuth2Response result = DropboxOAuth2Helper.ParseTokenFragment(e.Url);

                if (result.State == authState)
                {
                    Task.Run(() => onToken(result));
                }
                else
                {
                    Task.Run(() => onToken(null));
                }

                webBrowser.Navigate(disposeUrl);
                Hide();
            }
            catch (ArgumentException ev)
            {
                Console.WriteLine(ev.Message);
            }
        }

        /// <summary>
        /// Form is closed event.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            Task.Run(() => onToken(null));
        }
    }
}
