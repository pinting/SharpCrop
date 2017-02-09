using System;
using System.Windows.Forms;

namespace SharpCrop.Provider.Forms
{
    /// <summary>
    /// CodeForm shows the token request link and it waits for the user 
    /// to copy the API code into its input box.
    /// </summary>
    public partial class CodeForm : Form
    {
        private Action<string> onResult;
        private readonly int length;

        /// <summary>
        /// Construct a new CodePaste form with an URL.
        /// </summary>
        /// <param name="url"></param>
        /// <param name="length"></param>
        public CodeForm(string url = "", int length = 128)
        {
            InitializeComponent();

            this.length = length;
            linkBox.Text = url;
        }

        /// <summary>
        /// Register for callback.
        /// </summary>
        /// <param name="callback"></param>
        public void OnResult(Action<string> callback)
        {
            onResult = callback;
        }

        /// <summary>
        /// Call callback when the code is copied.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CodeBoxChanged(object sender, EventArgs e)
        {
            if(codeBox.Text.Length >= length)
            {
                onResult(codeBox.Text);
            }
        }

        /// <summary>
        /// Open browser when the link was clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(linkBox.Text);
        }
    }
}
