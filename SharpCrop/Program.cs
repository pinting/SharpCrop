using SharpCrop.Provider.Utils;
using SharpCrop.Utils;
using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace SharpCrop
{
    public static class Program
    {
        [STAThread]
        public static void Main()
        {
            if (Environment.OSVersion.Version.Major >= 6)
            {
                SetProcessDPIAware();
            }

            ConfigHelper.Load();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Loader());

            ConfigHelper.Save();
        }

        [DllImport("user32.dll")]
        private static extern bool SetProcessDPIAware();
    }
}
