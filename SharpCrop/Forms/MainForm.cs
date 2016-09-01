using System;
using System.Windows.Forms;

namespace SharpCrop.Forms
{
    /// <summary>
    /// MainForm gives user a UI to choose a Provider.
    /// </summary>
    public partial class MainForm : Form
    {
        private Controller controller;

        /// <summary>
        /// Consturct a MainForm - the Loader is needed, because CropForm
        /// is constructed there.
        /// </summary>
        /// <param name="controller"></param>
        public MainForm(Controller controller)
        {
            this.controller = controller;

            InitializeComponent();
        }

        /// <summary>
        /// Focus on MainForm when shown.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            Focus();
        }

        /// <summary>
        /// When Dropbox button is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnDropbox(object sender, EventArgs e)
        {
            controller.LoadProvider("Dropbox");
            Hide();
        }

        /// <summary>
        /// When Google Drive button is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnGoogleDrive(object sender, EventArgs e)
        {
            controller.LoadProvider("GoogleDrive");
            Hide();
        }

        /// <summary>
        /// When OneDrive button is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnOneDrive(object sender, EventArgs e)
        {
            controller.LoadProvider("OneDrive");
            Hide();
        }

        /// <summary>
        /// When LocalFile link is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnLocalFile(object sender, LinkLabelLinkClickedEventArgs e)
        {
            controller.LoadProvider("LocalFile");
            Hide();
        }
    }
}
