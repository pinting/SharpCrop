using Dropbox.Api;
using SharpCrop.Services;
using SharpCrop.Interfaces;
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
        }

        private void TokenParser(OAuth2Response result)
        {
            if (result == null)
            {
                Failed();
                return;
            }

            MessageBox.Show(result.AccessToken);
            Focus();
        }

        private void Failed()
        {
            MessageBox.Show("Failed to connect!");
            Focus();
            Show();
        }

        private void Login(IGrabber grabber)
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
                Login(new GrabberServer());
            }
            catch
            {
                Failed();
            }
        }
        private void InternalLogin(object sender, LinkLabelLinkClickedEventArgs e)
        {
            var form = new GrabberForm();

            form.Show();
            Login(form);
        }
    }
}
