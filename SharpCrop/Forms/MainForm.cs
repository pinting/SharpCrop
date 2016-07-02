using Dropbox.Api;
using SharpCrop.Services;
using SharpCrop.Auth;
using System;
using System.Timers;
using System.Windows.Forms;

namespace SharpCrop.Forms
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();

            var accessToken = Settings.Default.AccessToken;

            if (accessToken.Length > 0)
            {
                Console.WriteLine(accessToken);
                Start();
            }
        }

        private void Start()
        {
            var action = new Action(() =>
            {
                var form = new ClickForm();
                form.Show();
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

        private void TokenParser(OAuth2Response result)
        {
            if (result == null)
            {
                Failed();
                return;
            }
            
            Settings.Default.AccessToken = result.AccessToken;
            Settings.Default.UserId = result.Uid;
            Settings.Default.Save();

            Start();
        }

        private void Failed()
        {
            MessageBox.Show("Failed to connect!");
            Show();
            Focus();
        }

        private void Login(IToken grabber)
        {
            var timer = new System.Timers.Timer(1000 * 60 * 3);

            Hide();
            grabber.OnToken(TokenParser);

            timer.Elapsed += delegate (Object source, ElapsedEventArgs ev)
            {
                grabber.Close();
                Failed();
            };

            timer.AutoReset = false;
            timer.Enabled = true;
        }

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

        private void InternalLogin(object sender, LinkLabelLinkClickedEventArgs e)
        {
            var form = new TokenForm();

            form.Show();
            Login(form);
        }
    }
}
