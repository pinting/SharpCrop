using SharpCrop.Forms;
using System;
using System.Windows.Forms;

namespace SharpCrop
{
    static class Program
    {
        [STAThread]
        static public void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }
}
