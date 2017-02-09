using SharpCrop.FTP.Models;
using System;
using System.Windows.Forms;

namespace SharpCrop.FTP.Forms
{
    public partial class LoginForm : Form
    {
        private Action<LoginCreds> onResult;

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

        private void OnLogin(object sender, EventArgs e)
        {
            onResult(new LoginCreds()
            {
                Username = usernameBox.Text,
                Password = passwordBox.Text,
                RemotePath = remotePathBox.Text,
                CopyPath = copyPathBox.Text
            });
        }
    }
}
