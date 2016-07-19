namespace SharpCrop.Forms
{
    partial class MainForm
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
            this.externalLogin = new System.Windows.Forms.Button();
            this.internalLogin = new System.Windows.Forms.LinkLabel();
            this.SuspendLayout();
            // 
            // externalLogin
            // 
            this.externalLogin.Location = new System.Drawing.Point(12, 12);
            this.externalLogin.Name = "externalLogin";
            this.externalLogin.Size = new System.Drawing.Size(751, 88);
            this.externalLogin.TabIndex = 0;
            this.externalLogin.Text = "Connect to Dropbox";
            this.externalLogin.UseVisualStyleBackColor = true;
            this.externalLogin.Click += new System.EventHandler(this.ExternalLogin);
            // 
            // internalLogin
            // 
            this.internalLogin.AutoSize = true;
            this.internalLogin.Location = new System.Drawing.Point(276, 103);
            this.internalLogin.Name = "internalLogin";
            this.internalLogin.Size = new System.Drawing.Size(206, 20);
            this.internalLogin.TabIndex = 1;
            this.internalLogin.TabStop = true;
            this.internalLogin.Text = "Login using internal browser";
            this.internalLogin.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.InternalLogin);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(778, 144);
            this.Controls.Add(this.internalLogin);
            this.Controls.Add(this.externalLogin);
            this.Name = "MainForm";
            this.Text = "SharpCrop";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button externalLogin;
        private System.Windows.Forms.LinkLabel internalLogin;
    }
}