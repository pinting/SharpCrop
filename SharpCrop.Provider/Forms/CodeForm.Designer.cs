namespace SharpCrop.Provider.Forms
{
    partial class CodeForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CodeForm));
            this.linkLabel = new System.Windows.Forms.LinkLabel();
            this.linkBox = new System.Windows.Forms.TextBox();
            this.stepOneLabel = new System.Windows.Forms.Label();
            this.codeBox = new System.Windows.Forms.TextBox();
            this.stepTwoLabel = new System.Windows.Forms.Label();
            this.helpLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // linkLabel
            // 
            this.linkLabel.AutoSize = true;
            this.linkLabel.Location = new System.Drawing.Point(4, 40);
            this.linkLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.linkLabel.Name = "linkLabel";
            this.linkLabel.Size = new System.Drawing.Size(150, 13);
            this.linkLabel.TabIndex = 3;
            this.linkLabel.TabStop = true;
            this.linkLabel.Text = "Please open the following link!";
            this.linkLabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.LinkClicked);
            // 
            // linkBox
            // 
            this.linkBox.Location = new System.Drawing.Point(5, 55);
            this.linkBox.Margin = new System.Windows.Forms.Padding(2);
            this.linkBox.Name = "linkBox";
            this.linkBox.ReadOnly = true;
            this.linkBox.Size = new System.Drawing.Size(280, 20);
            this.linkBox.TabIndex = 2;
            // 
            // stepOneLabel
            // 
            this.stepOneLabel.AutoSize = true;
            this.stepOneLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.stepOneLabel.Location = new System.Drawing.Point(2, 5);
            this.stepOneLabel.Name = "stepOneLabel";
            this.stepOneLabel.Size = new System.Drawing.Size(74, 25);
            this.stepOneLabel.TabIndex = 6;
            this.stepOneLabel.Text = "Step 1";
            this.stepOneLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // codeBox
            // 
            this.codeBox.Location = new System.Drawing.Point(5, 141);
            this.codeBox.Name = "codeBox";
            this.codeBox.Size = new System.Drawing.Size(280, 20);
            this.codeBox.TabIndex = 5;
            this.codeBox.TextChanged += new System.EventHandler(this.CodeBoxChanged);
            // 
            // stepTwoLabel
            // 
            this.stepTwoLabel.AutoSize = true;
            this.stepTwoLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.stepTwoLabel.Location = new System.Drawing.Point(2, 90);
            this.stepTwoLabel.Name = "stepTwoLabel";
            this.stepTwoLabel.Size = new System.Drawing.Size(74, 25);
            this.stepTwoLabel.TabIndex = 7;
            this.stepTwoLabel.Text = "Step 2";
            this.stepTwoLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // helpLabel
            // 
            this.helpLabel.AutoSize = true;
            this.helpLabel.Location = new System.Drawing.Point(4, 125);
            this.helpLabel.Name = "helpLabel";
            this.helpLabel.Size = new System.Drawing.Size(278, 13);
            this.helpLabel.TabIndex = 4;
            this.helpLabel.Text = "Copy the given code bellow to give access to SharpCrop!";
            // 
            // CodeForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(291, 166);
            this.Controls.Add(this.stepTwoLabel);
            this.Controls.Add(this.stepOneLabel);
            this.Controls.Add(this.codeBox);
            this.Controls.Add(this.helpLabel);
            this.Controls.Add(this.linkLabel);
            this.Controls.Add(this.linkBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "CodeForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "SharpCrop";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.LinkLabel linkLabel;
        private System.Windows.Forms.TextBox linkBox;
        private System.Windows.Forms.Label helpLabel;
        private System.Windows.Forms.Label stepTwoLabel;
        private System.Windows.Forms.TextBox codeBox;
        private System.Windows.Forms.Label stepOneLabel;
    }
}