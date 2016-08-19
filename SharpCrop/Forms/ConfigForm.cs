using SharpCrop.Utils;
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
    public partial class ConfigForm : Form
    {
        public ConfigForm()
        {
            InitializeComponent();

            formatList.Text = ConfigHelper.Memory.FormatExt;
            noCopyCheckBox.Checked = ConfigHelper.Memory.NoCopy;
            noScalingCheckBox.Checked = ConfigHelper.Memory.NoScaling;
        }

        private void ResetClick(object sender, EventArgs e)
        {
            ConfigHelper.Reset();
            Application.Exit();
        }

        private void FormatChanged(object sender, EventArgs e)
        {
            ConfigHelper.Memory.Format = formatList.Text;
        }

        private void NoCopyChanged(object sender, EventArgs e)
        {
            ConfigHelper.Memory.NoCopy = noCopyCheckBox.Checked;
        }

        private void NoScalingChanged(object sender, EventArgs e)
        {
            ConfigHelper.Memory.NoScaling = noScalingCheckBox.Checked;
        }
    }
}
