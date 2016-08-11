using Dropbox.Api;
using SharpCrop.Auth;
using System;
using System.Timers;
using System.Windows.Forms;

namespace SharpCrop.Forms
{
    public partial class MainForm : Form
    {
        /// <summary>
        /// Main form which get an AccessToken from the user (using internal or external tools).
        /// </summary>
        public MainForm()
        {
            InitializeComponent();

            var accessToken = Settings.Default.AccessToken;

            // If an AccessToken is available there is no need to show this form
            if (accessToken.Length > 0)
            {
                WindowState = FormWindowState.Minimized;
                ShowInTaskbar = false;

                Start();
            }
        }

        /// <summary>
        /// Executed on failure and gives a chance to the user to try again.
        /// </summary>
        public void Failed()
        {
            MessageBox.Show("Failed to connect!");
            
            WindowState = FormWindowState.Normal;
            ShowInTaskbar = true;

            var action = new Action(() =>
            {
                Show();
                Focus();
            });

            if (InvokeRequired)
            {
                Invoke(action);
            }
            else
            {
                action();
            }
        }

        /// <summary>
        /// Start the ClickForm - and the program itself.
        /// </summary>
        private void Start()
        {
            var action = new Action(() =>
            {
                try
                {
                    var form = new ClickForm(this);
                    form.Show();
                }
                catch
                {
                    Failed();
                }
            });

            if (InvokeRequired)
            {
                Invoke(action);
            }
            else
            {
                action();
            }
        }

        /// <summary>
        /// This function is executed when the AccessToken was obtained successfully.
        /// </summary>
        /// <param name="result"></param>
        private void Success(OAuth2Response result)
        {
            if (result == null)
            {
                Failed();
                return;
            }
            
            Settings.Default.AccessToken = result.AccessToken;
            Settings.Default.UserId = result.Uid;
            Settings.Default.Save();

            MessageBox.Show("Successfully connected!");
            Start();
        }

        /// <summary>
        /// Callback function which waits for the token from an IToken implemation.
        /// </summary>
        /// <param name="grabber"></param>
        private void Login(IToken grabber)
        {
            var timer = new System.Timers.Timer(1000 * 60 * 3);

            Hide();

            grabber.OnToken((OAuth2Response token) => 
            {
                timer.Stop();
                Success(token);
            });

            timer.Elapsed += delegate (Object source, ElapsedEventArgs ev)
            {
                grabber.Close();
                Failed();
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
                Login(new TokenServer());
            }
            catch
            {
                Failed();
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
            Login(form);
        }
    }
}
