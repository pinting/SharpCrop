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
            gifFpsList.Text = ConfigHelper.Memory.SafeGifFps.ToString();

            noCopyCheckBox.Checked = ConfigHelper.Memory.NoCopy;
            noScalingCheckBox.Checked = ConfigHelper.Memory.NoScaling;
            noGifRepeatCheckBox.Checked = ConfigHelper.Memory.NoGifRepeat;
            noFocusCheckBox.Checked = ConfigHelper.Memory.NoFocus;
            noTransparencyCheckBox.Checked = ConfigHelper.Memory.NoTransparency;

#if __MonoCS__
            noFocusCheckBox.Enabled = false;
#endif
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
        /// Change GIF FPS value.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GifFpsChanged(object sender, EventArgs e)
        {
            ConfigHelper.Memory.GifFps = int.Parse(gifFpsList.Text);
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

        /// <summary>
        /// Disable or enable GIF repeat.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NoGifRepeatChanged(object sender, EventArgs e)
        {
            ConfigHelper.Memory.NoGifRepeat = noGifRepeatCheckBox.Checked;
        }

        /// <summary>
        /// Disable or enable focus on launch.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NoFocusChanged(object sender, EventArgs e)
        {
            ConfigHelper.Memory.NoFocus = noFocusCheckBox.Checked;
        }

        private void NoTransparencyChanged(object sender, EventArgs e)
        {
            ConfigHelper.Memory.NoTransparency = noTransparencyCheckBox.Checked;
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
    }
}
