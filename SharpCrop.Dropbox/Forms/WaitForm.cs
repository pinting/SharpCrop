using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SharpCrop.Dropbox.Forms
{
    public partial class WaitForm : Form
    {
        string link;

        public WaitForm()
        {
            InitializeComponent();
        }

        public void SetLink(string url)
        {
            linkBox.Text = url;
            link = url;
        }

        private void Clicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(link);
        }
    }
}
