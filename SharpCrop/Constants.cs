using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;

namespace SharpCrop
{
    public static class Constants
    {
        // List of the registered providers
        public static readonly IReadOnlyDictionary<string, Type> AvailableProviders = new Dictionary<string, Type>()
        {
            {"Dropbox", typeof(Dropbox.Provider)},
            {"GoogleDrive", typeof(GoogleDrive.Provider)},
            {"OneDrive", typeof(OneDrive.Provider)},
            {"FTP", typeof(FTP.Provider)},
            {"LocalFile", typeof(LocalFile.Provider)}
        };

        // List of avaiable image formats
        public static readonly IReadOnlyDictionary<string, ImageFormat> AvailableImageFormats = new Dictionary<string, ImageFormat>()
        {
            {"png", ImageFormat.Png},
            {"jpg", ImageFormat.Jpeg},
            {"bmp", ImageFormat.Bmp}
        };
        
        // List of available FPS values
        public static readonly IReadOnlyList<string> AvailableFps = new List<string>()
        {
            "5",
            "10",
            "15",
            "20",
            "25",
            "30"
        };

        public static readonly int MaxFps = 30;

        // General constants
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
