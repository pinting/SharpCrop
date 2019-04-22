using System;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using SharpCrop.Properties;
using SharpCrop.Services;

// ReSharper disable LocalizableElement
// ReSharper disable CoVariantArrayConversion

namespace SharpCrop.Forms
{
    /// <summary>
    /// ConfigForm is a UI for config modification.
    /// </summary>
    public sealed class ConfigForm : Form
    {
        private ComboBox formatBox;
        private CheckBox noUrlCopyCheckBox;
        private ComboBox videoFpsBox;
        private Label formatLabel;
        private Label videoFpsLabel;
        private CheckBox noTransparencyCheckBox;
        private ListBox addProviderBox;
        private ListBox removeProviderBox;
        private Label removeProviderLabel;
        private Label addProviderLabel;
        private Label copyProviderBox;
        private ComboBox urlToCopyBox;
        private LinkLabel updateLinkLabel;
        private CheckBox noImageCopyCheckBox;
        
        /// <summary>
        /// Construct a new ConfigForm and initialize it with ConfigHelper.
        /// </summary>
        public ConfigForm()
        {
            InitializeComponent();

            // Show welcome message if this is the first launch of the app
            if (!SettingsService.Current.NoWelcome)
            {
                MessageBox.Show(Resources.WelcomeMessage, "SharpCrop");
                SettingsService.Current.NoWelcome = true;
            }

            // Init version checker
            var url = VersionService.GetLatestLink();

            if (url != null)
            {
                updateLinkLabel.LinkClicked += (s, e) => Process.Start(url);
                updateLinkLabel.Show();
            }

            // Register an update event and update provider list
            ProviderService.RegisteredProvidersChanged += UpdateProviderList;
            ProviderService.LoadedProvidersChanged += UpdateProviderList;
            UpdateProviderList();
        }
        
        private void InitializeComponent()
        {
            SuspendLayout();

            // Format label
            formatLabel = new Label();
            formatLabel.Location = new Point(195, 35);
            formatLabel.Size = new Size(95, 20);
            formatLabel.AutoSize = true;
            formatLabel.TabIndex = 6;
            formatLabel.Text = Resources.ConfigFormat;
            
            // Image format box
            formatBox = new ComboBox();
            formatBox.Location = new Point(30, 30);
            formatBox.Size = new Size(150, 30);
            formatBox.FormattingEnabled = true;
            formatBox.TabIndex = 0;
            formatBox.SelectedIndexChanged += FormatChanged;
            formatBox.Items.AddRange(Config.ImageFormats.Keys.ToArray());
            formatBox.Text = SettingsService.Current.SafeImageExt;
            
            // Video FPS label
            videoFpsLabel = new Label();
            videoFpsLabel.Location = new Point(195, 75);
            videoFpsLabel.Size = new Size(115, 20);
            videoFpsLabel.AutoSize = true;
            videoFpsLabel.TabIndex = 7;
            videoFpsLabel.Text = Resources.ConfigVideoFps;
            
            // Video FPS box
            videoFpsBox = new ComboBox();
            videoFpsBox.Location = new Point(30, 70);
            videoFpsBox.Size = new Size(150, 30);
            videoFpsBox.FormattingEnabled = true;
            videoFpsBox.TabIndex = 4;
            videoFpsBox.SelectedIndexChanged += VideoFpsChanged;
            videoFpsBox.Items.AddRange(Config.FpsList.ToArray());
            videoFpsBox.Text = SettingsService.Current.SafeVideoFps.ToString();
            
            // Provider to copy label
            copyProviderBox = new Label();
            copyProviderBox.Location = new Point(195, 115);
            copyProviderBox.Size = new Size(125, 20);
            copyProviderBox.AutoSize = true;
            copyProviderBox.TabIndex = 23;
            copyProviderBox.Text = Resources.ConfigCopyProvider;
            
            // Provider to copy checkbox
            urlToCopyBox = new ComboBox();
            urlToCopyBox.Location = new Point(30, 110);
            urlToCopyBox.Size = new Size(150, 30);
            urlToCopyBox.FormattingEnabled = true;
            urlToCopyBox.TabIndex = 22;
            urlToCopyBox.SelectedIndexChanged += UrlToCopyChanged;
            urlToCopyBox.Text = SettingsService.Current.CopyProvider;
            
            // No URL copy checkbox 
            noUrlCopyCheckBox = new CheckBox();
            noUrlCopyCheckBox.Location = new Point(30, 170);
            noUrlCopyCheckBox.Size = new Size(175, 25);
            noUrlCopyCheckBox.AutoSize = true;
            noUrlCopyCheckBox.TabIndex = 1;
            noUrlCopyCheckBox.UseVisualStyleBackColor = true;
            noUrlCopyCheckBox.CheckedChanged += NoCopyChanged;
            noUrlCopyCheckBox.Checked = SettingsService.Current.NoUrlCopy;
            noUrlCopyCheckBox.Text = Resources.ConfigNoCopy;
            
            // No image copy
            noImageCopyCheckBox = new CheckBox();
            noImageCopyCheckBox.Location = new Point(30, 200);
            noImageCopyCheckBox.Size = new Size(200, 25);
            noImageCopyCheckBox.AutoSize = true;
            noImageCopyCheckBox.TabIndex = 26;
            noImageCopyCheckBox.UseVisualStyleBackColor = true;
            noImageCopyCheckBox.CheckedChanged += NoImageCopyChanged;
            noImageCopyCheckBox.Checked = SettingsService.Current.NoImageCopy;
            noImageCopyCheckBox.Text = Resources.ConfigNoImageCopy;
            
            // No transparency checkbox
            noTransparencyCheckBox = new CheckBox();
            noTransparencyCheckBox.Location = new Point(30, 230);
            noTransparencyCheckBox.Size = new Size(215, 25);
            noTransparencyCheckBox.AutoSize = true;
            noTransparencyCheckBox.TabIndex = 9;
            noTransparencyCheckBox.UseVisualStyleBackColor = true;
            noTransparencyCheckBox.CheckedChanged += NoTransparencyChanged;
            noTransparencyCheckBox.Checked = SettingsService.Current.NoTransparency;
            noTransparencyCheckBox.Text = Resources.ConfigNoTransparency;
            
            // Add provider label
            addProviderLabel = new Label();
            addProviderLabel.Location = new Point(425, 30);
            addProviderLabel.Size = new Size(135, 20);
            addProviderLabel.AutoSize = true;
            addProviderLabel.TabIndex = 21;
            addProviderLabel.Text = Resources.ConfigAddProvider;
            
            // Add provider box
            addProviderBox = new ListBox();
            addProviderBox.Location = new Point(425, 55);
            addProviderBox.Size = new Size(180, 230);
            addProviderBox.FormattingEnabled = true;
            addProviderBox.ItemHeight = 20;
            addProviderBox.TabIndex = 18;
            addProviderBox.SelectedIndexChanged += OnAddProvider;
            
            // Remove provider label
            removeProviderLabel = new Label();
            removeProviderLabel.Location = new Point(650, 30);
            removeProviderLabel.Size = new Size(160, 20);
            removeProviderLabel.AutoSize = true;
            removeProviderLabel.TabIndex = 20;
            removeProviderLabel.Text = Resources.ConfigRemoveProvider;
            
            // Remove provider box
            removeProviderBox = new ListBox();
            removeProviderBox.Location = new Point(650, 55);
            removeProviderBox.Size = new Size(180, 230);
            removeProviderBox.FormattingEnabled = true;
            removeProviderBox.ItemHeight = 20;
            removeProviderBox.TabIndex = 19;
            removeProviderBox.SelectedIndexChanged += OnRemoveProvider;
            
            // Update link label
            updateLinkLabel = new LinkLabel();
            updateLinkLabel.Location = new Point(30, 270);
            updateLinkLabel.Size = new Size(130, 20);
            updateLinkLabel.AutoSize = true;
            updateLinkLabel.TabIndex = 25;
            updateLinkLabel.TabStop = true;
            updateLinkLabel.Text = Resources.ConfigUpdateLink;
            updateLinkLabel.Visible = false;
            
            AutoScaleDimensions = new SizeF(9F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(860, 310);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            StartPosition = FormStartPosition.CenterScreen;
            
            Controls.Add(noImageCopyCheckBox);
            Controls.Add(updateLinkLabel);
            Controls.Add(copyProviderBox);
            Controls.Add(urlToCopyBox);
            Controls.Add(addProviderLabel);
            Controls.Add(removeProviderLabel);
            Controls.Add(removeProviderBox);
            Controls.Add(addProviderBox);
            Controls.Add(noTransparencyCheckBox);
            Controls.Add(videoFpsLabel);
            Controls.Add(formatLabel);
            Controls.Add(videoFpsBox);
            Controls.Add(noUrlCopyCheckBox);
            Controls.Add(formatBox);
            
            ResumeLayout(false);
            PerformLayout();
        }
        
        /// <summary>
        /// Change image format.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormatChanged(object sender, EventArgs e)
        {
            SettingsService.Current.ImageExt = formatBox.Text;
        }

        /// <summary>
        /// Change FPS value.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void VideoFpsChanged(object sender, EventArgs e)
        {
            try
            {
                SettingsService.Current.VideoFps = int.Parse(videoFpsBox.Text);
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
            SettingsService.Current.CopyProvider = urlToCopyBox.Text;
        }

        /// <summary>
        /// Disable or enable URL copy.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NoCopyChanged(object sender, EventArgs e)
        {
            SettingsService.Current.NoUrlCopy = noUrlCopyCheckBox.Checked;
        }

        /// <summary>
        /// Change image copy value.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NoImageCopyChanged(object sender, EventArgs e)
        {
            SettingsService.Current.NoImageCopy = noImageCopyCheckBox.Checked;
        }

        /// <summary>
        /// Disable or enable transparency.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NoTransparencyChanged(object sender, EventArgs e)
        {
            SettingsService.Current.NoTransparency = noTransparencyCheckBox.Checked;
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
            urlToCopyBox.Items.AddRange(ProviderService.Loaded.Keys.ToArray());
            removeProviderBox.Items.AddRange(ProviderService.Loaded.Keys.ToArray());
            addProviderBox.Items.AddRange(ProviderService.Available.Select(p => p.Name).ToArray());
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
            var provider = ProviderService.GetProviderByName(name);

            await ProviderService.RegisterProvider(provider, $"{name} {number}");

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

            ProviderService.ClearProvider(name);
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
