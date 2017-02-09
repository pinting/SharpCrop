using Newtonsoft.Json;
using System.Collections.Generic;
using System.Drawing.Imaging;

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
        public int VideoFPS { get; set; }

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
        public ImageFormat ImageFormatType
        {
            get
            {
                return Constants.AvailableImageFormats[SafeImageFormat];
            }
        }

        [JsonIgnore]
        public string SafeImageFormat
        {
            get
            {
                if (!string.IsNullOrEmpty(ImageFormat) && Constants.AvailableImageFormats.ContainsKey(ImageFormat))
                {
                    return ImageFormat;
                }
                else
                {
                    return Constants.DefaultImageFormat;
                }
            }
        }

        [JsonIgnore]
        public int SafeVideoFPS
        {
            get
            {
                return VideoFPS > 0 && VideoFPS <= Constants.MaxFPS ? VideoFPS : Constants.MaxFPS;
            }
        }

        [JsonIgnore]
        public List<int> SafeManualScaling
        {
            get
            {
                if(ManualScaling == null)
                {
                    ManualScaling = new List<int>();
                }

                return ManualScaling;
            }
        }

        [JsonIgnore]
        public Dictionary<string, string> SafeProviders
        {
            get
            {
                if (Providers == null)
                {
                    Providers = new Dictionary<string, string>();
                }

                return Providers;
            }
        }

        #endregion
    }
}
