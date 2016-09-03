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
            this.resetButton = new System.Windows.Forms.Button();
            this.gifFpsList = new System.Windows.Forms.ComboBox();
            this.noGifRepeatCheckBox = new System.Windows.Forms.CheckBox();
            this.formatLabel = new System.Windows.Forms.Label();
            this.gifFpsLabel = new System.Windows.Forms.Label();
            this.noFocusCheckBox = new System.Windows.Forms.CheckBox();
            this.noTransparencyCheckBox = new System.Windows.Forms.CheckBox();
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
            this.formatList.Size = new System.Drawing.Size(57, 21);
            this.formatList.TabIndex = 0;
            this.formatList.SelectedIndexChanged += new System.EventHandler(this.FormatChanged);
            // 
            // noCopyCheckBox
            // 
            this.noCopyCheckBox.AutoSize = true;
            this.noCopyCheckBox.Location = new System.Drawing.Point(8, 59);
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
            this.noScalingCheckBox.Location = new System.Drawing.Point(8, 78);
            this.noScalingCheckBox.Margin = new System.Windows.Forms.Padding(2);
            this.noScalingCheckBox.Name = "noScalingCheckBox";
            this.noScalingCheckBox.Size = new System.Drawing.Size(130, 17);
            this.noScalingCheckBox.TabIndex = 2;
            this.noScalingCheckBox.Text = "Disable scaling check";
            this.noScalingCheckBox.UseVisualStyleBackColor = true;
            this.noScalingCheckBox.CheckedChanged += new System.EventHandler(this.NoScalingChanged);
            // 
            // resetButton
            // 
            this.resetButton.Location = new System.Drawing.Point(7, 157);
            this.resetButton.Margin = new System.Windows.Forms.Padding(2);
            this.resetButton.Name = "resetButton";
            this.resetButton.Size = new System.Drawing.Size(131, 25);
            this.resetButton.TabIndex = 3;
            this.resetButton.Text = "Reset";
            this.resetButton.UseVisualStyleBackColor = true;
            this.resetButton.Click += new System.EventHandler(this.ResetClick);
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
            this.gifFpsList.Size = new System.Drawing.Size(57, 21);
            this.gifFpsList.TabIndex = 4;
            this.gifFpsList.SelectedIndexChanged += new System.EventHandler(this.GifFpsChanged);
            // 
            // noGifRepeatCheckBox
            // 
            this.noGifRepeatCheckBox.AutoSize = true;
            this.noGifRepeatCheckBox.Location = new System.Drawing.Point(8, 97);
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
            this.formatLabel.Location = new System.Drawing.Point(70, 13);
            this.formatLabel.Name = "formatLabel";
            this.formatLabel.Size = new System.Drawing.Size(68, 13);
            this.formatLabel.TabIndex = 6;
            this.formatLabel.Text = "Image format";
            // 
            // gifFpsLabel
            // 
            this.gifFpsLabel.AutoSize = true;
            this.gifFpsLabel.Location = new System.Drawing.Point(70, 37);
            this.gifFpsLabel.Name = "gifFpsLabel";
            this.gifFpsLabel.Size = new System.Drawing.Size(47, 13);
            this.gifFpsLabel.TabIndex = 7;
            this.gifFpsLabel.Text = "GIF FPS";
            // 
            // noFocusCheckBox
            // 
            this.noFocusCheckBox.AutoSize = true;
            this.noFocusCheckBox.Location = new System.Drawing.Point(8, 116);
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
            this.noTransparencyCheckBox.Location = new System.Drawing.Point(8, 136);
            this.noTransparencyCheckBox.Margin = new System.Windows.Forms.Padding(2);
            this.noTransparencyCheckBox.Name = "noTransparencyCheckBox";
            this.noTransparencyCheckBox.Size = new System.Drawing.Size(125, 17);
            this.noTransparencyCheckBox.TabIndex = 9;
            this.noTransparencyCheckBox.Text = "Disable transparency";
            this.noTransparencyCheckBox.UseVisualStyleBackColor = true;
            this.noTransparencyCheckBox.CheckedChanged += new System.EventHandler(this.NoTransparencyChanged);
            // 
            // ConfigForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(146, 190);
            this.Controls.Add(this.noTransparencyCheckBox);
            this.Controls.Add(this.noFocusCheckBox);
            this.Controls.Add(this.gifFpsLabel);
            this.Controls.Add(this.formatLabel);
            this.Controls.Add(this.noGifRepeatCheckBox);
            this.Controls.Add(this.gifFpsList);
            this.Controls.Add(this.resetButton);
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
        private System.Windows.Forms.Button resetButton;
        private System.Windows.Forms.ComboBox gifFpsList;
        private System.Windows.Forms.CheckBox noGifRepeatCheckBox;
        private System.Windows.Forms.Label formatLabel;
        private System.Windows.Forms.Label gifFpsLabel;
        private System.Windows.Forms.CheckBox noFocusCheckBox;
        private System.Windows.Forms.CheckBox noTransparencyCheckBox;
    }
}