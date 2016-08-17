using Dropbox.Api;
using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.ComponentModel;
using SharpCrop.Provider.Models;

namespace SharpCrop.Dropbox.Auth
{
    public partial class TokenForm : Form, IToken
    {
        private Action<string, ProviderState> onToken;
        private string authState;

        /// <summary>
        /// TokenForm is an internal implemantaion of the IToken interface. It works like the TokenServer,
        /// but requires less privilege. It gonna listen for the AccessToken through a WebBrowser element.
        /// </summary>
        public TokenForm()
        {
            InitializeComponent();
            authState = Guid.NewGuid().ToString("N");

            var url = DropboxOAuth2Helper.GetAuthorizeUri(OAuthResponseType.Token, Provider.ClientId, new Uri(Constants.RedirectUrl), authState);

            webBrowser.Navigate(url);
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
        /// Called by the internal WebBrowser after the current page is loaded.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnResponse(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            // Close the form if disposeUrl was loaded
            if(e.Url.ToString() == Constants.DisposeUrl)
            {
                webBrowser.Stop();
                Close();
            }

            if (!e.Url.ToString().StartsWith(Constants.RedirectUrl, StringComparison.OrdinalIgnoreCase))
            {
                return;
            }

            try
            {
                OAuth2Response result = DropboxOAuth2Helper.ParseTokenFragment(e.Url);

                if (result != null && result.AccessToken != null && result.State == authState)
                {
                    Task.Run(() => onToken(result.AccessToken, ProviderState.Normal));
                }
                else
                {
                    Task.Run(() => onToken(null, ProviderState.ServiceError));
                }

                // Need to navigate to a blank page to close it, because sometimes IE
                // opens after the Form is closed.
                webBrowser.Navigate(Constants.RedirectUrl);
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
            Task.Run(() => onToken(null, ProviderState.UserError));
        }
    }
}
