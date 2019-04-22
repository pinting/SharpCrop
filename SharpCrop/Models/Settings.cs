using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Linq;
using Newtonsoft.Json;

// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable MemberCanBePrivate.Global

namespace SharpCrop.Models
{
    /// <summary>
    /// The configuration file memory model. The validators are written into this.
    /// </summary>
    public class Settings
    {
        [JsonProperty]
        public string ImageExt { get; set; }

        [JsonProperty]
        public int VideoFps { get; set; }

        [JsonProperty]
        public string CopyProvider { get; set; }

        [JsonProperty]
        public bool NoUrlCopy { get; set; }

        [JsonProperty]
        public bool NoImageCopy { get; set; }

        [JsonProperty]
        public bool NoTransparency { get; set; }

        [JsonProperty]
        public bool NoWelcome { get; set; }

        [JsonProperty]
        public Dictionary<string, SavedProvider> Providers { get; set; }

        #region Validators

        /// <summary>
        /// Get the ImageFormat form the current image extension.
        /// </summary>
        [JsonIgnore]
        public ImageFormat ImageFormat => Config.ImageFormats[SafeImageExt];

        /// <summary>
        /// Get the current FPS value in the permitted range.
        /// </summary>
        [JsonIgnore]
        public int SafeVideoFps => 
            VideoFps > 0 && VideoFps <= int.Parse(Config.FpsList.First()) 
                ? VideoFps 
                : int.Parse(Config.FpsList.First());

        /// <summary>
        /// Get or create a new dictionary for providers.
        /// </summary>
        [JsonIgnore]
        public Dictionary<string, SavedProvider> SafeProviders => 
            Providers ?? (Providers = new Dictionary<string, SavedProvider>());

        /// <summary>
        /// Get the current ImageExt and check if it is valid.
        /// </summary>
        [JsonIgnore]
        public string SafeImageExt =>
            !string.IsNullOrEmpty(ImageExt) && Config.ImageFormats.ContainsKey(ImageExt) 
                ? ImageExt 
                : Config.ImageFormats.Keys.First();

        #endregion
    }
}
