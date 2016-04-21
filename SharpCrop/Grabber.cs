using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace SharpCrop
{
    class Grabber
    {
        public Bitmap GetScreenshot(Point source, Point dest)
        {
            var bitmap = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height, PixelFormat.Format32bppArgb);
            var gfx = Graphics.FromImage(bitmap);

            gfx.CopyFromScreen(source.X, source.Y, dest.X, dest.Y, Screen.PrimaryScreen.Bounds.Size, CopyPixelOperation.SourceCopy);

            bitmap.Save("test.png", ImageFormat.Png);

            return bitmap;
        }
    }
}
