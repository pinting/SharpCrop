using System;
using System.Linq;
using System.Windows.Forms;
using SharpCrop.Utils;
using SharpCrop.Properties;

// ReSharper disable CoVariantArrayConversion

namespace SharpCrop.Forms
{
    /// <summary>
    /// ConfigForm is a UI for config modification.
    /// </summary>
    public sealed partial class ConfigForm : Form
    {
        private readonly Controller controller;

        /// <summary>
        /// Construct a new ConfigForm and initialize it with ConfigHelper.
        /// </summary>
        public ConfigForm(Controller controller)
        {
            this.controller = controller;

            InitializeComponent();

            // Init texts from Resources
            noGifRepeatCheckBox.Text = Resources.ConfigNoGifRepeat;
            startupLoadCheckBox.Text = Resources.ConfigStartupLoad;
            noTransparencyCheckBox.Text = Resources.ConfigNoTransparency;
            enableMpegCheckbox.Text = Resources.ConfigEnableMpeg;
            noFocusCheckBox.Text = Resources.ConfigNoFocus;
            noScalingCheckBox.Text = Resources.ConfigNoScaling;
            copyProviderBox.Text = Resources.ConfigCopyProvider;
            noCopyCheckBox.Text = Resources.ConfigNoCopy;
            addProviderLabel.Text = Resources.ConfigAddProvider;
            removeProviderLabel.Text = Resources.ConfigRemoveProvider;
            manualScallingLabel.Text = Resources.ConfigManualScalling;
            videoFpsLabel.Text = Resources.ConfigVideoFps;
            formatLabel.Text = Resources.ConfigFormat;
            resetButton.Text = Resources.ConfigReset;
            Text = Resources.AppName;

            // Init lists and boxes
            formatBox.Text = ConfigHelper.Current.SafeImageExt;
            formatBox.Items.AddRange(Constants.ImageFormats.Keys.ToArray());
            videoFpsBox.Text = ConfigHelper.Current.SafeVideoFps.ToString();
            videoFpsBox.Items.AddRange(Constants.FpsList.ToArray());
            urlToCopyBox.Text = ConfigHelper.Current.CopyProvider;
            manualScallingBox.Text = string.Join(" ", ConfigHelper.Current.SafeManualScaling);
            addProviderBox.Items.AddRange(Constants.Providers.Keys.ToArray());

            // Init checkboxes from settings
            noCopyCheckBox.Checked = ConfigHelper.Current.NoCopy;
            noScalingCheckBox.Checked = ConfigHelper.Current.NoAutoScaling;
            noGifRepeatCheckBox.Checked = ConfigHelper.Current.NoGifRepeat;
            noFocusCheckBox.Checked = ConfigHelper.Current.NoFocus;
            noTransparencyCheckBox.Checked = ConfigHelper.Current.NoTransparency;
            enableMpegCheckbox.Checked = ConfigHelper.Current.EnableMpeg;
            startupLoadCheckBox.Checked = ConfigHelper.Current.StartupLoad;

#if __MonoCS__
            noFocusCheckBox.Enabled = false;
#endif

            // Update provider list and register an update event
            UpdateProviderList();
            urlToCopyBox.MouseEnter += (s, e) => UpdateProviderList();
            removeProviderBox.MouseEnter += (s, e) => UpdateProviderList();

        }
        
        /// <summary>
        /// Change image format.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormatChanged(object sender, EventArgs e)
        {
            ConfigHelper.Current.ImageExt = formatBox.Text;
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
                ConfigHelper.Current.VideoFps = int.Parse(videoFpsBox.Text);
            }
            catch
            {
                videoFpsBox.Text = string.Empty;
            }
        }

        /// <summary>
        /// Change URL to copy value.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UrlToCopyChanged(object sender, EventArgs e)
        {
            ConfigHelper.Current.CopyProvider = urlToCopyBox.Text;
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
                    .Select(int.Parse)
                    .ToList();

                ConfigHelper.Current.ManualScaling = list;
            }
            catch
            {
                // Ignored
            }
        }

        /// <summary>
        /// Disable or enable scaling checker.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NoScalingChanged(object sender, EventArgs e)
        {
            ConfigHelper.Current.NoAutoScaling = noScalingCheckBox.Checked;
        }

        /// <summary>
        /// Disable or enable URL copy.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NoCopyChanged(object sender, EventArgs e)
        {
            ConfigHelper.Current.NoCopy = noCopyCheckBox.Checked;
        }

        /// <summary>
        /// Disable or enable GIF repeat.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NoGifRepeatChanged(object sender, EventArgs e)
        {
            ConfigHelper.Current.NoGifRepeat = noGifRepeatCheckBox.Checked;
        }

        /// <summary>
        /// Disable or enable focus on launch.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NoFocusChanged(object sender, EventArgs e)
        {
            ConfigHelper.Current.NoFocus = noFocusCheckBox.Checked;
        }

        /// <summary>
        /// Disable or enable transparency.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NoTransparencyChanged(object sender, EventArgs e)
        {
            ConfigHelper.Current.NoTransparency = noTransparencyCheckBox.Checked;
        }

        /// <summary>
        /// Disable or enable mpeq recording.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EnableMpegChanged(object sender, EventArgs e)
        {
            ConfigHelper.Current.EnableMpeg = enableMpegCheckbox.Checked;
        }

        /// <summary>
        /// Disable or enable the load of the providers on startup.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnLoadOnStartup(object sender, EventArgs e)
        {
            ConfigHelper.Current.StartupLoad = startupLoadCheckBox.Checked;
        }

        /// <summary>
        /// Reset application settings.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ResetClicked(object sender, EventArgs e)
        {
            ConfigHelper.Reset();
            Application.Exit();
        }

        /// <summary>
        /// Update the loaded providers lists.
        /// </summary>
        private void UpdateProviderList()
        {
            urlToCopyBox.Items.Clear();
            removeProviderBox.Items.Clear();
            urlToCopyBox.Items.AddRange(controller.LoadedProviders.Keys.ToArray());
            removeProviderBox.Items.AddRange(controller.LoadedProviders.Keys.ToArray());
        }

        /// <summary>
        /// Add new provider.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void OnAddProvider(object sender, EventArgs e)
        {
            if(addProviderBox.SelectedItem == null)
            {
                return;
            }

            var number = (new Random().Next(1000, 9999)).ToString();
            var name = addProviderBox.SelectedItem.ToString();

            await controller.LoadProvider($"{name}{number}", name);

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

            var name = removeProviderBox.SelectedItem.ToString();

            controller.ClearProvider(name);
            removeProviderBox.ClearSelected();
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
    }
}
