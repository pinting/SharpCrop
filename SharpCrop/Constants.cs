using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;

namespace SharpCrop
{
    public static class Constants
    {
        // Version
        public const int Version = 2200;

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
        public const string LatestVersion = "http://api.github.com/repos/pinting/SharpCrop/releases/latest";
        public static readonly Brush RightColor = Brushes.PaleVioletRed;
        public static readonly Brush LeftColor = Brushes.RoyalBlue;
        public const string SettingsPath = "Settings.json";
        public const int VersionLength = 4;
        public const int PenWidth = 2;

        // For .NET (Bumpkit) GifEncoder
        public const int GifMaxColorDiff = 10;
        public const int GifCheckStep = 2;

        // For Mono (NGif) GifEncoder
        public const int GifQuality = 20;
    }
}
