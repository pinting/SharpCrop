using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using SharpCrop.Models;

namespace SharpCrop.Modules
{
    /// <summary>
    /// Capture helper is responsible for capturing a part of the screen (with or without scaling).
    /// </summary>
    public static class CaptureHelper
    {
        private static readonly List<GifFrame> frames = new List<GifFrame>();
        private static TaskCompletionSource<MemoryStream> result;
        private static float manualScaling;
        private static bool recording;

        /// <summary>
        /// Safe manual scaling.
        /// </summary>
        private static float ManualScaling
        {
            get { return manualScaling; }
            set { manualScaling = value > 0 ? value : manualScaling; }
        }

        /// <summary>
        /// Check if a frame exists.
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        private static bool FrameExists(int i)
        {
            return frames.Count > i && frames[i] != null;
        }

        /// <summary>
        /// Resize a Bitmap.
        /// </summary>
        /// <param name="bitmap"></param>
        /// <param name="w"></param>
        /// <param name="h"></param>
        /// <returns></returns>
        public static Bitmap ResizeBitmap(Bitmap bitmap, int w, int h)
        {
            var resized = new Bitmap(w, h);
            var graph = Graphics.FromImage(resized);
            
            graph.FillRectangle(Brushes.Black, new RectangleF(0, 0, w, h));
            graph.DrawImage(bitmap, new Rectangle(0, 0, w, h));

            return resized;
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
        
		/// <summary>
		/// Get the percentage of scalling.
		/// </summary>
		/// <returns></returns>
		private static float GetScaling()
		{
            // For Unix
		    if (VersionHelper.GetSystemType() != SystemType.Windows)
            {
                using (var graphics = Graphics.FromHwnd(IntPtr.Zero))
                {
                    return graphics.DpiX / 96;
                }
            }

            // For Windows
            var gfx = Graphics.FromHwnd(IntPtr.Zero);
            var desktop = gfx.GetHdc();

            var logicalHeight = GetDeviceCaps(desktop, 10);
            var physicalHeight = GetDeviceCaps(desktop, 117);

            return (float)physicalHeight / logicalHeight;
		}

        /// <summary>
        /// Start capturing frames with CaptureHelper.
        /// </summary>
        /// <param name="rectangle"></param>
        /// <param name="offset"></param>
        private static void CaptureFrames(Rectangle rectangle, Point offset)
        {
            var freq = (1000 / ConfigHelper.Current.SafeVideoFps);
            var wait = 0;

            while (recording)
            {
                Thread.Sleep(wait < 0 ? 0 : wait);

                var delay = Stopwatch.StartNew();
                var image = GetBitmap(rectangle, offset);
                var frame = new GifFrame()
                {
                    Image = image,
                    Delay = freq
                };

                delay.Stop();

                wait = freq - (int)delay.ElapsedMilliseconds;
                frame.Delay += (wait < 0 ? -1 * wait : 0);

                frames.Add(frame);
            }
        }

        /// <summary>
        /// Check if two frames are the same.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="step">Distance between tested pixels</param>
        /// <param name="diff">Maximum difference in colors</param>
        /// <returns></returns>
        private static bool CompareFrames(GifFrame a, GifFrame b, int step = 1, int diff = 0)
        {
            if (!a.Image.Size.Equals(b.Image.Size))
            {
                return false;
            }

            for (var x = 0; x < a.Image.Width; x += step)
            {
                for (var y = 0; y < a.Image.Height; y += step)
                {
                    var p = a.Image.GetPixel(x, y);
                    var q = b.Image.GetPixel(x, y);

                    if (Math.Abs(p.R - q.R) > diff || Math.Abs(p.G - q.G) > diff || Math.Abs(p.B - q.B) > diff)
                    {
                        return false;
                    }
                }
            }

            return true;
        }
        
        /// <summary>
        /// Old NGif encoder for Mono.
        /// </summary>
        private static void EncodeGifUnix()
        {
            var stream = new MemoryStream();
            var gif = new Utils.NGif.AnimatedGifEncoder();

            gif.Start(stream);
            gif.SetQuality(Constants.GifQuality);
            gif.SetRepeat(0);

            while (recording || frames.Count > 0)
            {
                if (!FrameExists(0))
                {
                    continue;
                }

                gif.AddFrame(frames[0].Image);
                frames[0].Image.Dispose();
                frames.RemoveAt(0);
            }

            gif.SetDelay(1000 / ConfigHelper.Current.SafeVideoFps);
            gif.Finish();
            StopRecording();

            result.SetResult(stream);
        }

        /// <summary>
        /// Start encoding GIF while waiting for new frames. If the capture process is still recording, the algorithm
        /// waits for minimum 2 frames, then it will compares them. If they are the same, there is no need to save
        /// each, the first one gonna have an additional delay, while the second one will be deleted.
        /// </summary>
        private static void EncodeGif()
        {
            var stream = new MemoryStream();

            // Wait for the first frame
            while (!FrameExists(0))
            {
            }

            using (var gif = new Utils.BumpKit.GifEncoder(stream, frames[0].Image.Width, frames[0].Image.Height, 0))
            {
                while (recording || frames.Count > 0)
                {
                    // Check if it is possible to remove a frame
                    if (FrameExists(1) &&
                        CompareFrames(frames[0], frames[1], Constants.GifCheckStep, Constants.GifMaxColorDiff))
                    {
                        frames[0].Delay += frames[1].Delay;
                        frames[1].Image.Dispose();
                        frames.RemoveAt(1);
                    }

                    // Save a frame to the Gif
                    // - if 0 and 1 are different
                    // - if there is one last frame left (and the capture process is stopped)
                    else if (FrameExists(1) || FrameExists(0) && !recording)
                    {
                        gif.AddFrame(frames[0].Image, 0, 0, TimeSpan.FromMilliseconds(frames[0].Delay));
                        frames[0].Image.Dispose();
                        frames.RemoveAt(0);
                    }
                }
            }

            StopRecording();
            result.SetResult(stream);
        }

        /// <summary>
        /// RecordVideo Mpeg using FFmpeg. Sadly pipes are not seekable, so we have to use the file system as a destination
        /// for the output and read it back MemoryStream. This is extremely bad for solid states drives.
        /// </summary>
        private static void EncodeMpeg()
        {
            var temp = Guid.NewGuid().ToString();
            var output = new MemoryStream();

            try
            {
                using (var ffmpeg = new Process())
                {
                    var fps = ConfigHelper.Current.SafeVideoFps;

                    ffmpeg.StartInfo.FileName = "ffmpeg";
                    ffmpeg.StartInfo.Arguments = $"-f image2pipe -r {fps} -i pipe:0 -r {fps} -an -y -f mp4 {temp}";
                    ffmpeg.StartInfo.RedirectStandardInput = true;
                    ffmpeg.StartInfo.UseShellExecute = false;
                    ffmpeg.StartInfo.CreateNoWindow = true;

                    ffmpeg.Start();

                    while (recording || frames.Count > 0)
                    {
                        if (!FrameExists(0))
                        {
                            continue;
                        }

                        using (var stream = new MemoryStream())
                        {
                            frames[0].Image.Save(stream, ImageFormat.Bmp);
                            stream.WriteTo(ffmpeg.StandardInput.BaseStream);
                            frames[0].Image.Dispose();
                            frames.RemoveAt(0);
                        }
                    }

                    ffmpeg.StandardInput.BaseStream.Close();
                    ffmpeg.WaitForExit();
                }

                // TODO: Experiment with something (RAMDisk?)
                using (var file = new FileStream(temp, FileMode.Open))
                {
                    var buffer = new byte[512];

                    while (file.Read(buffer, 0, buffer.Length) > 0)
                    {
                        output.Write(buffer, 0, buffer.Length);
                    }
                }

                result.SetResult(output);
            }
            catch
            {
                output.Dispose();
                result.SetResult(new MemoryStream(0));
            }
            finally
            {
                if (File.Exists(temp))
                {
                    File.Delete(temp);
                }

                StopRecording();
            }
        }

        /// <summary>
        /// Start a recording with the given encoder.
        /// </summary>
        /// <param name="region"></param>
        /// <param name="offset"></param>
        /// <param name="encoder"></param>
        /// <returns></returns>
        private static Task<MemoryStream> RecordVideo(Rectangle region, Point offset, Action encoder)
        {
            if (result != null && !result.Task.IsCompleted)
            {
                return Task.FromResult<MemoryStream>(null);
            }

            recording = true;
            result = new TaskCompletionSource<MemoryStream>();

            // Run the two process in the same time
            Task.Run(() => CaptureFrames(region, offset));
            Task.Run(encoder);

            return result.Task;
        }

        /// <summary>
        /// RecordVideo video into GIF.
        /// </summary>
        /// <param name="region"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        public static Task<MemoryStream> RecordGif(Rectangle region, Point offset)
        {
            if (VersionHelper.GetSystemType() == SystemType.Windows)
            {
                return RecordVideo(region, offset, EncodeGif);
            }

            return RecordVideo(region, offset, EncodeGifUnix);
        }

        /// <summary>
        /// RecordVideo video into Mpeg.
        /// </summary>
        /// <param name="region"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        public static Task<MemoryStream> RecordMpeg(Rectangle region, Point offset)
        {
            return RecordVideo(region, offset, EncodeMpeg);
        }

        /// <summary>
        /// Request stop - the encoder will finish captured frames before it terminates.
        /// </summary>
        public static void StopRecording()
        {
            recording = false;
        }


        /// <summary>
        /// Get display informations on Windows.
        /// </summary>
        /// <param name="hdc"></param>
        /// <param name="nIndex"></param>
        /// <returns></returns>
        [DllImport("gdi32.dll")]
        private static extern int GetDeviceCaps(IntPtr hdc, int nIndex);
    }
}
