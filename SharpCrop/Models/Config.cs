using Newtonsoft.Json;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Linq;

namespace SharpCrop.Models
{
    /// <summary>
    /// The configuration file memory model. The validators are written into this.
    /// </summary>
    public class Config
    {
        [JsonProperty]
        public string ImageExt { get; set; }

        [JsonProperty]
        public List<int> ManualScaling { get; set; }

        [JsonProperty]
        public int VideoFps { get; set; }

        [JsonProperty]
        public string CopyProvider { get; set; }

        [JsonProperty]
        public bool NoCopy { get; set; }

		[JsonProperty]
		public bool NoAutoScaling { get; set; }

        [JsonProperty]
        public bool NoTransparency { get; set; }

        [JsonProperty]
        public bool EnableMpeg { get; set; }

        [JsonProperty]
        public bool StartupRegister { get; set; }

        [JsonProperty]
        public bool NoWelcome { get; set; }

        [JsonProperty]
        public Dictionary<string, SavedProvider> Providers { get; set; }

        #region Validators

        /// <summary>
        /// Get the ImageFormat form the current image extension.
        /// </summary>
        [JsonIgnore]
        public ImageFormat ImageFormat => Constants.ImageFormats[SafeImageExt];

        /// <summary>
        /// Get the current FPS value in the permitted range.
        /// </summary>
        [JsonIgnore]
        public int SafeVideoFps => VideoFps > 0 && VideoFps <= int.Parse(Constants.FpsList.First()) ? VideoFps : int.Parse(Constants.FpsList.First());

        /// <summary>
        /// Get the manual scalling list - or create a new one and return it.
        /// </summary>
        [JsonIgnore]
        public List<int> SafeManualScaling => ManualScaling ?? (ManualScaling = new List<int>());

        /// <summary>
        /// Get or create a new dictionary for providers.
        /// </summary>
        [JsonIgnore]
        public Dictionary<string, SavedProvider> SafeProviders => Providers ?? (Providers = new Dictionary<string, SavedProvider>());

        /// <summary>
        /// Get the current ImageExt and check if it is valid.
        /// </summary>
        [JsonIgnore]
        public string SafeImageExt => (!string.IsNullOrEmpty(ImageExt) && Constants.ImageFormats.ContainsKey(ImageExt)) ? ImageExt : Constants.ImageFormats.Keys.First();

        #endregion
    }
}
