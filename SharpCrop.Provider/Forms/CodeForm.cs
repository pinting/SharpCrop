using System;
using System.Windows.Forms;
using SharpCrop.Provider.Properties;

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
        /// Construct a new CodePaste form with an URL and a required code length.
        /// </summary>
        /// <param name="url"></param>
        /// <param name="length">Required length of the activation code.</param>
        public CodeForm(string url = "", int length = 128)
        {
            InitializeComponent();

            // Init texts
            stepOneLabel.Text = Resources.CodeStepOne;
            linkLabel.Text = Resources.CodeLink;
            stepTwoLabel.Text = Resources.CodeStepTwo;
            helpLabel.Text = Resources.CodeHelp;

            // Init variables
            this.length = length;
            linkBox.Text = url;
        }

        /// <summary>
        /// Register a callback function.
        /// </summary>
        /// <param name="callback"></param>
        public void OnResult(Action<string> callback)
        {
            onResult = callback;
        }

        /// <summary>
        /// Call the registered callback when the required code length was reached.
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
        /// Open the browser when the link was clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(linkBox.Text);
        }
    }
}
