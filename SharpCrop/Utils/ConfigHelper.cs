using Newtonsoft.Json;
using SharpCrop.Models;
using System;
using System.IO;

namespace SharpCrop.Utils
{
    /// <summary>
    /// ConfigHelper reads the configuration json into memory and modify it if it is needed.
    /// </summary>
    public static class ConfigHelper
    {
        /// <summary>
        /// In-memory settings.
        /// </summary>
        public static Config Memory;

        /// <summary>
        /// Load settings into memory.
        /// </summary>
        public static void Load()
        {
            try
            {
                Memory = JsonConvert.DeserializeObject<Config>(File.ReadAllText(Constants.SettingsPath));
            }
            catch
            {
                Memory = new Config();
            }

        }

        /// <summary>
        /// Save it back to file.
        /// </summary>
        public static void Save()
        {
            try
            {
                File.WriteAllText(Constants.SettingsPath, JsonConvert.SerializeObject(Memory, Formatting.Indented));
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
            Memory = new Config();
        }
    }
}
