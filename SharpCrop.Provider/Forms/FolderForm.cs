using System;
using System.Windows.Forms;
using SharpCrop.Provider.Properties;

namespace SharpCrop.Provider.Forms
{
    /// <summary>
    /// FolderForm is used to help the user select a directory.
    /// </summary>
    public partial class FolderForm : Form
    {
        /// <summary>
        /// Executed when the required code is pasted.
        /// </summary>
        public event Action<string> OnResult;

        /// <summary>
        /// Init form.
        /// </summary>
        public FolderForm()
        {
            InitializeComponent();

            // Init texts
            submitButton.Text = Resources.FolderSubmit;
            browseButton.Text = Resources.FolderBrowse;
        }

        /// <summary>
        /// Start OS specific folder browser when the browse button was clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnBrowse(object sender, EventArgs e)
        {
            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                folderBox.Text = folderBrowserDialog.SelectedPath;
            }
        }

        /// <summary>
        /// Call the callback with the result.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnSubmit(object sender, EventArgs e)
        {
            OnResult?.Invoke(folderBox.Text);
        }
    }
}
