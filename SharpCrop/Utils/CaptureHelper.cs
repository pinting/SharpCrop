using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace SharpCrop.Utils
{
    /// <summary>
    /// Capture helper is responsible for capturing a part of the screen (with or without scaling).
    /// </summary>
    public static class CaptureHelper
    {
        /// <summary>
        /// Private helper function to construct a rectangle from two points.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="dest"></param>
        /// <returns></returns>
        public static Rectangle GetRectangle(Point source, Point dest)
        {
            return new Rectangle(
                Math.Min(source.X, dest.X),
                Math.Min(source.Y, dest.Y),
                Math.Abs(source.X - dest.X),
                Math.Abs(source.Y - dest.Y));
        }

        /// <summary>
        /// Get a Bitmap from screen in the size of the given rectangle.
        /// </summary>
        /// <param name="r"></param>
        /// <returns></returns>
        public static Bitmap GetBitmap(Rectangle r)
        {
            var s = GetScaling();
            var rs = new Rectangle(
                (int)(r.X * s), 
                (int)(r.Y * s), 
                (int)(r.Width * s), 
                (int)(r.Height * s));

            var bitmap = new Bitmap(rs.Width, rs.Height, PixelFormat.Format32bppArgb);
            var gfx = Graphics.FromImage(bitmap);
            
            gfx.CopyFromScreen(rs.X, rs.Y, 0, 0, new Size(rs.Width, rs.Height), CopyPixelOperation.SourceCopy);

            return bitmap;
        }

#if __MonoCS__

		/// <summary>
		/// Get the percentage of scalling for Mono on Linux.
		/// </summary>
		/// <returns></returns>
		public static float GetScaling()
		{
            if(ConfigHelper.Memory.NoScaling)
            {
                return 1.0f;
            }
            
            float result;

            using (var graphics = Graphics.FromHwnd(IntPtr.Zero))
            {
                result = graphics.DpiX / 96;
            }

            return result;
		}

#else

        /// <summary>
        /// Get the percentage of scaling for .NET on Windows.
        /// </summary>
        /// <returns></returns>
        public static float GetScaling()
        {
            if(ConfigHelper.Memory.NoScaling)
            {
                return 1.0f;
            }

            var gfx = Graphics.FromHwnd(IntPtr.Zero);
            var desktop = gfx.GetHdc();

            var logicalHeight = GetDeviceCaps(desktop, 10);
            var physicalHeight = GetDeviceCaps(desktop, 117);

            return (float)physicalHeight / logicalHeight;
        }

        [DllImport("gdi32.dll")]
        private static extern int GetDeviceCaps(IntPtr hdc, int nIndex);

#endif

    }
}
