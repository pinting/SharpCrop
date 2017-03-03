using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;

namespace SharpCrop
{
    public static class Constants
    {
        // Version
        public static readonly int Version = 2000;

        // List of the registered providers
        public static readonly IReadOnlyList<Type> Providers = new List<Type>()
        {
            typeof(Dropbox.Provider),
            typeof(GoogleDrive.Provider),
            typeof(OneDrive.Provider),
            typeof(FTP.Provider),
            typeof(LocalFile.Provider)
        };

        // List of avaiable image formats (first if default)
        public static readonly IReadOnlyDictionary<string, ImageFormat> ImageFormats = new Dictionary<string, ImageFormat>()
        {
            {"png", ImageFormat.Png},
            {"jpg", ImageFormat.Jpeg},
            {"bmp", ImageFormat.Bmp}
        };
        
        // List of available FPS values (the first must be the greatest)
        public static readonly IReadOnlyList<string> FpsList = new List<string>()
        {
            "30",
            "25",
            "20",
            "15",
            "10",
            "5"
        };

        // General constants
        public static readonly string UpdateLink = "http://api.github.com/repos/pinting/SharpCrop/releases/latest";
        public static readonly string SettingsPath = "Settings.json";
        public static readonly Brush LeftColor = Brushes.RoyalBlue;
        public static readonly Brush RightColor = Brushes.PaleVioletRed;
        public static readonly int VersionLength = 4;
        public static readonly int PenWidth = 2;

        // For .NET (Bumpkit) GifEncoder
        public static readonly int GifMaxColorDiff = 10;
        public static readonly int GifCheckStep = 2;

        // For Mono (NGif) GifEncoder
        public static readonly int GifQuality = 20;
    }
}
