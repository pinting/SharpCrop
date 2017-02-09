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
        public string ImageFormat { get; set; }

        [JsonProperty]
        public List<int> ManualScaling { get; set; }

        [JsonProperty]
        public int VideoFps { get; set; }

        [JsonProperty]
        public string ProviderToCopy { get; set; }

        [JsonProperty]
        public bool NoCopy { get; set; }

		[JsonProperty]
		public bool NoAutoScaling { get; set; }

        [JsonProperty]
        public bool NoGifRepeat { get; set; }

        [JsonProperty]
        public bool NoFocus { get; set; }

        [JsonProperty]
        public bool NoTransparency { get; set; }

        [JsonProperty]
        public bool EnableMpeg { get; set; }

        [JsonProperty]
        public Dictionary<string, string> Providers { get; set; }

        #region Validators

        [JsonIgnore]
        public ImageFormat ImageFormatType => Constants.AvailableImageFormats[SafeImageFormat];

        [JsonIgnore]
        public int SafeVideoFps => VideoFps > 0 && VideoFps <= Constants.MaxFps ? VideoFps : Constants.MaxFps;

        [JsonIgnore]
        public List<int> SafeManualScaling => ManualScaling ?? (ManualScaling = new List<int>());

        [JsonIgnore]
        public Dictionary<string, string> SafeProviders => Providers ?? (Providers = new Dictionary<string, string>());

        [JsonIgnore]
        public string SafeImageFormat
        {
            get
            {
                if (!string.IsNullOrEmpty(ImageFormat) && Constants.AvailableImageFormats.ContainsKey(ImageFormat))
                {
                    return ImageFormat;
                }

                return Constants.AvailableImageFormats.Keys.First();
            }
        }

        #endregion
    }
}
