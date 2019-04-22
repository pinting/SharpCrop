using System;
using System.Drawing;
using System.Windows.Forms;
using SharpCrop.FTP.Models;
using SharpCrop.FTP.Properties;

namespace SharpCrop.FTP.Forms
{
    /// <summary>
    /// LoginForm is responsible for getting new FTP credentials.
    /// </summary>
    public class LoginForm : Form
    {
        private Button loginButton;
        private Label copyPathLabel;
        private Label remotePathLabel;
        private Label passwordLabel;
        private Label usernameLabel;
        private TextBox copyPathBox;
        private TextBox remotePathBox;
        private TextBox passwordBox;
        private TextBox usernameBox;
                
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
        }
        
        private void InitializeComponent()
        {
            SuspendLayout();

            // Password label
            passwordLabel = new Label();
            passwordLabel.AutoSize = true;
            passwordLabel.Location = new Point(165, 40);
            passwordLabel.Size = new Size(80, 15);
            passwordLabel.TabIndex = 14;
            passwordLabel.Text = Resources.Password;
            
            // Password box
            passwordBox = new TextBox();
            passwordBox.Location = new Point(15, 40);
            passwordBox.PasswordChar = '*';
            passwordBox.Size = new Size(150, 20);
            passwordBox.TabIndex = 10;
            
            // Username label
            usernameLabel = new Label();
            usernameLabel.AutoSize = true;
            usernameLabel.Location = new Point(165, 15);
            usernameLabel.Size = new Size(80, 15);
            usernameLabel.TabIndex = 13;
            usernameLabel.Text = Resources.Username;
            
            // Username box
            usernameBox = new TextBox();
            usernameBox.Location = new Point(15, 15);
            usernameBox.Size = new Size(150, 20);
            usernameBox.TabIndex = 9;
            usernameBox.Text = Config.DefaultUsername;
            
            // Copy path label
            copyPathLabel = new Label();
            copyPathLabel.AutoSize = true;
            copyPathLabel.Location = new Point(165, 90);
            copyPathLabel.Size = new Size(80, 15);
            copyPathLabel.TabIndex = 16;
            copyPathLabel.Text = Resources.CopyPath;
            
            // Copy path box
            copyPathBox = new TextBox();
            copyPathBox.Location = new Point(15, 90);
            copyPathBox.Size = new Size(150, 20);
            copyPathBox.TabIndex = 12;
            copyPathBox.Text = Config.DefaultCopyPath;
            
            // Remote path box
            remotePathBox = new TextBox();
            remotePathBox.Location = new Point(15, 65);
            remotePathBox.Size = new Size(150, 20);
            remotePathBox.TabIndex = 11;
            remotePathBox.Text = Config.DefaultRemotePath;
            
            // Remote path label
            remotePathLabel = new Label();
            remotePathLabel.AutoSize = true;
            remotePathLabel.Location = new Point(165, 65);
            remotePathLabel.Size = new Size(90, 15);
            remotePathLabel.TabIndex = 15;
            remotePathLabel.Text = Resources.RemotePath;

            // Login button
            loginButton = new Button();
            loginButton.Location = new Point(15, 120);
            loginButton.Size = new Size(220, 25);
            loginButton.TabIndex = 17;
            loginButton.UseVisualStyleBackColor = true;
            loginButton.Click += OnLogin;
            loginButton.Text = Resources.Login;

            
            AutoScaleDimensions = new SizeF(6F, 13F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(245, 150);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "SharpCrop";
            
            Controls.Add(loginButton);
            Controls.Add(copyPathLabel);
            Controls.Add(remotePathLabel);
            Controls.Add(passwordLabel);
            Controls.Add(usernameLabel);
            Controls.Add(copyPathBox);
            Controls.Add(remotePathBox);
            Controls.Add(passwordBox);
            Controls.Add(usernameBox);
            
            ResumeLayout(false);
            PerformLayout();
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
                OnResult?.Invoke(new LoginCredentials
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
