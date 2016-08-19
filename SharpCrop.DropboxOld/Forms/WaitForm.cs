using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SharpCrop.DropboxOld.Forms
{
    public partial class WaitForm : Form
    {
        string link;

        public WaitForm(string url)
        {
            InitializeComponent();

            linkBox.Text = url;
            link = url;
        }

        private void Clicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(link);
        }
    }
}
