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
        public static float ManualScaling;

        /// <summary>
        /// Resize a Bitmap.
        /// </summary>
        /// <param name="bitmap"></param>
        /// <param name="w"></param>
        /// <param name="h"></param>
        /// <returns></returns>
        public static Bitmap ResizeBitmap(Bitmap bitmap, int w, int h)
        {
            var result = new Bitmap(w, h);
            var graph = Graphics.FromImage(result);
            
            graph.FillRectangle(Brushes.Black, new RectangleF(0, 0, w, h));
            graph.DrawImage(bitmap, new Rectangle(0, 0, w, h));

            return result;
        }

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
        /// <param name="rectangle"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        public static Bitmap GetBitmap(Rectangle rectangle, Point offset)
        {
            float scaling;

            if(ConfigHelper.Current.NoAutoScaling && ManualScaling > 0.0f)
            {
                scaling = ManualScaling;
            }
            else
            {
                scaling = GetScaling();
            }

            var r = new Rectangle(
                (int)(rectangle.X * scaling) + offset.X, 
                (int)(rectangle.Y * scaling) + offset.Y, 
                (int)(rectangle.Width * scaling), 
                (int)(rectangle.Height * scaling));

            var bitmap = new Bitmap(r.Width, r.Height, PixelFormat.Format32bppArgb);
            var gfx = Graphics.FromImage(bitmap);
            
            gfx.CopyFromScreen(r.X, r.Y, 0, 0, new Size(r.Width, r.Height), CopyPixelOperation.SourceCopy);

            return bitmap;
        }
        
        /// <summary>
        /// Set manual scalling from the ConfigHelper by screen index.
        /// </summary>
        /// <param name="index"></param>
        public static void SetManualScaling(int index)
        {
            var list = ConfigHelper.Current.SafeManualScaling;

            if (index < list.Count)
            {
                // ManualScalling is in percentage
                ManualScaling = list[index] / 100.0f;
            }
        }

#if __MonoCS__

		/// <summary>
		/// Get the percentage of scalling for Mono on Linux.
		/// </summary>
		/// <returns></returns>
		public static float GetScaling()
		{
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
