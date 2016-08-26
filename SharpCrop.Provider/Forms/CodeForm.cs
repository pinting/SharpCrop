using System;
using System.Windows.Forms;

namespace SharpCrop.Provider.Forms
{
    public partial class CodeForm : Form
    {
        private Action<string> onCode;
        
        /// <summary>
        /// Code paste form.
        /// </summary>
        /// <param name="url"></param>
        public CodeForm(string url)
        {
            InitializeComponent();

            linkBox.Text = url;
        }

        /// <summary>
        /// Register for callback.
        /// </summary>
        /// <param name="onCode"></param>
        public void OnCode(Action<string> onCode)
        {
            this.onCode = onCode;
        }

        /// <summary>
        /// On code box changed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CodeBoxChanged(object sender, EventArgs e)
        {
            if(codeBox.Text.Length > 1)
            {
                onCode(codeBox.Text);
            }
        }

        /// <summary>
        /// On link clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(linkBox.Text);
        }
    }
}
