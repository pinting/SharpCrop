using SharpCrop.Models;
using SharpCrop.Utils.BumpKit;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace SharpCrop.Utils
{
    public static class VideoFactory
    {
        private static readonly List<GifFrame> frames = new List<GifFrame>();
        private static TaskCompletionSource<MemoryStream> result;
        private static bool running;

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
        /// Start capturing frames with CaptureHelper.
        /// </summary>
        /// <param name="rectangle"></param>
        /// <param name="offset"></param>
        private static void CaptureFrames(Rectangle rectangle, Point offset)
        {
            var freq = (1000 / ConfigHelper.Current.SafeVideoFps);
            var wait = 0;

            while (running)
            {
                Thread.Sleep(wait < 0 ? 0 : wait);

                var delay = Stopwatch.StartNew();
                var image = CaptureHelper.GetBitmap(rectangle, offset);
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
        private static bool Compare(GifFrame a, GifFrame b, int step = 1, int diff = 0)
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

#if __MonoCS__
        
        /// <summary>
        /// Old NGif encoder for Mono.
        /// </summary>
        private static void EncodeGif()
        {
            var stream = new MemoryStream();
            var gif = new NGif.AnimatedGifEncoder();

            gif.Start(stream);
            gif.SetQuality(Constants.GifQuality);
            gif.SetRepeat(0);

            while (running || frames.Count > 0)
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
            Stop();

            result.SetResult(stream);
        }

#else

        /// <summary>
        /// Start encoding GIF while waiting for new frames. If the capture process is still running, the algorithm
        /// waits for minimum 2 frames, then it will compares them. If they are the same, there is no need to save
        /// each, the first one gonna have an additional delay, while the second one will be deleted.
        /// </summary>
        private static void EncodeGif()
        {
            var stream = new MemoryStream();

            // Wait for the first frame
            while (!FrameExists(0)) { }

            using (var gif = new GifEncoder(stream, frames[0].Image.Width, frames[0].Image.Height, 0))
            {
                while (running || frames.Count > 0)
                {
                    // Check if it is possible to remove a frame
                    if (FrameExists(1) && Compare(frames[0], frames[1], Constants.GifCheckStep, Constants.GifMaxColorDiff))
                    {
                        frames[0].Delay += frames[1].Delay;
                        frames[1].Image.Dispose();
                        frames.RemoveAt(1);
                    }

                    // Save a frame to the Gif
                    // - if 0 and 1 are different
                    // - if there is one last frame left (and the capture process is stopped)
                    else if (FrameExists(1) || FrameExists(0) && !running)
                    {
                        gif.AddFrame(frames[0].Image, 0, 0, TimeSpan.FromMilliseconds(frames[0].Delay));
                        frames[0].Image.Dispose();
                        frames.RemoveAt(0);
                    }
                }
            }
            
            Stop();
            result.SetResult(stream);
        }

#endif

        /// <summary>
        /// Record Mpeg using FFmpeg. Sadly pipes are not seekable, so we have to use the file system as a destination
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

                    while (running || frames.Count > 0)
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
                if(File.Exists(temp))
                {
                    File.Delete(temp);
                }

                Stop();
            }
        }

        /// <summary>
        /// Start a recording with the given encoder.
        /// </summary>
        /// <param name="region"></param>
        /// <param name="offset"></param>
        /// <param name="encoder"></param>
        /// <returns></returns>
        private static Task<MemoryStream> Record(Rectangle region, Point offset, Action encoder)
        {
            if(result != null && !result.Task.IsCompleted)
            {
                return Task.FromResult<MemoryStream>(null);
            }

            running = true;
            result = new TaskCompletionSource<MemoryStream>();

            // Run the two process in the same time
            Task.Run(() => CaptureFrames(region, offset));
            Task.Run(encoder);

            return result.Task;
        }

        /// <summary>
        /// Record video into GIF.
        /// </summary>
        /// <param name="region"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        public static Task<MemoryStream> RecordGif(Rectangle region, Point offset)
        {
            return Record(region, offset, EncodeGif);
        }

        /// <summary>
        /// Record video into Mpeg.
        /// </summary>
        /// <param name="region"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        public static Task<MemoryStream> RecordMpeg(Rectangle region, Point offset)
        {
            return Record(region, offset, EncodeMpeg);
        }

        /// <summary>
        /// Request stop - EncodeGif will finish captured frames before it terminates.
        /// </summary>
        public static void Stop()
        {
            running = false;
        }
    }
}
