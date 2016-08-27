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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.dropboxButton = new System.Windows.Forms.Button();
            this.googleDriveButton = new System.Windows.Forms.Button();
            this.oneDriveButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // dropboxButton
            // 
            this.dropboxButton.BackColor = System.Drawing.Color.PaleTurquoise;
            this.dropboxButton.Location = new System.Drawing.Point(11, 15);
            this.dropboxButton.Margin = new System.Windows.Forms.Padding(2);
            this.dropboxButton.Name = "dropboxButton";
            this.dropboxButton.Size = new System.Drawing.Size(167, 163);
            this.dropboxButton.TabIndex = 0;
            this.dropboxButton.Text = "Dropbox";
            this.dropboxButton.UseVisualStyleBackColor = false;
            this.dropboxButton.Click += new System.EventHandler(this.OnDropbox);
            // 
            // googleDriveButton
            // 
            this.googleDriveButton.BackColor = System.Drawing.Color.PaleGreen;
            this.googleDriveButton.Location = new System.Drawing.Point(183, 15);
            this.googleDriveButton.Margin = new System.Windows.Forms.Padding(2);
            this.googleDriveButton.Name = "googleDriveButton";
            this.googleDriveButton.Size = new System.Drawing.Size(167, 163);
            this.googleDriveButton.TabIndex = 1;
            this.googleDriveButton.Text = "Google Drive";
            this.googleDriveButton.UseVisualStyleBackColor = false;
            this.googleDriveButton.Click += new System.EventHandler(this.OnGoogleDrive);
            // 
            // oneDriveButton
            // 
            this.oneDriveButton.BackColor = System.Drawing.Color.SandyBrown;
            this.oneDriveButton.Location = new System.Drawing.Point(354, 15);
            this.oneDriveButton.Margin = new System.Windows.Forms.Padding(2);
            this.oneDriveButton.Name = "oneDriveButton";
            this.oneDriveButton.Size = new System.Drawing.Size(156, 163);
            this.oneDriveButton.TabIndex = 2;
            this.oneDriveButton.Text = "OneDrive";
            this.oneDriveButton.UseVisualStyleBackColor = false;
            this.oneDriveButton.Click += new System.EventHandler(this.OnOneDrive);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(519, 193);
            this.Controls.Add(this.oneDriveButton);
            this.Controls.Add(this.googleDriveButton);
            this.Controls.Add(this.dropboxButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "SharpCrop";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button dropboxButton;
        private System.Windows.Forms.Button googleDriveButton;
        private System.Windows.Forms.Button oneDriveButton;
    }
}