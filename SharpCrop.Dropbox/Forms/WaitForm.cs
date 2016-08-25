using System;
using System.Windows.Forms;

namespace SharpCrop.Dropbox.Forms
{
    public partial class WaitForm : Form
    {
        private Action<string> onCode;

        public WaitForm(string url)
        {
            InitializeComponent();

            linkBox.Text = url;
        }

        public void OnCode(Action<string> onCode)
        {
            this.onCode = onCode;
        }

        private void CodeBoxChanged(object sender, EventArgs e)
        {
            if(codeBox.Text.Length == 43)
            {
                onCode(codeBox.Text);
            }
        }
    }
}
