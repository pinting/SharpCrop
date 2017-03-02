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
            startupLoadCheckBox.Text = Resources.ConfigStartupLoad;
            noTransparencyCheckBox.Text = Resources.ConfigNoTransparency;
            enableMpegCheckbox.Text = Resources.ConfigEnableMpeg;
            noScalingCheckBox.Text = Resources.ConfigNoScaling;
            copyProviderBox.Text = Resources.ConfigCopyProvider;
            noCopyCheckBox.Text = Resources.ConfigNoCopy;
            addProviderLabel.Text = Resources.ConfigAddProvider;
            removeProviderLabel.Text = Resources.ConfigRemoveProvider;
            manualScallingLabel.Text = Resources.ConfigManualScalling;
            videoFpsLabel.Text = Resources.ConfigVideoFps;
            formatLabel.Text = Resources.ConfigFormat;
            resetButton.Text = Resources.ConfigReset;

            // Init lists and boxes
            formatBox.Items.AddRange(Constants.ImageFormats.Keys.ToArray());
            videoFpsBox.Items.AddRange(Constants.FpsList.ToArray());
            manualScallingBox.Text = string.Join(" ", ConfigHelper.Current.SafeManualScaling);
            videoFpsBox.Text = ConfigHelper.Current.SafeVideoFps.ToString();
            urlToCopyBox.Text = ConfigHelper.Current.CopyProvider;
            formatBox.Text = ConfigHelper.Current.SafeImageExt;

            // Init checkboxes
            noCopyCheckBox.Checked = ConfigHelper.Current.NoCopy;
            noScalingCheckBox.Checked = ConfigHelper.Current.NoAutoScaling;
            noTransparencyCheckBox.Checked = ConfigHelper.Current.NoTransparency;
            enableMpegCheckbox.Checked = ConfigHelper.Current.EnableMpeg;
            startupLoadCheckBox.Checked = ConfigHelper.Current.StartupRegister;

#if __MonoCS__
            noFocusCheckBox.Enabled = false;
#endif

            // Update provider list and register an update event
            UpdateProviderList();
            urlToCopyBox.MouseEnter += (s, e) => UpdateProviderList();
            addProviderBox.MouseEnter += (s, e) => UpdateProviderList();
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
            ConfigHelper.Current.StartupRegister = startupLoadCheckBox.Checked;
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
        /// Update the providers lists.
        /// </summary>
        private void UpdateProviderList()
        {
            // Clear existing items
            urlToCopyBox.Items.Clear();
            removeProviderBox.Items.Clear();
            addProviderBox.Items.Clear();

            // Add them from the controller
            urlToCopyBox.Items.AddRange(controller.RegisteredProviders.Keys.ToArray());
            removeProviderBox.Items.AddRange(controller.RegisteredProviders.Keys.ToArray());
            addProviderBox.Items.AddRange(controller.LoadedProviders.Select(p => p.Name).ToArray());
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

            var number = (new Random().Next(100, 999)).ToString();
            var name = addProviderBox.SelectedItem.ToString();
            var provider = controller.GetProviderByName(name);

            await controller.RegisterProvider(provider, $"{name} {number}");

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
