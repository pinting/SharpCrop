using System;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;
using SharpCrop.Modules;
using SharpCrop.Properties;

// ReSharper disable LocalizableElement
// ReSharper disable CoVariantArrayConversion

namespace SharpCrop.Forms
{
    /// <summary>
    /// ConfigForm is a UI for config modification.
    /// </summary>
    public sealed partial class ConfigForm : Form
    {
        /// <summary>
        /// Construct a new ConfigForm and initialize it with ConfigHelper.
        /// </summary>
        public ConfigForm()
        {
            InitializeComponent();

            // Init texts from Resources
            startupLoadCheckBox.Text = Resources.ConfigStartupLoad;
            noTransparencyCheckBox.Text = Resources.ConfigNoTransparency;
            enableMpegCheckbox.Text = Resources.ConfigEnableMpeg;
            copyProviderBox.Text = Resources.ConfigCopyProvider;
            noUrlCopyCheckBox.Text = Resources.ConfigNoCopy;
            addProviderLabel.Text = Resources.ConfigAddProvider;
            removeProviderLabel.Text = Resources.ConfigRemoveProvider;
            manualScallingLabel.Text = Resources.ConfigManualScalling;
            updateLinkLabel.Text = Resources.ConfigUpdateLink;
            videoFpsLabel.Text = Resources.ConfigVideoFps;
            formatLabel.Text = Resources.ConfigFormat;
            noImageCopyCheckBox.Text = Resources.ConfigNoImageCopy;

            // Init checkboxes
            noUrlCopyCheckBox.Checked = ConfigHelper.Current.NoUrlCopy;
            noTransparencyCheckBox.Checked = ConfigHelper.Current.NoTransparency;
            enableMpegCheckbox.Checked = ConfigHelper.Current.EnableMpeg;
            startupLoadCheckBox.Checked = ConfigHelper.Current.StartupRegister;
            noImageCopyCheckBox.Checked = ConfigHelper.Current.NoImageCopy;

            // Init lists and boxes
            formatBox.Items.AddRange(Constants.ImageFormats.Keys.ToArray());
            videoFpsBox.Items.AddRange(Constants.FpsList.ToArray());
            videoFpsBox.Text = ConfigHelper.Current.SafeVideoFps.ToString();
            urlToCopyBox.Text = ConfigHelper.Current.CopyProvider;
            formatBox.Text = ConfigHelper.Current.SafeImageExt;
            manualScallingBox.Text = string.Join(" ", ConfigHelper.Current.SafeManualScaling);

            // Set tooltips
            toolTip.SetToolTip(manualScallingBox, Resources.ConfigManualScallingHelp);

            // Show welcome message if this is the first launch of the app
            if (!ConfigHelper.Current.NoWelcome)
            {
                MessageBox.Show(Resources.WelcomeMessage, "SharpCrop");
                ConfigHelper.Current.NoWelcome = true;
            }

            // Init version checker
            var url = VersionHelper.GetLatestLink();

            if (url != null)
            {
                updateLinkLabel.LinkClicked += (s, e) => Process.Start(url);
                updateLinkLabel.Show();
            }

            // Register an update event and update provider list
            ProviderManager.RegisteredProvidersChanged += UpdateProviderList;
            ProviderManager.LoadedProvidersChanged += UpdateProviderList;
            UpdateProviderList();
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
        /// Disable or enable URL copy.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NoCopyChanged(object sender, EventArgs e)
        {
            ConfigHelper.Current.NoUrlCopy = noUrlCopyCheckBox.Checked;
        }

        /// <summary>
        /// Change image copy value.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NoImageCopyChanged(object sender, EventArgs e)
        {
            ConfigHelper.Current.NoImageCopy = noImageCopyCheckBox.Checked;
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
        /// Update the providers lists.
        /// </summary>
        private void UpdateProviderList()
        {
            // Clear existing items
            urlToCopyBox.Items.Clear();
            removeProviderBox.Items.Clear();
            addProviderBox.Items.Clear();

            // Add them from the controller
            urlToCopyBox.Items.AddRange(ProviderManager.RegisteredProviders.Keys.ToArray());
            removeProviderBox.Items.AddRange(ProviderManager.RegisteredProviders.Keys.ToArray());
            addProviderBox.Items.AddRange(ProviderManager.LoadedProviders.Select(p => p.Name).ToArray());
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
            var provider = ProviderManager.GetProviderByName(name);

            await ProviderManager.RegisterProvider(provider, $"{name} {number}");

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

            ProviderManager.ClearProvider(name);
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
