using SharpCrop.Utils;
using System;
using System.Linq;
using System.Windows.Forms;

namespace SharpCrop.Forms
{
    /// <summary>
    /// ConfigForm is a UI for config modification.
    /// </summary>
    public partial class ConfigForm : Form
    {
        private readonly Controller controller;

        /// <summary>
        /// Construct a new ConfigForm and initialize it with ConfigHelper.
        /// </summary>
        public ConfigForm(Controller controller)
        {
            this.controller = controller;

            InitializeComponent();

            // Update add/remove provider list
            UpdateProviderList();

            // Init lists and boxes
            formatList.Text = ConfigHelper.Memory.FormatExt;
            videoFpsList.Text = ConfigHelper.Memory.SafeVideoFPS.ToString();
            manualScallingBox.Text = string.Join(" ", ConfigHelper.Memory.SafeManualScaling);

            // Init checkboxes
            noCopyCheckBox.Checked = ConfigHelper.Memory.NoCopy;
            noScalingCheckBox.Checked = ConfigHelper.Memory.NoAutoScaling;
            noGifRepeatCheckBox.Checked = ConfigHelper.Memory.NoGifRepeat;
            noFocusCheckBox.Checked = ConfigHelper.Memory.NoFocus;
            noTransparencyCheckBox.Checked = ConfigHelper.Memory.NoTransparency;
            enableMpegCheckbox.Checked = ConfigHelper.Memory.EnableMpeg;

#if __MonoCS__
            noFocusCheckBox.Enabled = false;
#endif
        }

        /// <summary>
        /// Update the add/remove provider list the reflect the current state.
        /// </summary>
        private void UpdateProviderList()
        {
            addProviderBox.Items.Clear();
            removeProviderBox.Items.Clear();

            controller.LoadedProviders.Keys
                .ToList()
                .ForEach(p => removeProviderBox.Items.Add(p));

            Constants.Providers.Keys
                .Where(p => !controller.LoadedProviders.Keys.Contains(p))
                .ToList()
                .ForEach(p => addProviderBox.Items.Add(p));
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
                ConfigHelper.Memory.VideoFPS = int.Parse(videoFpsList.Text);
            }
            catch
            {
                videoFpsList.Text = string.Empty;
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
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EnableMpegChanged(object sender, EventArgs e)
        {
            ConfigHelper.Memory.EnableMpeg = enableMpegCheckbox.Checked;
        }

        /// <summary>
        /// Unlink application from provider.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UnlinkClicked(object sender, EventArgs e)
        {
            ConfigHelper.Memory.Provider.Clear();
            Application.Exit();
        }

        /// <summary>
        /// Focus on MainForm when shown.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);

            Focus();
        }

        /// <summary>
        /// Update provider list on activation.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnActivated(EventArgs e)
        {
            base.OnActivated(e);

            UpdateProviderList();
        }

        /// <summary>
        /// Add new provider.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnAddProvider(object sender, EventArgs e)
        {
            if(addProviderBox.SelectedItem == null)
            {
                return;
            }
            
            controller.LoadProvider(addProviderBox.SelectedItem.ToString());
            addProviderBox.ClearSelected();
        }

        /// <summary>
        /// Remove existing provider.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnRemoveProvider(object sender, EventArgs e)
        {
            if (removeProviderBox.SelectedItem == null)
            {
                return;
            }

            controller.ClearProvider(removeProviderBox.SelectedItem.ToString());
            removeProviderBox.ClearSelected();
            UpdateProviderList();
        }
    }
}
