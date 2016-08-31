using Newtonsoft.Json;
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
        public bool NoCopy { get; set; }

		[JsonProperty]
		public bool NoScaling { get; set; }

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
		public bool Copy
		{
			get 
			{
				return !NoCopy;
			}
        }

        [JsonIgnore]
        public bool Scaling
        {
            get
            {
                return !NoScaling;
            }
        }
    }
}
