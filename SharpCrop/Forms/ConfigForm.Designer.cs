namespace SharpCrop.Forms
{
    sealed partial class ConfigForm
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
            this.formatBox = new System.Windows.Forms.ComboBox();
            this.noCopyCheckBox = new System.Windows.Forms.CheckBox();
            this.noScalingCheckBox = new System.Windows.Forms.CheckBox();
            this.videoFpsBox = new System.Windows.Forms.ComboBox();
            this.noGifRepeatCheckBox = new System.Windows.Forms.CheckBox();
            this.formatLabel = new System.Windows.Forms.Label();
            this.videoFpsLabel = new System.Windows.Forms.Label();
            this.noFocusCheckBox = new System.Windows.Forms.CheckBox();
            this.noTransparencyCheckBox = new System.Windows.Forms.CheckBox();
            this.manualScallingBox = new System.Windows.Forms.TextBox();
            this.manualScallingLabel = new System.Windows.Forms.Label();
            this.enableMpegCheckbox = new System.Windows.Forms.CheckBox();
            this.addProviderBox = new System.Windows.Forms.ListBox();
            this.resetButton = new System.Windows.Forms.Button();
            this.removeProviderBox = new System.Windows.Forms.ListBox();
            this.removeProviderLabel = new System.Windows.Forms.Label();
            this.addProviderLabel = new System.Windows.Forms.Label();
            this.copyProviderBox = new System.Windows.Forms.Label();
            this.urlToCopyBox = new System.Windows.Forms.ComboBox();
            this.startupLoadCheckBox = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // formatBox
            // 
            this.formatBox.FormattingEnabled = true;
            this.formatBox.Location = new System.Drawing.Point(8, 8);
            this.formatBox.Margin = new System.Windows.Forms.Padding(2);
            this.formatBox.Name = "formatBox";
            this.formatBox.Size = new System.Drawing.Size(100, 21);
            this.formatBox.TabIndex = 0;
            this.formatBox.SelectedIndexChanged += new System.EventHandler(this.FormatChanged);
            // 
            // noCopyCheckBox
            // 
            this.noCopyCheckBox.AutoSize = true;
            this.noCopyCheckBox.Location = new System.Drawing.Point(8, 134);
            this.noCopyCheckBox.Margin = new System.Windows.Forms.Padding(2);
            this.noCopyCheckBox.Name = "noCopyCheckBox";
            this.noCopyCheckBox.Size = new System.Drawing.Size(111, 17);
            this.noCopyCheckBox.TabIndex = 1;
            this.noCopyCheckBox.Text = "noCopyCheckBox";
            this.noCopyCheckBox.UseVisualStyleBackColor = true;
            this.noCopyCheckBox.CheckedChanged += new System.EventHandler(this.NoCopyChanged);
            // 
            // noScalingCheckBox
            // 
            this.noScalingCheckBox.AutoSize = true;
            this.noScalingCheckBox.Location = new System.Drawing.Point(8, 115);
            this.noScalingCheckBox.Margin = new System.Windows.Forms.Padding(2);
            this.noScalingCheckBox.Name = "noScalingCheckBox";
            this.noScalingCheckBox.Size = new System.Drawing.Size(122, 17);
            this.noScalingCheckBox.TabIndex = 2;
            this.noScalingCheckBox.Text = "noScalingCheckBox";
            this.noScalingCheckBox.UseVisualStyleBackColor = true;
            this.noScalingCheckBox.CheckedChanged += new System.EventHandler(this.NoScalingChanged);
            // 
            // videoFpsBox
            // 
            this.videoFpsBox.FormattingEnabled = true;
            this.videoFpsBox.Location = new System.Drawing.Point(8, 33);
            this.videoFpsBox.Margin = new System.Windows.Forms.Padding(2);
            this.videoFpsBox.Name = "videoFpsBox";
            this.videoFpsBox.Size = new System.Drawing.Size(100, 21);
            this.videoFpsBox.TabIndex = 4;
            this.videoFpsBox.SelectedIndexChanged += new System.EventHandler(this.GifFpsChanged);
            // 
            // noGifRepeatCheckBox
            // 
            this.noGifRepeatCheckBox.AutoSize = true;
            this.noGifRepeatCheckBox.Location = new System.Drawing.Point(8, 153);
            this.noGifRepeatCheckBox.Margin = new System.Windows.Forms.Padding(2);
            this.noGifRepeatCheckBox.Name = "noGifRepeatCheckBox";
            this.noGifRepeatCheckBox.Size = new System.Drawing.Size(135, 17);
            this.noGifRepeatCheckBox.TabIndex = 5;
            this.noGifRepeatCheckBox.Text = "noGifRepeatCheckBox";
            this.noGifRepeatCheckBox.UseVisualStyleBackColor = true;
            this.noGifRepeatCheckBox.CheckedChanged += new System.EventHandler(this.NoGifRepeatChanged);
            // 
            // formatLabel
            // 
            this.formatLabel.AutoSize = true;
            this.formatLabel.Location = new System.Drawing.Point(117, 13);
            this.formatLabel.Name = "formatLabel";
            this.formatLabel.Size = new System.Drawing.Size(62, 13);
            this.formatLabel.TabIndex = 6;
            this.formatLabel.Text = "formatLabel";
            // 
            // videoFpsLabel
            // 
            this.videoFpsLabel.AutoSize = true;
            this.videoFpsLabel.Location = new System.Drawing.Point(117, 37);
            this.videoFpsLabel.Name = "videoFpsLabel";
            this.videoFpsLabel.Size = new System.Drawing.Size(61, 13);
            this.videoFpsLabel.TabIndex = 7;
            this.videoFpsLabel.Text = "videoFpsLabel";
            // 
            // noFocusCheckBox
            // 
            this.noFocusCheckBox.AutoSize = true;
            this.noFocusCheckBox.Location = new System.Drawing.Point(8, 172);
            this.noFocusCheckBox.Margin = new System.Windows.Forms.Padding(2);
            this.noFocusCheckBox.Name = "noFocusCheckBox";
            this.noFocusCheckBox.Size = new System.Drawing.Size(116, 17);
            this.noFocusCheckBox.TabIndex = 8;
            this.noFocusCheckBox.Text = "noFocusCheckBox";
            this.noFocusCheckBox.UseVisualStyleBackColor = true;
            this.noFocusCheckBox.CheckedChanged += new System.EventHandler(this.NoFocusChanged);
            // 
            // noTransparencyCheckBox
            // 
            this.noTransparencyCheckBox.AutoSize = true;
            this.noTransparencyCheckBox.Location = new System.Drawing.Point(8, 192);
            this.noTransparencyCheckBox.Margin = new System.Windows.Forms.Padding(2);
            this.noTransparencyCheckBox.Name = "noTransparencyCheckBox";
            this.noTransparencyCheckBox.Size = new System.Drawing.Size(152, 17);
            this.noTransparencyCheckBox.TabIndex = 9;
            this.noTransparencyCheckBox.Text = "noTransparencyCheckBox";
            this.noTransparencyCheckBox.UseVisualStyleBackColor = true;
            this.noTransparencyCheckBox.CheckedChanged += new System.EventHandler(this.NoTransparencyChanged);
            // 
            // manualScallingBox
            // 
            this.manualScallingBox.Location = new System.Drawing.Point(8, 86);
            this.manualScallingBox.Name = "manualScallingBox";
            this.manualScallingBox.Size = new System.Drawing.Size(100, 20);
            this.manualScallingBox.TabIndex = 10;
            this.manualScallingBox.TextChanged += new System.EventHandler(this.ManualScalingChanged);
            // 
            // manualScallingLabel
            // 
            this.manualScallingLabel.AutoSize = true;
            this.manualScallingLabel.Location = new System.Drawing.Point(117, 90);
            this.manualScallingLabel.Name = "manualScallingLabel";
            this.manualScallingLabel.Size = new System.Drawing.Size(104, 13);
            this.manualScallingLabel.TabIndex = 11;
            this.manualScallingLabel.Text = "manualScallingLabel";
            // 
            // enableMpegCheckbox
            // 
            this.enableMpegCheckbox.AutoSize = true;
            this.enableMpegCheckbox.Location = new System.Drawing.Point(8, 212);
            this.enableMpegCheckbox.Margin = new System.Windows.Forms.Padding(2);
            this.enableMpegCheckbox.Name = "enableMpegCheckbox";
            this.enableMpegCheckbox.Size = new System.Drawing.Size(133, 17);
            this.enableMpegCheckbox.TabIndex = 12;
            this.enableMpegCheckbox.Text = "enableMpegCheckbox";
            this.enableMpegCheckbox.UseVisualStyleBackColor = true;
            this.enableMpegCheckbox.CheckedChanged += new System.EventHandler(this.EnableMpegChanged);
            // 
            // addProviderBox
            // 
            this.addProviderBox.FormattingEnabled = true;
            this.addProviderBox.Location = new System.Drawing.Point(269, 29);
            this.addProviderBox.Name = "addProviderBox";
            this.addProviderBox.Size = new System.Drawing.Size(120, 186);
            this.addProviderBox.TabIndex = 18;
            this.addProviderBox.SelectedIndexChanged += new System.EventHandler(this.OnAddProvider);
            // 
            // resetButton
            // 
            this.resetButton.Location = new System.Drawing.Point(269, 225);
            this.resetButton.Margin = new System.Windows.Forms.Padding(2);
            this.resetButton.Name = "resetButton";
            this.resetButton.Size = new System.Drawing.Size(271, 25);
            this.resetButton.TabIndex = 3;
            this.resetButton.Text = "resetButton";
            this.resetButton.UseVisualStyleBackColor = true;
            this.resetButton.Click += new System.EventHandler(this.ResetClicked);
            // 
            // removeProviderBox
            // 
            this.removeProviderBox.FormattingEnabled = true;
            this.removeProviderBox.Location = new System.Drawing.Point(420, 29);
            this.removeProviderBox.Name = "removeProviderBox";
            this.removeProviderBox.Size = new System.Drawing.Size(120, 186);
            this.removeProviderBox.TabIndex = 19;
            this.removeProviderBox.SelectedIndexChanged += new System.EventHandler(this.OnRemoveProvider);
            // 
            // removeProviderLabel
            // 
            this.removeProviderLabel.AutoSize = true;
            this.removeProviderLabel.Location = new System.Drawing.Point(417, 8);
            this.removeProviderLabel.Name = "removeProviderLabel";
            this.removeProviderLabel.Size = new System.Drawing.Size(107, 13);
            this.removeProviderLabel.TabIndex = 20;
            this.removeProviderLabel.Text = "removeProviderLabel";
            // 
            // addProviderLabel
            // 
            this.addProviderLabel.AutoSize = true;
            this.addProviderLabel.Location = new System.Drawing.Point(266, 8);
            this.addProviderLabel.Name = "addProviderLabel";
            this.addProviderLabel.Size = new System.Drawing.Size(90, 13);
            this.addProviderLabel.TabIndex = 21;
            this.addProviderLabel.Text = "addProviderLabel";
            // 
            // copyProviderBox
            // 
            this.copyProviderBox.AutoSize = true;
            this.copyProviderBox.Location = new System.Drawing.Point(117, 63);
            this.copyProviderBox.Name = "copyProviderBox";
            this.copyProviderBox.Size = new System.Drawing.Size(81, 13);
            this.copyProviderBox.TabIndex = 23;
            this.copyProviderBox.Text = "copyProviderBox";
            // 
            // urlToCopyBox
            // 
            this.urlToCopyBox.FormattingEnabled = true;
            this.urlToCopyBox.Location = new System.Drawing.Point(8, 59);
            this.urlToCopyBox.Margin = new System.Windows.Forms.Padding(2);
            this.urlToCopyBox.Name = "urlToCopyBox";
            this.urlToCopyBox.Size = new System.Drawing.Size(100, 21);
            this.urlToCopyBox.TabIndex = 22;
            this.urlToCopyBox.SelectedIndexChanged += new System.EventHandler(this.UrlToCopyChanged);
            // 
            // startupLoadCheckBox
            // 
            this.startupLoadCheckBox.AutoSize = true;
            this.startupLoadCheckBox.Location = new System.Drawing.Point(8, 233);
            this.startupLoadCheckBox.Margin = new System.Windows.Forms.Padding(2);
            this.startupLoadCheckBox.Name = "startupLoadCheckBox";
            this.startupLoadCheckBox.Size = new System.Drawing.Size(94, 17);
            this.startupLoadCheckBox.TabIndex = 24;
            this.startupLoadCheckBox.Text = "startupLoadCheckBox";
            this.startupLoadCheckBox.UseVisualStyleBackColor = true;
            this.startupLoadCheckBox.CheckedChanged += new System.EventHandler(this.OnLoadOnStartup);
            // 
            // ConfigForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(555, 259);
            this.Controls.Add(this.startupLoadCheckBox);
            this.Controls.Add(this.copyProviderBox);
            this.Controls.Add(this.urlToCopyBox);
            this.Controls.Add(this.addProviderLabel);
            this.Controls.Add(this.removeProviderLabel);
            this.Controls.Add(this.removeProviderBox);
            this.Controls.Add(this.addProviderBox);
            this.Controls.Add(this.enableMpegCheckbox);
            this.Controls.Add(this.manualScallingLabel);
            this.Controls.Add(this.manualScallingBox);
            this.Controls.Add(this.noTransparencyCheckBox);
            this.Controls.Add(this.noFocusCheckBox);
            this.Controls.Add(this.videoFpsLabel);
            this.Controls.Add(this.formatLabel);
            this.Controls.Add(this.noGifRepeatCheckBox);
            this.Controls.Add(this.videoFpsBox);
            this.Controls.Add(this.resetButton);
            this.Controls.Add(this.noScalingCheckBox);
            this.Controls.Add(this.noCopyCheckBox);
            this.Controls.Add(this.formatBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "ConfigForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "SharpCrop";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox formatBox;
        private System.Windows.Forms.CheckBox noCopyCheckBox;
        private System.Windows.Forms.CheckBox noScalingCheckBox;
        private System.Windows.Forms.ComboBox videoFpsBox;
        private System.Windows.Forms.CheckBox noGifRepeatCheckBox;
        private System.Windows.Forms.Label formatLabel;
        private System.Windows.Forms.Label videoFpsLabel;
        private System.Windows.Forms.CheckBox noFocusCheckBox;
        private System.Windows.Forms.CheckBox noTransparencyCheckBox;
        private System.Windows.Forms.TextBox manualScallingBox;
        private System.Windows.Forms.Label manualScallingLabel;
        private System.Windows.Forms.CheckBox enableMpegCheckbox;
        private System.Windows.Forms.ListBox addProviderBox;
        private System.Windows.Forms.Button resetButton;
        private System.Windows.Forms.ListBox removeProviderBox;
        private System.Windows.Forms.Label removeProviderLabel;
        private System.Windows.Forms.Label addProviderLabel;
        private System.Windows.Forms.Label copyProviderBox;
        private System.Windows.Forms.ComboBox urlToCopyBox;
        private System.Windows.Forms.CheckBox startupLoadCheckBox;
    }
}