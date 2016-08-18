using SharpCrop.Dropbox.Auth;
using System;
using System.Windows.Forms;
using SharpCrop.Provider.Models;

namespace SharpCrop.Dropbox.Forms
{
    public partial class MainForm : Form
    {
        private Action<string, ProviderState> onToken;

        // Main form which get an AccessToken from the user (using internal or external tools).
        public MainForm(Action<string, ProviderState> onToken)
        {
            // Cannot be used twice
            this.onToken = new Action<string, ProviderState>((t1, e1) => 
            {
                this.onToken = new Action<string, ProviderState>((t2, e2) => { });
                onToken(t1, e1);
            });

            InitializeComponent();
        }

        /// <summary>
        /// Callback function which waits for the token from an IToken implemation.
        /// </summary>
        /// <param name="grabber"></param>
        private void WaitForToken(IToken grabber)
        {
            Hide();

            grabber.OnToken((string token, ProviderState state) =>
            {
                var action = new Action(() =>
                {
                    onToken(token, state);
                    Close();
                });
                
                if(InvokeRequired)
                {
                    Invoke(action);
                }
                else
                {
                    action();
                }
            });
        }

        /// <summary>
        /// Login using a local server, so the user can use the default browser.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ExternalLogin(object sender, EventArgs e)
        {
            try
            {
                WaitForToken(new TokenServer());
            }
            catch
            {
                onToken(null, ProviderState.PermissionError);
                Close();
            }
        }

        /// <summary>
        /// Internal login using a WebView. Works without admin privilege.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void InternalLogin(object sender, LinkLabelLinkClickedEventArgs e)
        {
            #if __MonoCS__

            MessageBox.Show("Internal login not supported in Mono!");

            #else

            var form = new TokenForm();

            form.Show();
            WaitForToken(form);

            #endif
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            onToken(null, ProviderState.UserError);
        }
    }
}
