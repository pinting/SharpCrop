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
    /// <summary>
    /// ConfigForm is a UI for config modification.
    /// </summary>
    public partial class ConfigForm : Form
    {
        /// <summary>
        /// Construct a new ConfigForm and initialize it with ConfigHelper.
        /// </summary>
        public ConfigForm()
        {
            InitializeComponent();

            formatList.Text = ConfigHelper.Memory.FormatExt;
            noCopyCheckBox.Checked = ConfigHelper.Memory.NoCopy;
            noScalingCheckBox.Checked = ConfigHelper.Memory.NoScaling;
        }

        /// <summary>
        /// Reset application settings.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ResetClick(object sender, EventArgs e)
        {
            ConfigHelper.Reset();
            Application.Exit();
        }

        /// <summary>
        /// Change image format.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormatChanged(object sender, EventArgs e)
        {
            ConfigHelper.Memory.Format = formatList.Text;
        }

        /// <summary>
        /// Disable or enable URL copy.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NoCopyChanged(object sender, EventArgs e)
        {
            ConfigHelper.Memory.NoCopy = noCopyCheckBox.Checked;
        }

        /// <summary>
        /// Disable or enable scaling checker.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NoScalingChanged(object sender, EventArgs e)
        {
            ConfigHelper.Memory.NoScaling = noScalingCheckBox.Checked;
        }
    }
}
