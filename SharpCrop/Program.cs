using SharpCrop.Token;
using System;
using System.Windows.Forms;

namespace SharpCrop
{
    static class Program
    {
        [STAThread]
        static public void Main()
        {
            var token = new ExternalGrabber(delegate(string result)
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new ClickForm());
            });
        }
    }
}
