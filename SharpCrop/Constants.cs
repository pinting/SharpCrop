using System;
using System.Collections.Generic;
using System.Drawing;

namespace SharpCrop
{
    public static class Constants
    {
        /// All available providers.
        public static readonly IReadOnlyDictionary<string, Type> Providers = new Dictionary<string, Type>()
        {
            {"Dropbox", typeof(Dropbox.Provider)},
            {"GoogleDrive", typeof(GoogleDrive.Provider)},
            {"OneDrive", typeof(OneDrive.Provider)},
            {"LocalFile", typeof(LocalFile.Provider)}
        };

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
