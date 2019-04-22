using System;
using System.Drawing;
using System.Windows.Forms;
using SharpCrop.Provider.Properties;

namespace SharpCrop.Provider.Forms
{
    /// <summary>
    /// FolderForm is used to help the user select a directory.
    /// </summary>
    public class FolderForm : Form
    {
        private FolderBrowserDialog folderBrowserDialog;
        private Button browseButton;
        private TextBox folderBox;
        private Button submitButton;
        
        /// <summary>
        /// Executed when the required code is pasted.
        /// </summary>
        public event Action<string> OnResult;

        /// <summary>
        /// Init form.
        /// </summary>
        public FolderForm()
        {
            InitializeComponent();
        }
        
        private void InitializeComponent()
        {
            SuspendLayout();
            
            // Browse button
            browseButton = new Button();
            browseButton.Location = new Point(420, 20);
            browseButton.Size = new Size(110, 30);
            browseButton.TabIndex = 0;
            browseButton.UseVisualStyleBackColor = true;
            browseButton.Click += OnBrowse;
            browseButton.Text = Resources.FolderBrowse;
            
            // Folder box
            folderBox = new TextBox();
            folderBox.Location = new Point(20, 20);
            folderBox.Size = new Size(390, 20);
            folderBox.TabIndex = 1;
            
            // Submit button
            submitButton = new Button();
            submitButton.Location = new Point(540, 20);
            submitButton.Size = new Size(110, 30);
            submitButton.TabIndex = 2;
            submitButton.UseVisualStyleBackColor = true;
            submitButton.Click += OnSubmit;
            submitButton.Text = Resources.FolderSubmit;
            
            // Folder browser dialog
            folderBrowserDialog = new FolderBrowserDialog();
            
            AutoScaleMode = AutoScaleMode.Dpi;
            AutoScaleDimensions = new SizeF(144F, 144F);
            ClientSize = new Size(660, 70);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "SharpCrop";
            
            Controls.Add(submitButton);
            Controls.Add(folderBox);
            Controls.Add(browseButton);
            
            ResumeLayout(false);
            PerformLayout();
        }

        /// <summary>
        /// Start OS specific folder browser when the browse button is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnBrowse(object sender, EventArgs e)
        {
            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                folderBox.Text = folderBrowserDialog.SelectedPath;
            }
        }

        /// <summary>
        /// Call the callback with the result.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnSubmit(object sender, EventArgs e)
        {
            OnResult?.Invoke(folderBox.Text);
        }
    }
}
