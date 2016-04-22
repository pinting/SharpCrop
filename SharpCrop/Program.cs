using System;
using System.Windows.Forms;

namespace SharpCrop
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new ClickForm());
        }
    }
}
