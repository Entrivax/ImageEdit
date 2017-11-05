using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace ImageEdit
{
    public class LockBitmap : IDisposable, IImage
    {
        private readonly Bitmap source;
        private IntPtr iptr = IntPtr.Zero;
        private BitmapData bitmapData;

        private bool locked = false;
        private bool unlocked = false;
        private static readonly object lockObject = new object();
        private static readonly object unlockObject = new object();

        public byte[] Pixels { get; set; }
        public int Depth { get; private set; }
        public int Width { get; private set; }
        public int Height { get; private set; }

        public LockBitmap(Bitmap source)
        {
            this.source = source;
            LockBits();
        }

        /// <summary>
        /// Lock bitmap data
        /// </summary>
        public void LockBits()
        {
            try
            {
                // double-checked lock
                if (locked)
                {
                    return;
                }

                lock (lockObject)
                {
                    if (locked)
                    {
                        return;
                    }
                    // Get width and height of bitmap
                    Width = source.Width;
                    Height = source.Height;

                    // get total locked pixels count
                    var pixelCount = Width * Height;

                    // Create rectangle to lock
                    var rect = new Rectangle(0, 0, Width, Height);

                    // get source bitmap pixel format size
                    Depth = Image.GetPixelFormatSize(source.PixelFormat);

                    // Check if bpp (Bits Per Pixel) is 8, 24, or 32
                    if (Depth != 8 && Depth != 24 && Depth != 32)
                    {
                        throw new ArgumentException("Only 8, 24 and 32 bpp images are supported.");
                    }

                    // Lock bitmap and return bitmap data
                    bitmapData = source.LockBits(rect, ImageLockMode.ReadWrite,
                                                 source.PixelFormat);

                    // create byte array to copy pixel values
                    var step = Depth / 8;
                    Pixels = new byte[pixelCount * step];
                    iptr = bitmapData.Scan0;

                    // Copy data from pointer to array
                    Marshal.Copy(iptr, Pixels, 0, Pixels.Length);
                    locked = true;
                }
            }
            catch
            {
                throw;
            }

        }

        /// <summary>
        /// Unlock bitmap data
        /// </summary>
        public void UnlockBits()
        {
            try
            {
                if (unlocked)
                {
                    return;
                }
                lock (unlockObject)
                {
                    if (unlocked)
                    {
                        return;
                    }
                    // Copy data from byte array to pointer
                    Marshal.Copy(Pixels, 0, iptr, Pixels.Length);

                    // Unlock bitmap data
                    source.UnlockBits(bitmapData);
                    unlocked = true;
                }
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Get the color of the specified pixel
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public Color GetPixel(int x, int y)
        {
            var clr = Color.Empty;

            // Get color components count
            var cCount = Depth / 8;

            // Get start index of the specified pixel
            var i = ((y * Width) + x) * cCount;

            if (i > Pixels.Length - cCount)
                throw new IndexOutOfRangeException();

            if (Depth == 32) // For 32 BPP get Red, Green, Blue and Alpha
            {
                var b = Pixels[i];
                var g = Pixels[i + 1];
                var r = Pixels[i + 2];
                var a = Pixels[i + 3]; // a
                clr = Color.FromArgb(a, r, g, b);
            }

            if (Depth == 24) // For 24 BPP get Red, Green and Blue
            {
                var b = Pixels[i];
                var g = Pixels[i + 1];
                var r = Pixels[i + 2];
                clr = Color.FromArgb(r, g, b);
            }

            // For 8 BPP get color value (Red, Green and Blue values are the same)
            if (Depth == 8)
            {
                var c = Pixels[i];
                clr = Color.FromArgb(c, c, c);
            }
            return clr;
        }

        /// <summary>
        /// Set the color of the specified pixel
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="color"></param>
        public void SetPixel(int x, int y, Color color)
        {
            // Get color components count
            var cCount = Depth / 8;

            // Get start index of the specified pixel
            var i = ((y * Width) + x) * cCount;

            if (Depth == 32) // For 32 BPP set Red, Green, Blue and Alpha
            {
                Pixels[i] = color.B;
                Pixels[i + 1] = color.G;
                Pixels[i + 2] = color.R;
                Pixels[i + 3] = color.A;
            }

            if (Depth == 24) // For 24 BPP set Red, Green and Blue
            {
                Pixels[i] = color.B;
                Pixels[i + 1] = color.G;
                Pixels[i + 2] = color.R;
            }

            // For 8 BPP set color value (Red, Green and Blue values are the same)
            if (Depth == 8)
            {
                Pixels[i] = color.B;
            }
        }

        public void Dispose()
        {
            UnlockBits();
        }

        public Color Get(float x, float y)
        {
            var premultipliedX = x * (Width - 1);
            var premultipliedY = y * (Height - 1);
            while(premultipliedX < 0)
            {
                premultipliedX += Width;
            }
            while (premultipliedX >= Width)
            {
                premultipliedX -= Width;
            }
            while (premultipliedY < 0)
            {
                premultipliedY += Height;
            }
            while (premultipliedY >= Height)
            {
                premultipliedY -= Height;
            }
            var x1 = (int)premultipliedX;
            var y1 = (int)premultipliedY;
            var percentX = premultipliedX - x1;
            var percentY = premultipliedY - y1;
            var x2 = (x1 + 1) >= Width ? x1 + 1 - Width : x1 + 1;
            var y2 = (y1 + 1) >= Height ? y1 + 1 - Height : y1 + 1;

            var x1y1 = GetPixel(x1, y1);
            var x2y1 = GetPixel(x2, y1);
            var x1y2 = GetPixel(x1, y2);
            var x2y2 = GetPixel(x2, y2);

            return Color.FromArgb((byte)Clamp((x1y1.A * (1 - percentX) * (1 - percentY) + x2y1.A * (percentX) * (1 - percentY)
                            + x1y2.A * (1 - percentX) * (percentY) + x2y2.A * (percentX) * (percentY)), 0, 255),
                            (byte)Clamp((x1y1.R * (1 - percentX) * (1 - percentY) + x2y1.R * (percentX) * (1 - percentY)
                            + x1y2.R * (1 - percentX) * (percentY) + x2y2.R * (percentX) * (percentY)), 0, 255),
                            (byte)Clamp((x1y1.G * (1 - percentX) * (1 - percentY) + x2y1.G * (percentX) * (1 - percentY)
                            + x1y2.G * (1 - percentX) * (percentY) + x2y2.G * (percentX) * (percentY)), 0, 255),
                            (byte)Clamp((x1y1.B * (1 - percentX) * (1 - percentY) + x2y1.B * (percentX) * (1 - percentY)
                            + x1y2.B * (1 - percentX) * (percentY) + x2y2.B * (percentX) * (percentY)), 0, 255));
        }

        private float Clamp(float value, float min, float max)
        {
            if (value < min)
                return min;
            if (value > max)
                return max;
            return value;
        }
    }
}
