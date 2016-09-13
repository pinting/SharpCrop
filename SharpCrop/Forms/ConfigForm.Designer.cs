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
            this.gifFpsList = new System.Windows.Forms.ComboBox();
            this.noGifRepeatCheckBox = new System.Windows.Forms.CheckBox();
            this.formatLabel = new System.Windows.Forms.Label();
            this.gifFpsLabel = new System.Windows.Forms.Label();
            this.noFocusCheckBox = new System.Windows.Forms.CheckBox();
            this.noTransparencyCheckBox = new System.Windows.Forms.CheckBox();
            this.manualScallingBox = new System.Windows.Forms.TextBox();
            this.manualScallingLabel = new System.Windows.Forms.Label();
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
            this.noCopyCheckBox.Location = new System.Drawing.Point(8, 88);
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
            this.noScalingCheckBox.Location = new System.Drawing.Point(8, 107);
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
            this.unlinkButton.Location = new System.Drawing.Point(7, 186);
            this.unlinkButton.Margin = new System.Windows.Forms.Padding(2);
            this.unlinkButton.Name = "unlinkButton";
            this.unlinkButton.Size = new System.Drawing.Size(188, 25);
            this.unlinkButton.TabIndex = 3;
            this.unlinkButton.Text = "Unlink";
            this.unlinkButton.UseVisualStyleBackColor = true;
            this.unlinkButton.Click += new System.EventHandler(this.UnlinkClick);
            // 
            // gifFpsList
            // 
            this.gifFpsList.FormattingEnabled = true;
            this.gifFpsList.Items.AddRange(new object[] {
            "5",
            "10",
            "15",
            "20",
            "25",
            "30"});
            this.gifFpsList.Location = new System.Drawing.Point(8, 33);
            this.gifFpsList.Margin = new System.Windows.Forms.Padding(2);
            this.gifFpsList.Name = "gifFpsList";
            this.gifFpsList.Size = new System.Drawing.Size(100, 21);
            this.gifFpsList.TabIndex = 4;
            this.gifFpsList.SelectedIndexChanged += new System.EventHandler(this.GifFpsChanged);
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
            this.gifFpsLabel.Size = new System.Drawing.Size(47, 13);
            this.gifFpsLabel.TabIndex = 7;
            this.gifFpsLabel.Text = "GIF FPS";
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
            // ConfigForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(201, 221);
            this.Controls.Add(this.manualScallingLabel);
            this.Controls.Add(this.manualScallingBox);
            this.Controls.Add(this.noTransparencyCheckBox);
            this.Controls.Add(this.noFocusCheckBox);
            this.Controls.Add(this.gifFpsLabel);
            this.Controls.Add(this.formatLabel);
            this.Controls.Add(this.noGifRepeatCheckBox);
            this.Controls.Add(this.gifFpsList);
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
        private System.Windows.Forms.ComboBox gifFpsList;
        private System.Windows.Forms.CheckBox noGifRepeatCheckBox;
        private System.Windows.Forms.Label formatLabel;
        private System.Windows.Forms.Label gifFpsLabel;
        private System.Windows.Forms.CheckBox noFocusCheckBox;
        private System.Windows.Forms.CheckBox noTransparencyCheckBox;
        private System.Windows.Forms.TextBox manualScallingBox;
        private System.Windows.Forms.Label manualScallingLabel;
    }
}