using Dropbox.Api;
using SharpCrop.Dropbox.Auth;
using System;
using System.Timers;
using System.Windows.Forms;
using System.ComponentModel;
using System.Threading.Tasks;

namespace SharpCrop.Dropbox.Forms
{
    public partial class MainForm : Form
    {
        private Action<string> onToken;

        // Main form which get an AccessToken from the user (using internal or external tools).
        public MainForm(Action<string> onToken)
        {
            // Cannot be used twice
            this.onToken = new Action<string>(t1 => 
            {
                this.onToken = new Action<string>(t2 => { });
                onToken(t1);
            });

            InitializeComponent();
        }

        /// <summary>
        /// Callback function which waits for the token from an IToken implemation.
        /// </summary>
        /// <param name="grabber"></param>
        private void WaitForToken(IToken grabber)
        {
            var timer = new System.Timers.Timer(1000 * 60 * 3);

            Hide();

            grabber.OnToken((OAuth2Response result) =>
            {
                timer.Stop();

                var action = new Action(() =>
                {
                    if (result == null)
                    {
                        onToken(null);
                    }
                    else
                    {
                        onToken(result.AccessToken);
                    }

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

            timer.Elapsed += delegate (Object source, ElapsedEventArgs ev)
            {
                Invoke(new Action(() =>
                {
                    grabber.Close();
                    Close();
                }));
            };

            timer.AutoReset = false;
            timer.Enabled = true;
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
            var form = new TokenForm();

            form.Show();
            WaitForToken(form);
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            onToken(null);
        }
    }
}
