using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SharpCrop.Forms
{
    public partial class CopyForm : Form
    {
        public CopyForm(string url)
        {
            InitializeComponent();

            linkBox.Text = url;
        }
    }
}
