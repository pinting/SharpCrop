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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ConfigForm));
            this.formatBox = new System.Windows.Forms.ComboBox();
            this.noUrlCopyCheckBox = new System.Windows.Forms.CheckBox();
            this.videoFpsBox = new System.Windows.Forms.ComboBox();
            this.formatLabel = new System.Windows.Forms.Label();
            this.videoFpsLabel = new System.Windows.Forms.Label();
            this.noTransparencyCheckBox = new System.Windows.Forms.CheckBox();
            this.manualScallingBox = new System.Windows.Forms.TextBox();
            this.manualScallingLabel = new System.Windows.Forms.Label();
            this.enableMpegCheckbox = new System.Windows.Forms.CheckBox();
            this.addProviderBox = new System.Windows.Forms.ListBox();
            this.removeProviderBox = new System.Windows.Forms.ListBox();
            this.removeProviderLabel = new System.Windows.Forms.Label();
            this.addProviderLabel = new System.Windows.Forms.Label();
            this.copyProviderBox = new System.Windows.Forms.Label();
            this.urlToCopyBox = new System.Windows.Forms.ComboBox();
            this.startupLoadCheckBox = new System.Windows.Forms.CheckBox();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.updateLinkLabel = new System.Windows.Forms.LinkLabel();
            this.noImageCopyCheckBox = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // formatBox
            // 
            this.formatBox.FormattingEnabled = true;
            this.formatBox.Location = new System.Drawing.Point(30, 29);
            this.formatBox.Name = "formatBox";
            this.formatBox.Size = new System.Drawing.Size(148, 28);
            this.formatBox.TabIndex = 0;
            this.formatBox.SelectedIndexChanged += new System.EventHandler(this.FormatChanged);
            // 
            // noUrlCopyCheckBox
            // 
            this.noUrlCopyCheckBox.AutoSize = true;
            this.noUrlCopyCheckBox.Location = new System.Drawing.Point(30, 185);
            this.noUrlCopyCheckBox.Name = "noUrlCopyCheckBox";
            this.noUrlCopyCheckBox.Size = new System.Drawing.Size(174, 24);
            this.noUrlCopyCheckBox.TabIndex = 1;
            this.noUrlCopyCheckBox.Text = "noUrlCopyCheckBox";
            this.noUrlCopyCheckBox.UseVisualStyleBackColor = true;
            this.noUrlCopyCheckBox.CheckedChanged += new System.EventHandler(this.NoCopyChanged);
            // 
            // videoFpsBox
            // 
            this.videoFpsBox.FormattingEnabled = true;
            this.videoFpsBox.Location = new System.Drawing.Point(30, 68);
            this.videoFpsBox.Name = "videoFpsBox";
            this.videoFpsBox.Size = new System.Drawing.Size(148, 28);
            this.videoFpsBox.TabIndex = 4;
            this.videoFpsBox.SelectedIndexChanged += new System.EventHandler(this.GifFpsChanged);
            // 
            // formatLabel
            // 
            this.formatLabel.AutoSize = true;
            this.formatLabel.Location = new System.Drawing.Point(194, 37);
            this.formatLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.formatLabel.Name = "formatLabel";
            this.formatLabel.Size = new System.Drawing.Size(94, 20);
            this.formatLabel.TabIndex = 6;
            this.formatLabel.Text = "formatLabel";
            // 
            // videoFpsLabel
            // 
            this.videoFpsLabel.AutoSize = true;
            this.videoFpsLabel.Location = new System.Drawing.Point(194, 74);
            this.videoFpsLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.videoFpsLabel.Name = "videoFpsLabel";
            this.videoFpsLabel.Size = new System.Drawing.Size(112, 20);
            this.videoFpsLabel.TabIndex = 7;
            this.videoFpsLabel.Text = "videoFpsLabel";
            // 
            // noTransparencyCheckBox
            // 
            this.noTransparencyCheckBox.AutoSize = true;
            this.noTransparencyCheckBox.Location = new System.Drawing.Point(30, 245);
            this.noTransparencyCheckBox.Name = "noTransparencyCheckBox";
            this.noTransparencyCheckBox.Size = new System.Drawing.Size(214, 24);
            this.noTransparencyCheckBox.TabIndex = 9;
            this.noTransparencyCheckBox.Text = "noTransparencyCheckBox";
            this.noTransparencyCheckBox.UseVisualStyleBackColor = true;
            this.noTransparencyCheckBox.CheckedChanged += new System.EventHandler(this.NoTransparencyChanged);
            // 
            // manualScallingBox
            // 
            this.manualScallingBox.Location = new System.Drawing.Point(30, 149);
            this.manualScallingBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.manualScallingBox.Name = "manualScallingBox";
            this.manualScallingBox.Size = new System.Drawing.Size(148, 26);
            this.manualScallingBox.TabIndex = 10;
            this.manualScallingBox.TextChanged += new System.EventHandler(this.ManualScalingChanged);
            // 
            // manualScallingLabel
            // 
            this.manualScallingLabel.AutoSize = true;
            this.manualScallingLabel.Location = new System.Drawing.Point(194, 152);
            this.manualScallingLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.manualScallingLabel.Name = "manualScallingLabel";
            this.manualScallingLabel.Size = new System.Drawing.Size(155, 20);
            this.manualScallingLabel.TabIndex = 11;
            this.manualScallingLabel.Text = "manualScallingLabel";
            // 
            // enableMpegCheckbox
            // 
            this.enableMpegCheckbox.AutoSize = true;
            this.enableMpegCheckbox.Location = new System.Drawing.Point(30, 275);
            this.enableMpegCheckbox.Name = "enableMpegCheckbox";
            this.enableMpegCheckbox.Size = new System.Drawing.Size(186, 24);
            this.enableMpegCheckbox.TabIndex = 12;
            this.enableMpegCheckbox.Text = "enableMpegCheckbox";
            this.enableMpegCheckbox.UseVisualStyleBackColor = true;
            this.enableMpegCheckbox.CheckedChanged += new System.EventHandler(this.EnableMpegChanged);
            // 
            // addProviderBox
            // 
            this.addProviderBox.FormattingEnabled = true;
            this.addProviderBox.ItemHeight = 20;
            this.addProviderBox.Location = new System.Drawing.Point(422, 62);
            this.addProviderBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.addProviderBox.Name = "addProviderBox";
            this.addProviderBox.Size = new System.Drawing.Size(178, 284);
            this.addProviderBox.TabIndex = 18;
            this.addProviderBox.SelectedIndexChanged += new System.EventHandler(this.OnAddProvider);
            // 
            // removeProviderBox
            // 
            this.removeProviderBox.FormattingEnabled = true;
            this.removeProviderBox.ItemHeight = 20;
            this.removeProviderBox.Location = new System.Drawing.Point(648, 62);
            this.removeProviderBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.removeProviderBox.Name = "removeProviderBox";
            this.removeProviderBox.Size = new System.Drawing.Size(178, 284);
            this.removeProviderBox.TabIndex = 19;
            this.removeProviderBox.SelectedIndexChanged += new System.EventHandler(this.OnRemoveProvider);
            // 
            // removeProviderLabel
            // 
            this.removeProviderLabel.AutoSize = true;
            this.removeProviderLabel.Location = new System.Drawing.Point(644, 29);
            this.removeProviderLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.removeProviderLabel.Name = "removeProviderLabel";
            this.removeProviderLabel.Size = new System.Drawing.Size(157, 20);
            this.removeProviderLabel.TabIndex = 20;
            this.removeProviderLabel.Text = "removeProviderLabel";
            // 
            // addProviderLabel
            // 
            this.addProviderLabel.AutoSize = true;
            this.addProviderLabel.Location = new System.Drawing.Point(417, 29);
            this.addProviderLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.addProviderLabel.Name = "addProviderLabel";
            this.addProviderLabel.Size = new System.Drawing.Size(132, 20);
            this.addProviderLabel.TabIndex = 21;
            this.addProviderLabel.Text = "addProviderLabel";
            // 
            // copyProviderBox
            // 
            this.copyProviderBox.AutoSize = true;
            this.copyProviderBox.Location = new System.Drawing.Point(194, 114);
            this.copyProviderBox.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.copyProviderBox.Name = "copyProviderBox";
            this.copyProviderBox.Size = new System.Drawing.Size(126, 20);
            this.copyProviderBox.TabIndex = 23;
            this.copyProviderBox.Text = "copyProviderBox";
            // 
            // urlToCopyBox
            // 
            this.urlToCopyBox.FormattingEnabled = true;
            this.urlToCopyBox.Location = new System.Drawing.Point(30, 108);
            this.urlToCopyBox.Name = "urlToCopyBox";
            this.urlToCopyBox.Size = new System.Drawing.Size(148, 28);
            this.urlToCopyBox.TabIndex = 22;
            this.urlToCopyBox.SelectedIndexChanged += new System.EventHandler(this.UrlToCopyChanged);
            // 
            // startupLoadCheckBox
            // 
            this.startupLoadCheckBox.AutoSize = true;
            this.startupLoadCheckBox.Location = new System.Drawing.Point(30, 305);
            this.startupLoadCheckBox.Name = "startupLoadCheckBox";
            this.startupLoadCheckBox.Size = new System.Drawing.Size(186, 24);
            this.startupLoadCheckBox.TabIndex = 24;
            this.startupLoadCheckBox.Text = "startupLoadCheckBox";
            this.startupLoadCheckBox.UseVisualStyleBackColor = true;
            this.startupLoadCheckBox.CheckedChanged += new System.EventHandler(this.OnLoadOnStartup);
            // 
            // updateLinkLabel
            // 
            this.updateLinkLabel.AutoSize = true;
            this.updateLinkLabel.Location = new System.Drawing.Point(26, 337);
            this.updateLinkLabel.Name = "updateLinkLabel";
            this.updateLinkLabel.Size = new System.Drawing.Size(127, 20);
            this.updateLinkLabel.TabIndex = 25;
            this.updateLinkLabel.TabStop = true;
            this.updateLinkLabel.Text = "updateLinkLabel";
            this.updateLinkLabel.Visible = false;
            // 
            // noImageCopyCheckBox
            // 
            this.noImageCopyCheckBox.AutoSize = true;
            this.noImageCopyCheckBox.Location = new System.Drawing.Point(30, 215);
            this.noImageCopyCheckBox.Name = "noImageCopyCheckBox";
            this.noImageCopyCheckBox.Size = new System.Drawing.Size(199, 24);
            this.noImageCopyCheckBox.TabIndex = 26;
            this.noImageCopyCheckBox.Text = "noImageCopyCheckBox";
            this.noImageCopyCheckBox.UseVisualStyleBackColor = true;
            this.noImageCopyCheckBox.CheckedChanged += new System.EventHandler(this.NoImageCopyChanged);
            // 
            // ConfigForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(861, 366);
            this.Controls.Add(this.noImageCopyCheckBox);
            this.Controls.Add(this.updateLinkLabel);
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
            this.Controls.Add(this.videoFpsLabel);
            this.Controls.Add(this.formatLabel);
            this.Controls.Add(this.videoFpsBox);
            this.Controls.Add(this.noUrlCopyCheckBox);
            this.Controls.Add(this.formatBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ConfigForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "SharpCrop";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox formatBox;
        private System.Windows.Forms.CheckBox noUrlCopyCheckBox;
        private System.Windows.Forms.ComboBox videoFpsBox;
        private System.Windows.Forms.Label formatLabel;
        private System.Windows.Forms.Label videoFpsLabel;
        private System.Windows.Forms.CheckBox noTransparencyCheckBox;
        private System.Windows.Forms.TextBox manualScallingBox;
        private System.Windows.Forms.Label manualScallingLabel;
        private System.Windows.Forms.CheckBox enableMpegCheckbox;
        private System.Windows.Forms.ListBox addProviderBox;
        private System.Windows.Forms.ListBox removeProviderBox;
        private System.Windows.Forms.Label removeProviderLabel;
        private System.Windows.Forms.Label addProviderLabel;
        private System.Windows.Forms.Label copyProviderBox;
        private System.Windows.Forms.ComboBox urlToCopyBox;
        private System.Windows.Forms.CheckBox startupLoadCheckBox;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.LinkLabel updateLinkLabel;
        private System.Windows.Forms.CheckBox noImageCopyCheckBox;
    }
}