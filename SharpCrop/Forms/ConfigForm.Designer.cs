namespace SharpCrop.Forms
{
    partial class ConfigForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ConfigForm));
            this.formatList = new System.Windows.Forms.ComboBox();
            this.noCopyCheckBox = new System.Windows.Forms.CheckBox();
            this.noScalingCheckBox = new System.Windows.Forms.CheckBox();
            this.unlinkButton = new System.Windows.Forms.Button();
            this.videoFpsList = new System.Windows.Forms.ComboBox();
            this.noGifRepeatCheckBox = new System.Windows.Forms.CheckBox();
            this.formatLabel = new System.Windows.Forms.Label();
            this.gifFpsLabel = new System.Windows.Forms.Label();
            this.noFocusCheckBox = new System.Windows.Forms.CheckBox();
            this.noTransparencyCheckBox = new System.Windows.Forms.CheckBox();
            this.manualScallingBox = new System.Windows.Forms.TextBox();
            this.manualScallingLabel = new System.Windows.Forms.Label();
            this.enableMpegCheckbox = new System.Windows.Forms.CheckBox();
            this.localFileLink = new System.Windows.Forms.LinkLabel();
            this.oneDriveButton = new System.Windows.Forms.Button();
            this.googleDriveButton = new System.Windows.Forms.Button();
            this.dropboxButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // formatList
            // 
            this.formatList.FormattingEnabled = true;
            this.formatList.Items.AddRange(new object[] {
            "jpg",
            "png",
            "bmp"});
            this.formatList.Location = new System.Drawing.Point(8, 8);
            this.formatList.Margin = new System.Windows.Forms.Padding(2);
            this.formatList.Name = "formatList";
            this.formatList.Size = new System.Drawing.Size(100, 21);
            this.formatList.TabIndex = 0;
            this.formatList.SelectedIndexChanged += new System.EventHandler(this.FormatChanged);
            // 
            // noCopyCheckBox
            // 
            this.noCopyCheckBox.AutoSize = true;
            this.noCopyCheckBox.Location = new System.Drawing.Point(8, 107);
            this.noCopyCheckBox.Margin = new System.Windows.Forms.Padding(2);
            this.noCopyCheckBox.Name = "noCopyCheckBox";
            this.noCopyCheckBox.Size = new System.Drawing.Size(112, 17);
            this.noCopyCheckBox.TabIndex = 1;
            this.noCopyCheckBox.Text = "Disable URL copy";
            this.noCopyCheckBox.UseVisualStyleBackColor = true;
            this.noCopyCheckBox.CheckedChanged += new System.EventHandler(this.NoCopyChanged);
            // 
            // noScalingCheckBox
            // 
            this.noScalingCheckBox.AutoSize = true;
            this.noScalingCheckBox.Location = new System.Drawing.Point(8, 88);
            this.noScalingCheckBox.Margin = new System.Windows.Forms.Padding(2);
            this.noScalingCheckBox.Name = "noScalingCheckBox";
            this.noScalingCheckBox.Size = new System.Drawing.Size(132, 17);
            this.noScalingCheckBox.TabIndex = 2;
            this.noScalingCheckBox.Text = "Enable manual scaling";
            this.noScalingCheckBox.UseVisualStyleBackColor = true;
            this.noScalingCheckBox.CheckedChanged += new System.EventHandler(this.NoScalingChanged);
            // 
            // unlinkButton
            // 
            this.unlinkButton.Location = new System.Drawing.Point(7, 207);
            this.unlinkButton.Margin = new System.Windows.Forms.Padding(2);
            this.unlinkButton.Name = "unlinkButton";
            this.unlinkButton.Size = new System.Drawing.Size(188, 25);
            this.unlinkButton.TabIndex = 3;
            this.unlinkButton.Text = "Unlink providers";
            this.unlinkButton.UseVisualStyleBackColor = true;
            this.unlinkButton.Click += new System.EventHandler(this.UnlinkClicked);
            // 
            // videoFpsList
            // 
            this.videoFpsList.FormattingEnabled = true;
            this.videoFpsList.Items.AddRange(new object[] {
            "5",
            "10",
            "15",
            "20",
            "25",
            "30"});
            this.videoFpsList.Location = new System.Drawing.Point(8, 33);
            this.videoFpsList.Margin = new System.Windows.Forms.Padding(2);
            this.videoFpsList.Name = "videoFpsList";
            this.videoFpsList.Size = new System.Drawing.Size(100, 21);
            this.videoFpsList.TabIndex = 4;
            this.videoFpsList.SelectedIndexChanged += new System.EventHandler(this.GifFpsChanged);
            // 
            // noGifRepeatCheckBox
            // 
            this.noGifRepeatCheckBox.AutoSize = true;
            this.noGifRepeatCheckBox.Location = new System.Drawing.Point(8, 126);
            this.noGifRepeatCheckBox.Margin = new System.Windows.Forms.Padding(2);
            this.noGifRepeatCheckBox.Name = "noGifRepeatCheckBox";
            this.noGifRepeatCheckBox.Size = new System.Drawing.Size(114, 17);
            this.noGifRepeatCheckBox.TabIndex = 5;
            this.noGifRepeatCheckBox.Text = "Disable GIF repeat";
            this.noGifRepeatCheckBox.UseVisualStyleBackColor = true;
            this.noGifRepeatCheckBox.CheckedChanged += new System.EventHandler(this.NoGifRepeatChanged);
            // 
            // formatLabel
            // 
            this.formatLabel.AutoSize = true;
            this.formatLabel.Location = new System.Drawing.Point(117, 13);
            this.formatLabel.Name = "formatLabel";
            this.formatLabel.Size = new System.Drawing.Size(68, 13);
            this.formatLabel.TabIndex = 6;
            this.formatLabel.Text = "Image format";
            // 
            // gifFpsLabel
            // 
            this.gifFpsLabel.AutoSize = true;
            this.gifFpsLabel.Location = new System.Drawing.Point(117, 37);
            this.gifFpsLabel.Name = "gifFpsLabel";
            this.gifFpsLabel.Size = new System.Drawing.Size(57, 13);
            this.gifFpsLabel.TabIndex = 7;
            this.gifFpsLabel.Text = "Video FPS";
            // 
            // noFocusCheckBox
            // 
            this.noFocusCheckBox.AutoSize = true;
            this.noFocusCheckBox.Location = new System.Drawing.Point(8, 145);
            this.noFocusCheckBox.Margin = new System.Windows.Forms.Padding(2);
            this.noFocusCheckBox.Name = "noFocusCheckBox";
            this.noFocusCheckBox.Size = new System.Drawing.Size(128, 17);
            this.noFocusCheckBox.TabIndex = 8;
            this.noFocusCheckBox.Text = "Launch without focus";
            this.noFocusCheckBox.UseVisualStyleBackColor = true;
            this.noFocusCheckBox.CheckedChanged += new System.EventHandler(this.NoFocusChanged);
            // 
            // noTransparencyCheckBox
            // 
            this.noTransparencyCheckBox.AutoSize = true;
            this.noTransparencyCheckBox.Location = new System.Drawing.Point(8, 165);
            this.noTransparencyCheckBox.Margin = new System.Windows.Forms.Padding(2);
            this.noTransparencyCheckBox.Name = "noTransparencyCheckBox";
            this.noTransparencyCheckBox.Size = new System.Drawing.Size(125, 17);
            this.noTransparencyCheckBox.TabIndex = 9;
            this.noTransparencyCheckBox.Text = "Disable transparency";
            this.noTransparencyCheckBox.UseVisualStyleBackColor = true;
            this.noTransparencyCheckBox.CheckedChanged += new System.EventHandler(this.NoTransparencyChanged);
            // 
            // manualScallingBox
            // 
            this.manualScallingBox.Location = new System.Drawing.Point(8, 59);
            this.manualScallingBox.Name = "manualScallingBox";
            this.manualScallingBox.Size = new System.Drawing.Size(100, 20);
            this.manualScallingBox.TabIndex = 10;
            this.manualScallingBox.TextChanged += new System.EventHandler(this.ManualScalingChanged);
            // 
            // manualScallingLabel
            // 
            this.manualScallingLabel.AutoSize = true;
            this.manualScallingLabel.Location = new System.Drawing.Point(117, 62);
            this.manualScallingLabel.Name = "manualScallingLabel";
            this.manualScallingLabel.Size = new System.Drawing.Size(78, 13);
            this.manualScallingLabel.TabIndex = 11;
            this.manualScallingLabel.Text = "Manual scaling";
            // 
            // enableMpegCheckbox
            // 
            this.enableMpegCheckbox.AutoSize = true;
            this.enableMpegCheckbox.Location = new System.Drawing.Point(8, 185);
            this.enableMpegCheckbox.Margin = new System.Windows.Forms.Padding(2);
            this.enableMpegCheckbox.Name = "enableMpegCheckbox";
            this.enableMpegCheckbox.Size = new System.Drawing.Size(188, 17);
            this.enableMpegCheckbox.TabIndex = 12;
            this.enableMpegCheckbox.Text = "Record in MPEG (ffmpeg required)";
            this.enableMpegCheckbox.UseVisualStyleBackColor = true;
            this.enableMpegCheckbox.CheckedChanged += new System.EventHandler(this.EnableMpegChanged);
            // 
            // localFileLink
            // 
            this.localFileLink.AutoSize = true;
            this.localFileLink.Location = new System.Drawing.Point(223, 216);
            this.localFileLink.Name = "localFileLink";
            this.localFileLink.Size = new System.Drawing.Size(129, 13);
            this.localFileLink.TabIndex = 16;
            this.localFileLink.TabStop = true;
            this.localFileLink.Text = "Save output as a local file";
            this.localFileLink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.OnLocalFile);
            // 
            // oneDriveButton
            // 
            this.oneDriveButton.BackColor = System.Drawing.Color.SandyBrown;
            this.oneDriveButton.Location = new System.Drawing.Point(568, 33);
            this.oneDriveButton.Margin = new System.Windows.Forms.Padding(2);
            this.oneDriveButton.Name = "oneDriveButton";
            this.oneDriveButton.Size = new System.Drawing.Size(156, 179);
            this.oneDriveButton.TabIndex = 15;
            this.oneDriveButton.Text = "OneDrive";
            this.oneDriveButton.UseVisualStyleBackColor = false;
            this.oneDriveButton.Click += new System.EventHandler(this.OnOneDrive);
            // 
            // googleDriveButton
            // 
            this.googleDriveButton.BackColor = System.Drawing.Color.PaleGreen;
            this.googleDriveButton.Location = new System.Drawing.Point(397, 33);
            this.googleDriveButton.Margin = new System.Windows.Forms.Padding(2);
            this.googleDriveButton.Name = "googleDriveButton";
            this.googleDriveButton.Size = new System.Drawing.Size(167, 179);
            this.googleDriveButton.TabIndex = 14;
            this.googleDriveButton.Text = "Google Drive";
            this.googleDriveButton.UseVisualStyleBackColor = false;
            this.googleDriveButton.Click += new System.EventHandler(this.OnGoogleDrive);
            // 
            // dropboxButton
            // 
            this.dropboxButton.BackColor = System.Drawing.Color.PaleTurquoise;
            this.dropboxButton.Location = new System.Drawing.Point(225, 33);
            this.dropboxButton.Margin = new System.Windows.Forms.Padding(2);
            this.dropboxButton.Name = "dropboxButton";
            this.dropboxButton.Size = new System.Drawing.Size(167, 179);
            this.dropboxButton.TabIndex = 13;
            this.dropboxButton.Text = "Dropbox";
            this.dropboxButton.UseVisualStyleBackColor = false;
            this.dropboxButton.Click += new System.EventHandler(this.OnDropbox);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(225, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(99, 13);
            this.label1.TabIndex = 17;
            this.label1.Text = "Add a new provider";
            // 
            // ConfigForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(754, 244);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.localFileLink);
            this.Controls.Add(this.oneDriveButton);
            this.Controls.Add(this.googleDriveButton);
            this.Controls.Add(this.dropboxButton);
            this.Controls.Add(this.enableMpegCheckbox);
            this.Controls.Add(this.manualScallingLabel);
            this.Controls.Add(this.manualScallingBox);
            this.Controls.Add(this.noTransparencyCheckBox);
            this.Controls.Add(this.noFocusCheckBox);
            this.Controls.Add(this.gifFpsLabel);
            this.Controls.Add(this.formatLabel);
            this.Controls.Add(this.noGifRepeatCheckBox);
            this.Controls.Add(this.videoFpsList);
            this.Controls.Add(this.unlinkButton);
            this.Controls.Add(this.noScalingCheckBox);
            this.Controls.Add(this.noCopyCheckBox);
            this.Controls.Add(this.formatList);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "ConfigForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "SharpCrop";
            this.TopMost = true;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox formatList;
        private System.Windows.Forms.CheckBox noCopyCheckBox;
        private System.Windows.Forms.CheckBox noScalingCheckBox;
        private System.Windows.Forms.Button unlinkButton;
        private System.Windows.Forms.ComboBox videoFpsList;
        private System.Windows.Forms.CheckBox noGifRepeatCheckBox;
        private System.Windows.Forms.Label formatLabel;
        private System.Windows.Forms.Label gifFpsLabel;
        private System.Windows.Forms.CheckBox noFocusCheckBox;
        private System.Windows.Forms.CheckBox noTransparencyCheckBox;
        private System.Windows.Forms.TextBox manualScallingBox;
        private System.Windows.Forms.Label manualScallingLabel;
        private System.Windows.Forms.CheckBox enableMpegCheckbox;
        private System.Windows.Forms.LinkLabel localFileLink;
        private System.Windows.Forms.Button oneDriveButton;
        private System.Windows.Forms.Button googleDriveButton;
        private System.Windows.Forms.Button dropboxButton;
        private System.Windows.Forms.Label label1;
    }
}