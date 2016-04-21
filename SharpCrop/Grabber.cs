using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace SharpCrop
{
    class Grabber
    {
        public static Bitmap GetScreenshot(Rectangle r)
        {
            var bitmap = new Bitmap(r.Width, r.Height, PixelFormat.Format32bppArgb);
            var gfx = Graphics.FromImage(bitmap);
            
            gfx.CopyFromScreen(r.X, r.Y, 0, 0, new Size(r.Width, r.Height), CopyPixelOperation.SourceCopy);
            bitmap.Save(DateTime.Now.Ticks.ToString() + ".png", ImageFormat.Png);

            return bitmap;
        }
    }
}
