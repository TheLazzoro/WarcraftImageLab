using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO.Compression;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using WebPWrapper;
using SixLabors.ImageSharp.PixelFormats;
using War3Net.Drawing.Blp;
using BCnEncoder.Decoder;
using Svg;

namespace WarcraftImageLabV2.ImageProcessing
{
    internal static class Reader
    {
        private static BcDecoder bcDecoder = new BcDecoder();

        public static Bitmap ReadImageFile(string fullPath)
        {
            Bitmap image;

            string extension = fullPath.Split('.').Last().ToUpper();
            ImageFormat format = Enum.Parse<ImageFormat>(extension);
            switch (format)
            {
                case ImageFormat.JPG:
                    image = ReadLegacy(fullPath);
                    break;
                case ImageFormat.JPEG:
                    image = ReadLegacy(fullPath);
                    break;
                case ImageFormat.PNG:
                    image = ReadLegacy(fullPath);
                    break;
                case ImageFormat.DDS:
                    image = ReadDDS(fullPath);
                    break;
                case ImageFormat.BLP:
                    image = ReadBLP(fullPath);
                    break;
                case ImageFormat.TGA:
                    image = ReadTGA(fullPath);
                    break;
                case ImageFormat.BMP:
                    image = ReadLegacy(fullPath);
                    break;
                case ImageFormat.WEBP:
                    image = ReadWebP(fullPath);
                    break;
                case ImageFormat.TIFF:
                    image = ReadLegacy(fullPath);
                    break;
                case ImageFormat.SVG:
                    image = ReadSVG(fullPath);
                    break;
                case ImageFormat.CR2:
                    image = ReadCR2(fullPath);
                    break;
                default:
                    image = null;
                    break;
            }

            return image;
        }

        private static Bitmap ReadLegacy(string filePath)
        {
            using (FileStream fs = new FileStream(filePath, FileMode.Open))
            {
                Bitmap bitmap = new Bitmap(Image.FromStream(fs));
                return bitmap;
            }
        }

        private static Bitmap ReadTGA(string filePath)
        {
            SixLabors.ImageSharp.Image<Rgba32> image = SixLabors.ImageSharp.Image.Load<Rgba32>(filePath);

            return BitmapConverter.ToBitmap(image);
        }

        private static Bitmap ReadBLP(string filePath)
        {
            FileStream fs = File.OpenRead(filePath);
            BlpFile blpFile = new BlpFile(fs);
            int width;
            int height;
            // The library does not determine what's BLP1 and BLP2 properly, so we manually set bool bgra in GetPixels depending on the checkbox.
            byte[] bytes = blpFile.GetPixels(0, out width, out height, blpFile._isBLP2); // 0 indicates first mipmap layer. width and height are assigned width and height in GetPixels().
            var actualImage = blpFile.GetBitmapSource(0);
            int bytesPerPixel = (actualImage.Format.BitsPerPixel + 7) / 8;
            int stride = bytesPerPixel * actualImage.PixelWidth;

            // blp read and convert
            Bitmap image = new Bitmap(width, height);

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    var offset = (y * stride) + (x * bytesPerPixel);

                    byte red;
                    byte green;
                    byte blue;
                    byte alpha = 0;

                    red = bytes[offset + 0];
                    green = bytes[offset + 1];
                    blue = bytes[offset + 2];
                    alpha = bytes[offset + 3];

                    image.SetPixel(x, y, System.Drawing.Color.FromArgb(alpha, blue, green, red)); // assign color to pixel
                }
            }

            blpFile.Dispose();

            return image;
        }

        private static Bitmap ReadDDS(string filePath)
        {
            SixLabors.ImageSharp.Image<Rgba32> image = null;
            using FileStream fs = File.OpenRead(filePath);
            image = bcDecoder.Decode(fs);

            return BitmapConverter.ToBitmap(image);
        }

        private static Bitmap ReadCR2(string filePath)
        {
            int _bufferSize = 512 * 1024;
            byte[] _buffer = new byte[_bufferSize];
            ImageCodecInfo _jpgImageCodec = GetJpegCodec();


            Bitmap bitmap = null;

            FileStream fs = File.OpenRead(filePath);
            // Start address is at offset 0x62, file size at 0x7A, orientation at 0x6E
            fs.Seek(0x62, SeekOrigin.Begin);
            BinaryReader br = new BinaryReader(fs);
            UInt32 jpgStartPosition = br.ReadUInt32();  // 62
            br.ReadUInt32();  // 66
            br.ReadUInt32();  // 6A
            UInt32 orientation = br.ReadUInt32() & 0x000000FF; // 6E
            br.ReadUInt32();  // 72
            br.ReadUInt32();  // 76
            Int32 fileSize = br.ReadInt32();  // 7A

            fs.Seek(jpgStartPosition, SeekOrigin.Begin);

            var ps = new PartialStream(fs, jpgStartPosition, fileSize);
            bitmap = new Bitmap(ps);

            br.Close();
            ps.Close();
            fs.Close();

            try
            {
                if (_jpgImageCodec != null && (orientation == 8 || orientation == 6))
                {
                    if (orientation == 8)
                        bitmap.RotateFlip(RotateFlipType.Rotate270FlipNone);
                    else
                        bitmap.RotateFlip(RotateFlipType.Rotate90FlipNone);
                }
            }
            catch (Exception ex)
            {
                // Image Skipped
            }


            Bitmap bitmapCopy = new Bitmap(bitmap);

            return bitmapCopy;
        }

        private static ImageCodecInfo GetJpegCodec()
        {
            foreach (ImageCodecInfo c in ImageCodecInfo.GetImageEncoders())
            {
                if (c.CodecName.ToLower().Contains("jpeg")
                    || c.FilenameExtension.ToLower().Contains("*.jpg")
                    || c.FormatDescription.ToLower().Contains("jpeg")
                    || c.MimeType.ToLower().Contains("image/jpeg"))
                    return c;
            }

            return null;
        }

        private static Bitmap ReadWebP(string filePath)
        {
            using (WebP webp = new WebP())
            {
                Bitmap bitmap = webp.Load(filePath);
                return bitmap;
            }
        }

        private static Bitmap ReadSVG(string filePath)
        {
            var svgDocument = SvgDocument.Open(filePath);
            Bitmap bitmap = svgDocument.Draw();
            return bitmap;
        }
    }
}
