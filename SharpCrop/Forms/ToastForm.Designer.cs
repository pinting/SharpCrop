namespace SharpCrop.Forms
{
    partial class ToastForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ToastForm));
            this.Desc = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // Desc
            // 
            this.Desc.AutoSize = true;
            this.Desc.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.Desc.Location = new System.Drawing.Point(25, 25);
            this.Desc.Name = "Desc";
            this.Desc.Size = new System.Drawing.Size(112, 22);
            this.Desc.TabIndex = 0;
            this.Desc.Text = "Lorem ispum";
            this.Desc.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ToastForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(278, 0);
            this.ControlBox = false;
            this.Controls.Add(this.Desc);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(300, 56);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(300, 56);
            this.Name = "ToastForm";
            this.Opacity = 0.9D;
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "ToastForm";
            this.TopMost = true;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label Desc;
    }
}