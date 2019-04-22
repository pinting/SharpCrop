using System.Drawing;
using System.Windows.Forms;

namespace SharpCrop.Forms
{
    /// <summary>
    /// A simple form which shows a URL to the user. This is needed, because Clipboard is
    /// not working in Mono - so the user must copy the output manually.
    /// </summary>
    public class CopyForm : Form
    {
        private TextBox linkBox;
        
        public CopyForm(string url)
        {
            InitializeComponent();

            linkBox.Text = url;
        }
        
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            SuspendLayout();
            
            linkBox = new TextBox();
            linkBox.Location = new Point(10, 10);
            linkBox.Size = new Size(600, 25);
            linkBox.TabIndex = 0;
            
            AutoScaleDimensions = new SizeF(144F, 144F);
            AutoScaleMode = AutoScaleMode.Dpi;
            ClientSize = new Size(630, 50);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "SharpCrop";
            TopMost = true;
            
            Controls.Add(linkBox);
            ResumeLayout(false);
            PerformLayout();

        }
    }
}
