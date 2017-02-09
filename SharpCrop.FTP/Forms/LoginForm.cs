using SharpCrop.FTP.Models;
using System;
using System.Windows.Forms;

namespace SharpCrop.FTP.Forms
{
    /// <summary>
    /// LoginForm is responsible for FTP login.
    /// </summary>
    public partial class LoginForm : Form
    {
        private Action<LoginCreds> onResult;

        /// <summary>
        /// Consturct a new LoginForm.
        /// </summary>
        public LoginForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Register for callback.
        /// </summary>
        /// <param name="callback"></param>
        public void OnResult(Action<LoginCreds> callback)
        {
            onResult = callback;
        }

        /// <summary>
        /// Executed when the login button is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnLogin(object sender, EventArgs e)
        {
            try
            {
                onResult?.Invoke(new LoginCreds()
                {
                    Username = usernameBox.Text,
                    Password = passwordBox.Text,
                    RemotePath = new Uri(remotePathBox.Text),
                    CopyPath = new Uri(copyPathBox.Text)
                });
            }
            catch
            {
                // Ignored
            }
        }
    }
}
