using SharpCrop.Dropbox;
using System;
using System.Windows.Forms;

namespace SharpCrop
{
    static class Program
    {
        [STAThread]
        static public void Main()
        {
            DropboxToken.GetToken(delegate (string token)
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new ClickForm());
            });
        }
    }
}
