﻿using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Windows.Media;

namespace WarcraftImageLabV2.ImageProcessing
{
    internal class BitmapConverter
    {
        internal static System.Drawing.Bitmap ToBitmap(SixLabors.ImageSharp.Image<Rgba32> image)
        {
            Stream stream = new MemoryStream();
            // we need an encoder to preserve transparency.
            SixLabors.ImageSharp.Formats.Bmp.BmpEncoder bmpEncoder = new SixLabors.ImageSharp.Formats.Bmp.BmpEncoder
            {
                BitsPerPixel = SixLabors.ImageSharp.Formats.Bmp.BmpBitsPerPixel.Pixel32, // bitmap transparency needs 32 bits per pixel before we set transparency support.
                SupportTransparency = true,
            }; 
            image.SaveAsBmp(stream, bmpEncoder);

            return new System.Drawing.Bitmap(stream);
        }

        internal static BitmapSource ToBitmapSource(System.Drawing.Bitmap bitmap)
        {
            var bitmapData = bitmap.LockBits(
                new System.Drawing.Rectangle(0, 0, bitmap.Width, bitmap.Height),
                System.Drawing.Imaging.ImageLockMode.ReadOnly, bitmap.PixelFormat);

            var bitmapSource = BitmapSource.Create(
                bitmapData.Width, bitmapData.Height,
                bitmap.HorizontalResolution, bitmap.VerticalResolution,
                PixelFormats.Bgr32, null,
                bitmapData.Scan0, bitmapData.Stride * bitmapData.Height, bitmapData.Stride);

            bitmap.UnlockBits(bitmapData);

            return bitmapSource;
        }
    }
}