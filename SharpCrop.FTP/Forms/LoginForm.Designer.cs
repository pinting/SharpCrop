namespace SharpCrop.FTP.Forms
{
    partial class LoginForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LoginForm));
            this.loginButton = new System.Windows.Forms.Button();
            this.copyPathLabel = new System.Windows.Forms.Label();
            this.remotePathLabel = new System.Windows.Forms.Label();
            this.passwordLabel = new System.Windows.Forms.Label();
            this.usernameLabel = new System.Windows.Forms.Label();
            this.copyPathBox = new System.Windows.Forms.TextBox();
            this.remotePathBox = new System.Windows.Forms.TextBox();
            this.passwordBox = new System.Windows.Forms.TextBox();
            this.usernameBox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // loginButton
            // 
            this.loginButton.Location = new System.Drawing.Point(12, 117);
            this.loginButton.Name = "loginButton";
            this.loginButton.Size = new System.Drawing.Size(222, 23);
            this.loginButton.TabIndex = 17;
            this.loginButton.Text = "Login";
            this.loginButton.UseVisualStyleBackColor = true;
            this.loginButton.Click += new System.EventHandler(this.OnLogin);
            // 
            // copyPathLabel
            // 
            this.copyPathLabel.AutoSize = true;
            this.copyPathLabel.Location = new System.Drawing.Point(166, 93);
            this.copyPathLabel.Name = "copyPathLabel";
            this.copyPathLabel.Size = new System.Drawing.Size(78, 13);
            this.copyPathLabel.TabIndex = 16;
            this.copyPathLabel.Text = "copyPathLabel";
            // 
            // remotePathLabel
            // 
            this.remotePathLabel.AutoSize = true;
            this.remotePathLabel.Location = new System.Drawing.Point(166, 67);
            this.remotePathLabel.Name = "remotePathLabel";
            this.remotePathLabel.Size = new System.Drawing.Size(87, 13);
            this.remotePathLabel.TabIndex = 15;
            this.remotePathLabel.Text = "remotePathLabel";
            // 
            // passwordLabel
            // 
            this.passwordLabel.AutoSize = true;
            this.passwordLabel.Location = new System.Drawing.Point(166, 41);
            this.passwordLabel.Name = "passwordLabel";
            this.passwordLabel.Size = new System.Drawing.Size(78, 13);
            this.passwordLabel.TabIndex = 14;
            this.passwordLabel.Text = "passwordLabel";
            // 
            // usernameLabel
            // 
            this.usernameLabel.AutoSize = true;
            this.usernameLabel.Location = new System.Drawing.Point(166, 15);
            this.usernameLabel.Name = "usernameLabel";
            this.usernameLabel.Size = new System.Drawing.Size(79, 13);
            this.usernameLabel.TabIndex = 13;
            this.usernameLabel.Text = "usernameLabel";
            // 
            // copyPathBox
            // 
            this.copyPathBox.Location = new System.Drawing.Point(12, 90);
            this.copyPathBox.Name = "copyPathBox";
            this.copyPathBox.Size = new System.Drawing.Size(149, 20);
            this.copyPathBox.TabIndex = 12;
            this.copyPathBox.Text = "http://";
            // 
            // remotePathBox
            // 
            this.remotePathBox.Location = new System.Drawing.Point(12, 64);
            this.remotePathBox.Name = "remotePathBox";
            this.remotePathBox.Size = new System.Drawing.Size(149, 20);
            this.remotePathBox.TabIndex = 11;
            this.remotePathBox.Text = "ftp://";
            // 
            // passwordBox
            // 
            this.passwordBox.Location = new System.Drawing.Point(12, 38);
            this.passwordBox.Name = "passwordBox";
            this.passwordBox.PasswordChar = '*';
            this.passwordBox.Size = new System.Drawing.Size(149, 20);
            this.passwordBox.TabIndex = 10;
            // 
            // usernameBox
            // 
            this.usernameBox.Location = new System.Drawing.Point(12, 12);
            this.usernameBox.Name = "usernameBox";
            this.usernameBox.Size = new System.Drawing.Size(149, 20);
            this.usernameBox.TabIndex = 9;
            this.usernameBox.Text = "anonymous";
            // 
            // LoginForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(242, 148);
            this.Controls.Add(this.loginButton);
            this.Controls.Add(this.copyPathLabel);
            this.Controls.Add(this.remotePathLabel);
            this.Controls.Add(this.passwordLabel);
            this.Controls.Add(this.usernameLabel);
            this.Controls.Add(this.copyPathBox);
            this.Controls.Add(this.remotePathBox);
            this.Controls.Add(this.passwordBox);
            this.Controls.Add(this.usernameBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "LoginForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "SharpCrop";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button loginButton;
        private System.Windows.Forms.Label copyPathLabel;
        private System.Windows.Forms.Label remotePathLabel;
        private System.Windows.Forms.Label passwordLabel;
        private System.Windows.Forms.Label usernameLabel;
        private System.Windows.Forms.TextBox copyPathBox;
        private System.Windows.Forms.TextBox remotePathBox;
        private System.Windows.Forms.TextBox passwordBox;
        private System.Windows.Forms.TextBox usernameBox;
    }
}