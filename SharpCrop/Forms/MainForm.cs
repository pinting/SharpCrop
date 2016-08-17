using SharpCrop.Provider;
using SharpCrop.Utils;
using System;
using System.Windows.Forms;

namespace SharpCrop.Forms
{
    public partial class MainForm : Form
    {
        private Loader loader;

        /// <summary>
        /// MainForm gives user a UI to choose a Provider.
        /// </summary>
        /// <param name="loader"></param>
        public MainForm(Loader loader)
        {
            this.loader = loader;

            InitializeComponent();
        }

        /// <summary>
        /// Called when closed.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            Application.Exit();
        }

        /// <summary>
        /// When Dropbox button is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnDropbox(object sender, EventArgs e)
        {
            loader.LoadForm("Dropbox");
            Hide();
        }

        /// <summary>
        /// When Google Drive button is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnGoogleDrive(object sender, EventArgs e)
        {
            return;
        }

        /// <summary>
        /// When OneDrive button is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnOneDrive(object sender, EventArgs e)
        {
            return;
        }
    }
}
