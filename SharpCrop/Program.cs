using SharpCrop.Utils;
using SharpCrop.Utils.Gif;
using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace SharpCrop
{
    public static class Program
    {
        [STAThread]
        public static void Main()
        {
            /*
            var list = new string[] { "01.png", "02.png", "03.png" };
            var output = "test.gif";

            var gif = new AnimatedGifEncoder();

            gif.Start(output);
            gif.SetDelay(500);
            gif.SetRepeat(0);

            for (int i = 0, count = list.Length; i < count; i++)
            {
                gif.AddFrame(Image.FromFile(list[i]));
            }

            gif.Finish();
            */

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
