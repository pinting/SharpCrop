using SharpCrop.Utils;
using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

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
#if !__MonoCS__
            if (Environment.OSVersion.Version.Major >= 6)
            {
                SetProcessDPIAware();
            }
#endif

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
