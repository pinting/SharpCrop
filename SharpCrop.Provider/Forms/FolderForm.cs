using System;
using System.Windows.Forms;

namespace SharpCrop.Provider.Forms
{
    public partial class FolderForm : Form
    {
        private Action<string> onResult;

        public FolderForm()
        {
            InitializeComponent();
        }

        public void OnResult(Action<string> onResult)
        {
            this.onResult = onResult;
        }

        private void OnBrowse(object sender, EventArgs e)
        {
            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                folderBox.Text = folderBrowserDialog.SelectedPath;
            }
        }

        private void OnSubmit(object sender, EventArgs e)
        {
            onResult(folderBox.Text);
        }
    }
}
