using SharpCrop.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
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

            // Init lists and boxes
            formatList.Text = ConfigHelper.Memory.FormatExt;
            gifFpsList.Text = ConfigHelper.Memory.SafeGifSpeed.ToString();
            manualScallingBox.Text = ConfigHelper.Memory.SafeManualScaling.Select(s => s.ToString()).Aggregate((a, b) => $"{a} {b}");

            // Init checkboxes
            noCopyCheckBox.Checked = ConfigHelper.Memory.NoCopy;
            noScalingCheckBox.Checked = ConfigHelper.Memory.NoAutoScaling;
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
            try
            {
                ConfigHelper.Memory.GifSpeed = int.Parse(gifFpsList.Text);
            }
            catch
            {
                gifFpsList.Text = string.Empty;
            }
        }

        /// <summary>
        /// Change manual scaling list.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ManualScalingChanged(object sender, EventArgs e)
        {
            try
            {
                var list = manualScallingBox.Text
                    .Split(' ')
                    .Select(s => int.Parse(s))
                    .ToList();

                ConfigHelper.Memory.ManualScaling = list;
            }
            catch
            {
                return;
            }
        }

        /// <summary>
        /// Disable or enable scaling checker.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NoScalingChanged(object sender, EventArgs e)
        {
            ConfigHelper.Memory.NoAutoScaling = noScalingCheckBox.Checked;
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

        /// <summary>
        /// Disable on enable transparency.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NoTransparencyChanged(object sender, EventArgs e)
        {
            ConfigHelper.Memory.NoTransparency = noTransparencyCheckBox.Checked;
        }

        /// <summary>
        /// Unlink application from provider.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UnlinkClick(object sender, EventArgs e)
        {
            ConfigHelper.Memory.Provider = null;
            ConfigHelper.Memory.Token = null;

            Application.Exit();
        }
    }
}
