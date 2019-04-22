using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using SharpCrop.Models;

namespace SharpCrop.Services
{
    /// <summary>
    /// Capture service is responsible for capturing a part of the screen.
    /// </summary>
    public static class CaptureService
    {
        private static readonly List<Frame> videoFrames = new List<Frame>();
        private static TaskCompletionSource<MemoryStream> videoResult;
        private static bool isRecording;

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
        /// Capture a Bitmap from screen in the size of the given rectangle.
        /// </summary>
        /// <param name="region"></param>
        /// <returns></returns>
        public static Bitmap CaptureBitmap(Rectangle region)
        {
            var bitmap = new Bitmap(region.Width, region.Height, PixelFormat.Format32bppArgb);
            var gfx = Graphics.FromImage(bitmap);
            
            gfx.CopyFromScreen(
                region.X, 
                region.Y, 
                0, 
                0, 
                new Size(region.Width, region.Height), 
                CopyPixelOperation.SourceCopy);

            return bitmap;
        }

        /// <summary>
        /// Start capturing frames.
        /// </summary>
        /// <param name="region"></param>
        /// <param name="fps">FPS to record with.</param>
        private static void CaptureFrames(Rectangle region, int fps)
        {
            var freq = 1000 / fps;
            var wait = 0;

            while (isRecording)
            {
                Thread.Sleep(wait < 0 ? 0 : wait);

                var delay = Stopwatch.StartNew();
                var image = CaptureBitmap(region);
                var frame = new Frame { Image = image, Delay = freq };

                delay.Stop();

                wait = freq - (int)delay.ElapsedMilliseconds;
                frame.Delay += wait < 0 ? -1 * wait : 0;

                lock(videoFrames)
                {
                    videoFrames.Add(frame);
                }
            }
        }

        /// <summary>
        /// Record video using FFMpeg. Sadly pipes are not seekable, so we have to use the file system as
        /// a destination for the output and read it back MemoryStream. This is bad for solid states drives.
        /// </summary>
        /// <param name="ext">Container type to use. FFMpeg default codec gonna be used for this type.</param>
        /// <param name="fps">FPS to record with.</param>
        private static void EncodeVideo(string ext, int fps)
        {
            var temp = Guid.NewGuid().ToString();
            var output = new MemoryStream();

            try
            {
                using (var process = new Process())
                {
                    process.StartInfo.FileName = "ffmpeg";
                    process.StartInfo.Arguments = $"-f image2pipe -r {fps} -i pipe:0 -r {fps} -an -y -f {ext} {temp}";
                    process.StartInfo.RedirectStandardInput = true;
                    process.StartInfo.UseShellExecute = false;
                    process.StartInfo.CreateNoWindow = true;

                    process.Start();

                    while (isRecording)
                    {
                        Frame frame;
                        
                        lock (videoFrames)
                        {
                            if(videoFrames.Count == 0)
                            {
                                continue;
                            }

                            frame = videoFrames[0];
                            videoFrames.RemoveAt(0);
                        }

                        using (var stream = new MemoryStream())
                        {
                            stream.WriteTo(process.StandardInput.BaseStream);
                            frame.Image.Save(stream, ImageFormat.Bmp);
                        }
                            
                        frame.Image.Dispose();
                    }

                    process.StandardInput.BaseStream.Close();
                    process.WaitForExit();
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

                videoResult.SetResult(output);
            }
            catch
            {
                output.Dispose();
                videoResult.SetResult(new MemoryStream(0));
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
        /// Start a video recording with FFMpeg.
        /// </summary>
        /// <param name="region"></param>
        /// <param name="ext">Container type to use. FFMpeg default codec gonna be used for this type.</param>
        /// <param name="fps">FPS to record with.</param>
        /// <returns></returns>
        public static Task<MemoryStream> RecordVideo(Rectangle region, string ext, int fps)
        {
            if (videoResult != null && !videoResult.Task.IsCompleted)
            {
                return Task.FromResult<MemoryStream>(null);
            }

            videoResult = new TaskCompletionSource<MemoryStream>();
            isRecording = true;

            // Run the two process in the same time
            var captureThread = new Thread(() => CaptureFrames(region, fps));
            var encoderThread = new Thread(() => EncodeVideo(ext, fps));
            
            captureThread.Start();
            encoderThread.Start();

            return videoResult.Task;
        }

        /// <summary>
        /// Request stop - the encoder will finish captured frames before it terminates.
        /// </summary>
        public static void StopRecording()
        {
            isRecording = false;
        }
    }
}
