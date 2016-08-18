using Newtonsoft.Json;
using System.Drawing.Imaging;

namespace SharpCrop.Models
{
    public class Config
    {
        [JsonProperty]
        public string Provider { get; set; }

        [JsonProperty]
        public string Token { get; set; }

        [JsonProperty]
        public string Format { get; set; }

        [JsonProperty]
        public bool Copy { get; set; }

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
    }
}
