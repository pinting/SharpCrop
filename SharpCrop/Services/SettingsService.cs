using System;
using System.IO;
using Newtonsoft.Json;
using SharpCrop.Models;

namespace SharpCrop.Services
{
    /// <summary>
    /// Settings service reads the configuration JSON into the memory and can modify it.
    /// </summary>
    public static class SettingsService
    {
        /// <summary>
        /// In-memory settings.
        /// </summary>
        public static Settings Current;

        /// <summary>
        /// Load settings into memory.
        /// </summary>
        public static void Load()
        {
            try
            {
                Current = JsonConvert.DeserializeObject<Settings>(File.ReadAllText(Config.SettingsPath));
            }
            catch
            {
                Current = new Settings();
            }

        }

        /// <summary>
        /// Save it back to file.
        /// </summary>
        public static void Save()
        {
            try
            {
                File.WriteAllText(Config.SettingsPath, JsonConvert.SerializeObject(Current, Formatting.Indented));
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
            Current = new Settings();
        }
    }
}
