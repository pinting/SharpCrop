using SharpCrop.FTP.Models;
using System;
using System.Windows.Forms;
using SharpCrop.FTP.Properties;

namespace SharpCrop.FTP.Forms
{
    /// <summary>
    /// LoginForm is responsible for getting new FTP credentials.
    /// </summary>
    public partial class LoginForm : Form
    {
        /// <summary>
        /// Executed on form submit.
        /// </summary>
        public event Action<LoginCredentials> OnResult;

        /// <summary>
        /// Consturct a new LoginForm.
        /// </summary>
        public LoginForm()
        {
            InitializeComponent();

            // Init texts
            usernameLabel.Text = Resources.Username;
            passwordLabel.Text = Resources.Password;
            copyPathLabel.Text = Resources.CopyPath;
            remotePathLabel.Text = Resources.RemotePath;
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
                OnResult?.Invoke(new LoginCredentials()
                {
                    Username = usernameBox.Text,
                    Password = passwordBox.Text,
                    RemotePath = new Uri(remotePathBox.Text),
                    CopyPath = new Uri(copyPathBox.Text)
                });
            }
            catch
            {
                // Ignored, because it will be a UriFormatException or a ArgumentNullException
            }
        }
    }
}
