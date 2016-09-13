using System.Drawing;

namespace SharpCrop
{
    public static class Constants
    {
        public static readonly string SettingsPath = "Settings.json";
        public static readonly Brush LeftColor = Brushes.RoyalBlue;
        public static readonly Brush RightColor = Brushes.PaleVioletRed;
        public static readonly int PenWidth = 2;

        // For .NET (Bumpkit) GifEncoder
        public static readonly int GifMaxColorDiff = 10;
        public static readonly int GifCheckStep = 2;

        // For Mono (NGif) GifEncoder
        public static readonly int GifQuality = 20;
    }
}
