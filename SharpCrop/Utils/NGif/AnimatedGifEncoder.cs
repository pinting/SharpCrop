using System;
using System.Drawing;
using System.IO;

// ReSharper disable All

namespace SharpCrop.Utils.NGif
{
    /// <summary>
    /// Class AnimatedGifEncoder - Encodes a GIF file consisting of one or
    /// more frames.
    /// 
    /// No copyright asserted on the source code of this class.  May be used
    /// for any purpose, however, refer to the Unisys LZW patent for restrictions
    /// on use of the associated LZWEncoder class.  Please forward any corrections
    /// to kweiner@fmsware.com.
    /// 
    /// Author: Kevin Weiner, FM Software
    /// Version: 1.03 November 2003
    /// </summary>
    public class AnimatedGifEncoder
    {
        protected int width; // Image size
        protected int height;
        protected Color transparent = Color.Empty; // Transparent color if given
        protected int transIndex; // Transparent index in color table
        protected int repeat = -1; // No repeat
        protected int delay = 0; // Frame delay (hundredths)
        protected bool started = false; // Ready to output frames
        protected MemoryStream fs;
        protected Image image; // Current frame
        protected byte[] pixels; // BGR byte array from frame
        protected byte[] indexedPixels; // Converted frame indexed to palette
        protected int colorDepth; // Number of bit planes
        protected byte[] colorTab; // RGB palette
        protected bool[] usedEntry = new bool[256]; // Active palette entries
        protected int palSize = 7; // Color table size (bits-1)
        protected int dispose = -1; // Disposal code (-1 = use default)
        protected bool closeStream = false; // Close stream when finished
        protected bool firstFrame = true;
        protected bool sizeSet = false; // If false, get size from first frame
        protected int sample = 10; // Default sample interval for quantizer

        /// <summary>
        /// Sets the delay time between each frame, or changes it 
        /// for subsequent frames(applies to last frame added).
        /// </summary>
        /// <param name="ms">Delay time in milliseconds.</param>
        public void SetDelay(int ms)
        {
            delay = (int)Math.Round(ms / 10.0f);
        }

        /// <summary>
        /// Sets the GIF frame disposal code for the last added frame
        /// and any subsequent frames. Default is 0 if no transparent
        /// color has been set, otherwise 2.
        /// </summary>
        /// <param name="code">Disposal code.</param>
        public void SetDispose(int code)
        {
            if (code >= 0)
            {
                dispose = code;
            }
        }

        /// <summary>
        /// Sets the number of times the set of GIF frames
        /// should be played.  Default is 1; 0 means play
        /// indefinitely. Must be invoked before the first
        /// image is added.
        /// </summary>
        /// <param name="iter">Number of iterations.</param>
        public void SetRepeat(int iter)
        {
            if (iter >= 0)
            {
                repeat = iter;
            }
        }

        /// <summary>
        /// Sets the transparent color for the last added frame
        /// and any subsequent frames.
        /// Since all colors are subject to modification
        /// in the quantization process, the color in the final
        /// palette for each frame closest to the given color
        /// becomes the transparent color for that frame.
        /// May be set to null to indicate no transparent color.
        /// </summary>
        /// <param name="c">Color to be treated as transparent on display.</param>
        public void SetTransparent(Color c)
        {
            transparent = c;
        }

        /// <summary>
        /// Adds next GIF frame.  The frame is not written immediately, but is
        /// actually deferred until the next frame is received so that timing
        /// data can be inserted.  Invoking <code>finish()</code> flushes all
        /// frames.  If <code>setSize</code> was not invoked, the size of the
        /// first image is used for all subsequent frames.
        /// </summary>
        /// <param name="im">Image containing frame to write.</param>
        /// <returns>True if successful.</returns>
        public bool AddFrame(Image im)
        {
            if ((im == null) || !started)
            {
                return false;
            }

            bool ok = true;

            try
            {
                if (!sizeSet)
                {
                    // Use first frame's size
                    SetSize(im.Width, im.Height);
                }

                image = im;
                GetImagePixels(); // Convert to correct format if necessary
                AnalyzePixels(); // Build color table & map pixels

                if (firstFrame)
                {
                    WriteLSD(); // Logical screen descriptior
                    WritePalette(); // Global color table

                    if (repeat >= 0)
                    {
                        // Use NS app extension to indicate reps
                        WriteNetscapeExt();
                    }
                }

                WriteGraphicCtrlExt(); // Write graphic control extension
                WriteImageDesc(); // Image descriptor

                if (!firstFrame)
                {
                    WritePalette(); // Local color table
                }

                WritePixels(); // Encode and write pixel data
                firstFrame = false;
            }
            catch
            {
                ok = false;
            }

            return ok;
        }

        /// <summary>
        /// Flushes any pending data and closes output file.
        /// If writing to an OutputStream, the stream is not
        /// closed.
        /// </summary>
        /// <returns></returns>
        public bool Finish()
        {
            if (!started)
            {
                return false;
            }

            bool ok = true;
            started = false;

            try
            {
                fs.WriteByte(0x3b); // Gif trailer
                fs.Flush();

                if (closeStream)
                {
                    fs.Close();
                }
            }
            catch
            {
                ok = false;
            }

            // Reset for subsequent use
            transIndex = 0;
            fs = null;
            image = null;
            pixels = null;
            indexedPixels = null;
            colorTab = null;
            closeStream = false;
            firstFrame = true;

            return ok;
        }

        /// <summary>
        /// Sets frame rate in frames per second. Equivalent to setDelay(1000/fps)
        /// </summary>
        /// <param name="fps">Float frame rate (frames per second)</param>
        public void SetFrameRate(float fps)
        {
            if (fps != 0f)
            {
                delay = (int)Math.Round(100f / fps);
            }
        }

        /**
		 * Sets quality of color quantization (conversion of images
		 * to the maximum 256 colors allowed by the GIF specification).
		 * Lower values (minimum = 1) produce better colors, but slow
		 * processing significantly.  10 is the default, and produces
		 * good color mapping at reasonable speeds.  Values greater
		 * than 20 do not yield significant improvements in speed.
		 *
		 * @param quality int greater than 0.
		 * @return
		 */
        public void SetQuality(int quality)
        {
            if (quality < 1) quality = 1;
            sample = quality;
        }

        /**
		 * Sets the GIF frame size.  The default size is the
		 * size of the first frame added if this method is
		 * not invoked.
		 *
		 * @param w int frame width.
		 * @param h int frame width.
		 */
        public void SetSize(int w, int h)
        {
            if (started && !firstFrame) return;
            width = w;
            height = h;
            if (width < 1) width = 320;
            if (height < 1) height = 240;
            sizeSet = true;
        }

        /**
		 * Initiates GIF file creation on the given stream.  The stream
		 * is not closed automatically.
		 *
		 * @param os OutputStream on which GIF images are written.
		 * @return false if initial write failed.
		 */
        public bool Start(MemoryStream os)
        {
            if (os == null) return false;
            bool ok = true;
            closeStream = false;
            fs = os;
            try
            {
                WriteString("GIF89a"); // header
            }
            catch
            {
                ok = false;
            }
            return started = ok;
        }

        /**
		 * Analyzes image colors and creates color map.
		 */
        protected void AnalyzePixels()
        {
            int len = pixels.Length;
            int nPix = len / 3;
            indexedPixels = new byte[nPix];
            NeuQuant nq = new NeuQuant(pixels, len, sample);

            // Initialize quantizer

            colorTab = nq.Process(); // Create reduced palette

            // Convert map from BGR to RGB

            /*
			for (int i = 0; i < colorTab.Length; i += 3) 
			{
				byte temp = colorTab[i];
				colorTab[i] = colorTab[i + 2];
				colorTab[i + 2] = temp;
				usedEntry[i / 3] = false;
			}
            */

            // Map image pixels to new palette
            int k = 0;

            for (int i = 0; i < nPix; i++)
            {
                int index =
                    nq.Map(pixels[k++] & 0xff,
                    pixels[k++] & 0xff,
                    pixels[k++] & 0xff);

                usedEntry[index] = true;
                indexedPixels[i] = (byte)index;
            }

            pixels = null;
            colorDepth = 8;
            palSize = 7;

            // Get closest match to transparent color if specified
            if (transparent != Color.Empty)
            {
                transIndex = FindClosest(transparent);
            }
        }

        /// <summary>
        /// Returns index of palette color closest to c.
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        protected int FindClosest(Color c)
        {
            if (colorTab == null)
            {
                return -1;
            }

            int r = c.R;
            int g = c.G;
            int b = c.B;

            int minpos = 0;
            int dmin = 256 * 256 * 256;
            int len = colorTab.Length;

            for (int i = 0; i < len;)
            {
                int dr = r - (colorTab[i++] & 0xff);
                int dg = g - (colorTab[i++] & 0xff);
                int db = b - (colorTab[i] & 0xff);
                int d = dr * dr + dg * dg + db * db;
                int index = i / 3;

                if (usedEntry[index] && (d < dmin))
                {
                    dmin = d;
                    minpos = index;
                }

                i++;
            }

            return minpos;
        }

        /// <summary>
        /// Extracts image pixels into byte array "pixels"
        /// </summary>
        protected void GetImagePixels()
        {
            int w = image.Width;
            int h = image.Height;

            // int type = image.GetType().;

            if (w != width || h != height)
            {
                // create new image with right size/format
                Image temp = new Bitmap(width, height);
                Graphics g = Graphics.FromImage(temp);

                g.DrawImage(image, 0, 0);
                image = temp;
                g.Dispose();
            }

            // TODO: Improve performance, use unsafe code

            pixels = new Byte[3 * image.Width * image.Height];

            int count = 0;
            Bitmap tempBitmap = new Bitmap(image);

            for (int th = 0; th < image.Height; th++)
            {
                for (int tw = 0; tw < image.Width; tw++)
                {
                    Color color = tempBitmap.GetPixel(tw, th);
                    pixels[count] = color.R;
                    count++;
                    pixels[count] = color.G;
                    count++;
                    pixels[count] = color.B;
                    count++;
                }
            }

            // pixels = ((DataBufferByte) image.getRaster().getDataBuffer()).getData();
        }

        /// <summary>
        /// Writes Graphic Control Extension.
        /// </summary>
        protected void WriteGraphicCtrlExt()
        {
            fs.WriteByte(0x21); // Extension introducer
            fs.WriteByte(0xf9); // GCE label
            fs.WriteByte(4); // Data block size

            int transp, disp;

            if (transparent == Color.Empty)
            {
                transp = 0;
                disp = 0; // Dispose = no action
            }
            else
            {
                transp = 1;
                disp = 2; // Force clear if using transparent color
            }

            if (dispose >= 0)
            {
                disp = dispose & 7; // User override
            }

            disp <<= 2;

            // Packed fields
            fs.WriteByte(Convert.ToByte(
                0 | // 1:3 Reserved
                disp | // 4:6 Disposal
                0 | // 7 User input - 0 = none
                transp)); // 8 Transparency flag

            WriteShort(delay); // Delay x 1/100 sec
            fs.WriteByte(Convert.ToByte(transIndex)); // Transparent color index
            fs.WriteByte(0); // Block terminator
        }

        /// <summary>
        /// Writes Image Descriptor.
        /// </summary>
        protected void WriteImageDesc()
        {
            fs.WriteByte(0x2c); // Image separator
            WriteShort(0); // Image position x,y = 0,0
            WriteShort(0);
            WriteShort(width); // Image size
            WriteShort(height);

            // Packed fields
            if (firstFrame)
            {
                // No LCT  - GCT is used for first (or only) frame
                fs.WriteByte(0);
            }
            else
            {
                // Specify normal LCT
                fs.WriteByte(Convert.ToByte(
                    0x80 | // 1 Local color table  1=yes
                    0 | // 2 Interlace - 0=no
                    0 | // 3 Sorted - 0=no
                    0 | // 4-5 Reserved
                    palSize)); // 6-8 Size of color table
            }
        }

        /// <summary>
        /// Writes Logical Screen Descriptor.
        /// </summary>
        protected void WriteLSD()
        {
            // Logical screen size
            WriteShort(width);
            WriteShort(height);

            // Packed fields
            fs.WriteByte(Convert.ToByte(
                0x80 | // 1 Global color table flag = 1 (gct used)
                0x70 | // 2-4 Color resolution = 7
                0x00 | // 5 Gct sort flag = 0
                palSize)); // 6-8 Gct size

            fs.WriteByte(0); // Background color index
            fs.WriteByte(0); // Pixel aspect ratio - assume 1:1
        }

        /// <summary>
        /// Writes Netscape application extension to define repeat count.
        /// </summary>
        protected void WriteNetscapeExt()
        {
            fs.WriteByte(0x21); // Extension introducer
            fs.WriteByte(0xff); // App extension label
            fs.WriteByte(11); // Block size
            WriteString("NETSCAPE" + "2.0"); // App id + auth code
            fs.WriteByte(3); // Sub-block size
            fs.WriteByte(1); // Loop sub-block id
            WriteShort(repeat); // Loop count (extra iterations, 0=repeat forever)
            fs.WriteByte(0); // Block terminator
        }

        /// <summary>
        /// Writes color table.
        /// </summary>
        protected void WritePalette()
        {
            fs.Write(colorTab, 0, colorTab.Length);
            int n = (3 * 256) - colorTab.Length;
            for (int i = 0; i < n; i++)
            {
                fs.WriteByte(0);
            }
        }

        /// <summary>
        /// Encodes and writes pixel data.
        /// </summary>
        protected void WritePixels()
        {
            LZWEncoder encoder = new LZWEncoder(width, height, indexedPixels, colorDepth);
            encoder.Encode(fs);
        }

        /// <summary>
        /// Write 16-bit value to output stream, LSB first.
        /// </summary>
        /// <param name="value"></param>
        protected void WriteShort(int value)
        {
            fs.WriteByte(Convert.ToByte(value & 0xff));
            fs.WriteByte(Convert.ToByte((value >> 8) & 0xff));
        }

        /// <summary>
        /// Writes string to output stream.
        /// </summary>
        /// <param name="s"></param>
        protected void WriteString(String s)
        {
            char[] chars = s.ToCharArray();
            for (int i = 0; i < chars.Length; i++)
            {
                fs.WriteByte((byte)chars[i]);
            }
        }
    }

}