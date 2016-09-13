using SharpCrop.Models;
using SharpCrop.Utils.BumpKit;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace SharpCrop.Utils
{
    public static class GifFactory
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
        private static void CaptureFrames(Rectangle rectangle, Point offset)
        {
            var freq = (1000 / ConfigHelper.Memory.SafeGifSpeed);
            var wait = 0;

            while (running)
            {
                Thread.Sleep(wait < 0 ? 0 : wait);

                var delay = Stopwatch.StartNew();
                var image = CaptureHelper.GetBitmap(rectangle, offset);

                delay.Stop();

                wait = freq - (int)delay.ElapsedMilliseconds;

                frames.Add(new GifFrame()
                {
                    Image = image,
                    Delay = freq + (wait < 0 ? -1 * wait : 0)
                });
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
            gif.SetRepeat(ConfigHelper.Memory.GifRepeat ? 0 : 1);

            while (running || frames.Count > 0)
            {
                if (!FrameExists(0))
                {
                    continue;
                }

                gif.AddFrame(frames[0].Image);
                frames.RemoveAt(0);
            }

            gif.SetDelay(1000 / ConfigHelper.Memory.SafeGifFps);
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
            // Wait for the first frame
            while (!FrameExists(0)) { }

            var stream = new MemoryStream();
            var gif = new GifEncoder(stream, frames[0].Image.Width, frames[0].Image.Height, ConfigHelper.Memory.NoGifRepeat ? 1 : 0);
            
            while(running || frames.Count > 0)
            {
                // Check if it is possible to remove a frame
                if (FrameExists(1) && Compare(frames[0], frames[1], Constants.GifCheckStep, Constants.GifMaxColorDiff))
                {
                    frames[0].Delay += frames[1].Delay;
                    frames.RemoveAt(1);
                }

                // Save a frame to the Gif
                // - if 0 and 1 are different
                // - if there is one last frame left (and the capture process is stopped)
                else if (FrameExists(1) || FrameExists(0) && !running)
                {
                    gif.AddFrame(frames[0].Image, 0, 0, TimeSpan.FromMilliseconds(frames[0].Delay));
                    frames.RemoveAt(0);
                }
            }

            gif.Dispose();
            Stop();

            result.SetResult(stream);
        }

#endif

        /// <summary>
        /// Start a recording with a rectangle.
        /// </summary>
        /// <param name="rectrectangle"></param>
        /// <returns></returns>
        public static Task<MemoryStream> Record(Rectangle rectrectangle, Point offset)
        {
            if(result != null && !result.Task.IsCompleted)
            {
                return Task.FromResult<MemoryStream>(null);
            }

            running = true;
            result = new TaskCompletionSource<MemoryStream>();

            // Run the two process in the same time
            Task.Run(() => CaptureFrames(rectrectangle, offset));
            Task.Run(() => EncodeGif());

            return result.Task;
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
