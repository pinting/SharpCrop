using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using SharpCrop.Models;
using SharpCrop.Modules;

namespace SharpCrop
{
    public static class Program
    {
        /// <summary>
        /// The main function which is responsible for the loading of the Controller.
        /// </summary>
        [STAThread]
        public static void Main()
        {
            if (VersionHelper.GetSystemType() == SystemType.Windows && Environment.OSVersion.Version.Major >= 6)
            {
                SetProcessDPIAware();
            }

            ConfigHelper.Load();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Controller());

            ConfigHelper.Save();
        }

        /// <summary>
        /// DPI aware Windows feature.
        /// </summary>
        /// <returns></returns>
        [DllImport("user32.dll")]
        private static extern bool SetProcessDPIAware();
    }
}
