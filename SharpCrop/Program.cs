using SharpCrop.Utils;
using System;
using System.Windows.Forms;

namespace SharpCrop
{
    static class Program
    {
        [STAThread]
        public static void Main()
        {
            SettingsHelper.Load();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Loader());

            SettingsHelper.Save();
        }
    }
}
