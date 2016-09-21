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
        public string Provider { get; set; }

        [JsonProperty]
        public string Token { get; set; }

        [JsonProperty]
        public string Format { get; set; }

        [JsonProperty]
        public List<int> ManualScaling { get; set; }

        [JsonProperty]
        public int VideoFPS { get; set; }

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

        #region Validators

        [JsonIgnore]
        public ImageFormat FormatType
        {
            get
            {
                switch(Format)
                {
                    case "jpg":
                        return ImageFormat.Jpeg;
                    case "bmp":
                        return ImageFormat.Bmp;
                    case "png":
                        return ImageFormat.Png;
                    default:
                        return ImageFormat.Png;
                }
            }
        }

        [JsonIgnore]
        public string FormatExt
        {
            get
            {
                switch(Format)
                {
                    case "jpg":
                    case "bmp":
                    case "png":
                        return Format;
                    default:
                        return "png";
                }
            }
        }

        [JsonIgnore]
        public int SafeVideoFPS
        {
            get
            {
                return VideoFPS > 0 && VideoFPS <= 30 ? VideoFPS : 30;
            }
        }

        [JsonIgnore]
        public List<int> SafeManualScaling
        {
            get
            {
                if(ManualScaling == null)
                {
                    return new List<int>();
                }

                return ManualScaling;
            }
        }

        #endregion
    }
}
