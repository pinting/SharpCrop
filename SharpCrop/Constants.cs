using System.Drawing;

namespace SharpCrop
{
    public static class Constants
    {
        public static readonly string SettingsPath = "Settings.json";
        public static readonly Brush LeftColor = Brushes.RoyalBlue;
        public static readonly Brush RightColor = Brushes.PaleVioletRed;
        public static readonly int GifMaxColorDiff = 10;
        public static readonly int GifCheckStep = 2;
    }
}
