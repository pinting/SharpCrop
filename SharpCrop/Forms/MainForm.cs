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
            
            if(string.IsNullOrEmpty(Settings.Default.Provider))
            {
                return;
            }

            WindowState = FormWindowState.Minimized;
            ShowInTaskbar = false;

            switch (Settings.Default.Provider)
            {
                case "Dropbox":
                    OnDropbox(null, null);
                    break;
            }
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

        private void StartProvider(string name, IProvider provider)
        {
            Hide();

            provider.Register(Settings.Default.Token, token => 
            {
                if(token == null)
                {
                    WindowState = FormWindowState.Normal;
                    ShowInTaskbar = true;

                    Show();
                    MessageBox.Show("Failed to register provider!");

                    return;
                }

                Settings.Default.Provider = name;
                Settings.Default.Token = token;
                Settings.Default.Save();

                Start(provider);
            });
        }

        private void OnDropbox(object sender, EventArgs e)
        {
            StartProvider("Dropbox", new Dropbox.Provider());
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
