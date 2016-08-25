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
            this.SuspendLayout();
            // 
            // formatList
            // 
            this.formatList.FormattingEnabled = true;
            this.formatList.Items.AddRange(new object[] {
            "jpg",
            "png",
            "bmp"});
            this.formatList.Location = new System.Drawing.Point(12, 12);
            this.formatList.Name = "formatList";
            this.formatList.Size = new System.Drawing.Size(197, 28);
            this.formatList.TabIndex = 0;
            this.formatList.Text = "Format";
            this.formatList.SelectedIndexChanged += new System.EventHandler(this.FormatChanged);
            // 
            // copyCheckBox
            // 
            this.noCopyCheckBox.AutoSize = true;
            this.noCopyCheckBox.Location = new System.Drawing.Point(12, 46);
            this.noCopyCheckBox.Name = "copyCheckBox";
            this.noCopyCheckBox.Size = new System.Drawing.Size(125, 24);
            this.noCopyCheckBox.TabIndex = 1;
            this.noCopyCheckBox.Text = "Disable copy";
            this.noCopyCheckBox.UseVisualStyleBackColor = true;
            this.noCopyCheckBox.CheckedChanged += new System.EventHandler(this.NoCopyChanged);
            // 
            // scalingCheckBox
            // 
            this.noScalingCheckBox.AutoSize = true;
            this.noScalingCheckBox.Location = new System.Drawing.Point(12, 76);
            this.noScalingCheckBox.Name = "scalingCheckBox";
            this.noScalingCheckBox.Size = new System.Drawing.Size(141, 24);
            this.noScalingCheckBox.TabIndex = 2;
            this.noScalingCheckBox.Text = "Disable scaling";
            this.noScalingCheckBox.UseVisualStyleBackColor = true;
            this.noScalingCheckBox.CheckedChanged += new System.EventHandler(this.NoScalingChanged);
            // 
            // resetButton
            // 
            this.resetButton.Location = new System.Drawing.Point(12, 106);
            this.resetButton.Name = "resetButton";
            this.resetButton.Size = new System.Drawing.Size(197, 38);
            this.resetButton.TabIndex = 3;
            this.resetButton.Text = "Reset";
            this.resetButton.UseVisualStyleBackColor = true;
            this.resetButton.Click += new System.EventHandler(this.ResetClick);
            // 
            // ConfigForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(217, 155);
            this.Controls.Add(this.resetButton);
            this.Controls.Add(this.noScalingCheckBox);
            this.Controls.Add(this.noCopyCheckBox);
            this.Controls.Add(this.formatList);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
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
    }
}