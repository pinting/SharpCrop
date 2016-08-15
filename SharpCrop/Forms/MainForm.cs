using SharpCrop.Provider;
using System;
using System.Windows.Forms;

namespace SharpCrop.Forms
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();

            // Debug
            Settings.Default.Token = "";
            Settings.Default.Save();
        }

        private void Start(IProvider provider)
        {
            var action = new Action(() =>
            {
                var form = new ClickForm(provider);
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

        private void StartProvider(IProvider provider)
        {
            Hide();

            provider.Register(Settings.Default.Token, token => 
            {
                if(token == null)
                {
                    MessageBox.Show("Failed to register provider!");
                    Show();
                    return;
                }

                Settings.Default.Token = token;
                Settings.Default.Save();

                Start(provider);
            });
        }

        private void OnDropbox(object sender, EventArgs e)
        {
            StartProvider(new Dropbox.Provider());
        }

        private void OnGoogleDrive(object sender, EventArgs e)
        {
            return;
        }

        private void OnOneDrive(object sender, EventArgs e)
        {
            return;
        }
    }
}
