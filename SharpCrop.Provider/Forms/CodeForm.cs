using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using SharpCrop.Provider.Properties;

namespace SharpCrop.Provider.Forms
{
    /// <summary>
    /// CodeForm shows the token request link and waits for the user 
    /// to copy the API code into the input box.
    /// </summary>
    public class CodeForm : Form
    {
        private LinkLabel linkLabel;
        private TextBox linkBox;
        private Label helpLabel;
        private Label stepTwoLabel;
        private TextBox codeBox;
        private Label stepOneLabel;
        
        private readonly int length;

        /// <summary>
        /// Executed when the required code is pasted.
        /// </summary>
        public event Action<string> OnResult;

        /// <summary>
        /// Construct a new CodeForm with an URL and a required code length.
        /// </summary>
        /// <param name="url"></param>
        /// <param name="length">Required length of the activation code.</param>
        public CodeForm(string url = "", int length = 128)
        {
            InitializeComponent(url);
            
            this.length = length;
        }
        
        private void InitializeComponent(string link)
        {
            SuspendLayout();
            
            var font = new Font("Sans Serif", 15.75F, FontStyle.Regular, GraphicsUnit.Point, 238);
            
            // Link Label
            linkLabel = new LinkLabel();
            linkLabel.AutoSize = true;
            linkLabel.Location = new Point(5, 40);
            linkLabel.Size = new Size(50, 15);
            linkLabel.TabIndex = 3;
            linkLabel.TabStop = true;
            linkLabel.Text = Resources.CodeLink;
            linkLabel.LinkClicked += LinkClicked;
            
            // Link box
            linkBox = new TextBox();
            linkBox.Location = new Point(5, 55);
            linkBox.ReadOnly = true;
            linkBox.Size = new Size(280, 20);
            linkBox.TabIndex = 2;
            linkBox.Text = link;
            
            // Step 1 label
            stepOneLabel = new Label();
            stepOneLabel.AutoSize = true;
            stepOneLabel.Location = new Point(5, 5);
            stepOneLabel.Size = new Size(215, 40);
            stepOneLabel.TabIndex = 6;
            stepOneLabel.Text = Resources.CodeStepOne;
            stepOneLabel.TextAlign = ContentAlignment.MiddleCenter;
            stepOneLabel.Font = font;
            
            // Code box
            codeBox = new TextBox();
            codeBox.Location = new Point(5, 140);
            codeBox.Size = new Size(280, 20);
            codeBox.TabIndex = 5;
            codeBox.TextChanged += CodeBoxChanged;
            
            // Step 2 label
            stepTwoLabel = new Label();
            stepTwoLabel.AutoSize = true;
            stepTwoLabel.Location = new Point(5, 90);
            stepTwoLabel.Size = new Size(215, 35);
            stepTwoLabel.TabIndex = 7;
            stepTwoLabel.Text = Resources.CodeStepTwo;
            stepTwoLabel.TextAlign = ContentAlignment.MiddleCenter;
            stepTwoLabel.Font = font;
            
            // Help label
            helpLabel = new Label();
            helpLabel.AutoSize = true;
            helpLabel.Location = new Point(5, 125);
            helpLabel.Size = new Size(55, 15);
            helpLabel.TabIndex = 4;
            helpLabel.Text = Resources.CodeHelp;
            
            AutoScaleDimensions = new SizeF(6F, 13F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(295, 170);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "SharpCrop";

            Controls.Add(stepTwoLabel);
            Controls.Add(stepOneLabel);
            Controls.Add(codeBox);
            Controls.Add(helpLabel);
            Controls.Add(linkLabel);
            Controls.Add(linkBox);
            
            ResumeLayout(false);
            PerformLayout();
        }

        /// <summary>
        /// Call the registered callback when the required code length is reached.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CodeBoxChanged(object sender, EventArgs e)
        {
            if(codeBox.Text.Length >= length)
            {
                OnResult?.Invoke(codeBox.Text);
            }
        }

        /// <summary>
        /// Open the browser when the link was clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(linkBox.Text);
        }
    }
}
