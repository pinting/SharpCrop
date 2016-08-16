using SharpCrop.Provider;
using SharpCrop.Utils;
using System;
using System.Windows.Forms;

namespace SharpCrop.Forms
{
    public partial class MainForm : Form
    {
        private Loader loader;

        public MainForm(Loader loader)
        {
            this.loader = loader;

            InitializeComponent();
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            Application.Exit();
        }

        private void OnDropbox(object sender, EventArgs e)
        {
            loader.Load("Dropbox");
            Hide();
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
