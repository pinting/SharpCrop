using Newtonsoft.Json;
using SharpCrop.Models;
using System;
using System.IO;

namespace SharpCrop.Utils
{
    public static class SettingsHelper
    {
        private static readonly string path = "Settings.json";

        /// <summary>
        /// In-memory settings.
        /// </summary>
        public static Settings Memory;

        /// <summary>
        /// Load settings into memory.
        /// </summary>
        public static void Load()
        {
            try
            {
                Memory = JsonConvert.DeserializeObject<Settings>(File.ReadAllText(path));
            }
            catch
            {
                Memory = new Settings();
            }

        }

        /// <summary>
        /// Save it back to file.
        /// </summary>
        public static void Save()
        {
            try
            {
                File.WriteAllText(path, JsonConvert.SerializeObject(Memory));
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        /// <summary>
        /// Reset in-memory settings.
        /// </summary>
        public static void Reset()
        {
            Memory = new Settings();
        }
    }
}
