using System;
using System.Windows.Forms;

namespace SharpCrop.Provider.Forms
{
    /// <summary>
    /// FolderForm is used to help the user select a directory.
    /// </summary>
    public partial class FolderForm : Form
    {
        private Action<string> onResult;

        /// <summary>
        /// Init form.
        /// </summary>
        public FolderForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Register a function as the callback.
        /// </summary>
        /// <param name="callback"></param>
        public void OnResult(Action<string> callback)
        {
            onResult = callback;
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
            onResult(folderBox.Text);
        }
    }
}
