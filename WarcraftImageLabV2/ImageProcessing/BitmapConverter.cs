using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Runtime.InteropServices;
using System.Windows;
using System.Drawing;
using System.Drawing.Imaging;

namespace WarcraftImageLabV2.ImageProcessing
{
    internal class BitmapConverter
    {
        internal static Bitmap ToBitmap(Image<Rgba32> image)
        {
            Stream stream = new MemoryStream();
            // we need an encoder to preserve transparency.
            SixLabors.ImageSharp.Formats.Bmp.BmpEncoder bmpEncoder = new SixLabors.ImageSharp.Formats.Bmp.BmpEncoder
            {
                BitsPerPixel = SixLabors.ImageSharp.Formats.Bmp.BmpBitsPerPixel.Pixel32, // bitmap transparency needs 32 bits per pixel before we set transparency support.
                SupportTransparency = true,
            };
            image.SaveAsBmp(stream, bmpEncoder);

            return new Bitmap(stream);
        }

        internal static BitmapSource ToBitmapSource(Bitmap bitmap)
        {
            var bitmapData = bitmap.LockBits(
                new System.Drawing.Rectangle(0, 0, bitmap.Width, bitmap.Height),
                ImageLockMode.ReadOnly, bitmap.PixelFormat);

            var bitmapSource = BitmapSource.Create(
                bitmapData.Width, bitmapData.Height,
                bitmap.HorizontalResolution, bitmap.VerticalResolution,
                PixelFormats.Bgra32, null,
                bitmapData.Scan0, bitmapData.Stride * bitmapData.Height, bitmapData.Stride);

            bitmap.UnlockBits(bitmapData);

            return bitmapSource;
        }

        internal static Bitmap BitmapSourceToBitmap(BitmapSource srs, System.Drawing.Imaging.PixelFormat format = System.Drawing.Imaging.PixelFormat.Format32bppPArgb)
        {
            var width = srs.PixelWidth;
            var height = srs.PixelHeight;
            var stride = width * ((srs.Format.BitsPerPixel + 7) / 8);
            var memoryBlockPointer = Marshal.AllocHGlobal(height * stride);
            srs.CopyPixels(new Int32Rect(0, 0, width, height), memoryBlockPointer, height * stride, stride);
            var bitmap = new Bitmap(width, height, stride, format, memoryBlockPointer);
            return bitmap;
        }

        internal static Image<Rgba32> ToImageSharpImage(System.Drawing.Bitmap bitmap)
        {
            using (var memoryStream = new MemoryStream())
            {
                bitmap.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Png);

                memoryStream.Seek(0, SeekOrigin.Begin);

                return SixLabors.ImageSharp.Image.Load<Rgba32>(memoryStream);
            }
        }
    }
}
