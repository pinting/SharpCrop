using Newtonsoft.Json;
using SharpCrop.Provider.Models;
using System;
using System.IO;

namespace SharpCrop.Provider.Utils
{
    public static class SettingsHelper
    {
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
                Memory = JsonConvert.DeserializeObject<Settings>(File.ReadAllText(Constants.SettingsPath));
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
                File.WriteAllText(Constants.SettingsPath, JsonConvert.SerializeObject(Memory));
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
