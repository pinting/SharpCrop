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
        public int GifFps { get; set; }

        [JsonProperty]
        public bool NoCopy { get; set; }

		[JsonProperty]
		public bool NoScaling { get; set; }

        [JsonProperty]
        public bool NoGifRepeat { get; set; }

        [JsonProperty]
        public bool NoFocus { get; set; }

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
        public int SafeGifFps
        {
            get
            {
                return GifFps > 0 && GifFps <= 30 ? GifFps : 30;
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

        [JsonIgnore]
        public bool GifRepeat
        {
            get
            {
                return !NoGifRepeat;
            }
        }

        [JsonIgnore]
        public bool Focus
        {
            get
            {
                return !NoFocus;
            }
        }

        #endregion
    }
}
