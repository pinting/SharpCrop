using Newtonsoft.Json;
using SharpCrop.Models;
using System;
using System.IO;

namespace SharpCrop.Services
{
    /// <summary>
    /// ConfigHelper reads the configuration JSON into the memory and modifies it if it is needed.
    /// </summary>
    public static class ConfigService
    {
        /// <summary>
        /// In-memory settings.
        /// </summary>
        public static Config Current;

        /// <summary>
        /// Load settings into memory.
        /// </summary>
        public static void Load()
        {
            try
            {
                Current = JsonConvert.DeserializeObject<Config>(File.ReadAllText(Constants.SettingsPath));
            }
            catch
            {
                Current = new Config();
            }

        }

        /// <summary>
        /// Save it back to file.
        /// </summary>
        public static void Save()
        {
            try
            {
                File.WriteAllText(Constants.SettingsPath, JsonConvert.SerializeObject(Current, Formatting.Indented));
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
            Current = new Config();
        }
    }
}
