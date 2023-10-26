using SixLabors.ImageSharp.Formats.Tga;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WarcraftImageLabV2.ImageProcessing.Read;
using BCnEncoder.Encoder;
using BCnEncoder.Shared;
using System.Windows.Media.Imaging;
using System.Windows.Markup;
using WarcraftImageLabV2.ImageProcessing.Enums;
using ImageFormat = WarcraftImageLabV2.ImageProcessing.Enums.ImageFormat;

namespace WarcraftImageLabV2.ImageProcessing
{
    public class Writer
    {
        public int progress { get; private set; } = 0;
        public event Action<string, string> OnError;
        public event Action<string> OnFileConverted;
        public event Action OnComplete;

        private Settings settings;
        private string outputFileName;
        private string outputDir;
        private string[] files;
        private bool keepFilenames;
        private bool cancel;

        private ImageCodecInfo jpgEncoder;
        private EncoderParameters encoderParameters;
        private BcEncoder bcEncoder;

        /// <param name="outputFileName">File name. Not full output path.</param>
        /// <param name="files"></param>
        public Writer(string outputFileName, string outputDir, string[] files)
        {
            this.outputFileName = outputFileName;
            this.files = files;
            this.outputDir = outputDir;

            settings = Settings.Load();
            keepFilenames = settings.KeepFilename;

            #region JPEG encoder
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageEncoders();
            foreach (ImageCodecInfo codec in codecs)
            {
                if (codec.FormatID == System.Drawing.Imaging.ImageFormat.Jpeg.Guid)
                {
                    jpgEncoder = codec;
                }
            }
            System.Drawing.Imaging.Encoder myEncoder = System.Drawing.Imaging.Encoder.Quality;
            encoderParameters = new EncoderParameters(1);
            var encoderParameter = new EncoderParameter(myEncoder, settings.QualityJPG);
            encoderParameters.Param[0] = encoderParameter;
            #endregion

            #region DDS encoder
            bcEncoder = new BcEncoder();
            bcEncoder.Options.multiThreaded = true;
            bcEncoder.OutputOptions.generateMipMaps = settings.GenerateMipmapsDDS;
            bcEncoder.OutputOptions.fileFormat = OutputFileFormat.Dds;

            switch (settings.CompressionDDS)
            {
                case CompressionDDS.BC1:
                    bcEncoder.OutputOptions.format = CompressionFormat.BC1;
                    break;
                case CompressionDDS.BC1a:
                    bcEncoder.OutputOptions.format = CompressionFormat.BC1WithAlpha;
                    break;
                case CompressionDDS.BC2:
                    bcEncoder.OutputOptions.format = CompressionFormat.BC2;
                    break;
                case CompressionDDS.BC3:
                    bcEncoder.OutputOptions.format = CompressionFormat.BC3;
                    break;
                default:
                    bcEncoder.OutputOptions.format = CompressionFormat.BC1;
                    break;
            }

            switch (settings.QualityDDS)
            {
                case QualityDDS.Fastest:
                    bcEncoder.OutputOptions.quality = CompressionQuality.Fast;
                    break;
                case QualityDDS.Balanced:
                    bcEncoder.OutputOptions.quality = CompressionQuality.Balanced;
                    break;
                case QualityDDS.Highest:
                    bcEncoder.OutputOptions.quality = CompressionQuality.BestQuality;
                    break;
                default:
                    bcEncoder.OutputOptions.quality = CompressionQuality.Balanced;
                    break;
            }
            #endregion

        }

        public void Cancel()
        {
            cancel = true;
        }

        public void Write()
        {
            Settings settings = Settings.Load();
            string extension = "." + settings.ImageFormat.ToString().ToLower();
            ImageFormat format;
            for (int i = 0; i < files.Length; i++)
            {
                if (cancel)
                    break;

                string fullPath = files[i];
                string outputFileName = this.outputFileName;
                if(keepFilenames)
                {
                    outputFileName = Path.GetFileNameWithoutExtension(fullPath);
                }
                
                try
                {
                    Bitmap image = Reader.ReadImageFile(fullPath);
                    Processor processor = new Processor(outputFileName);
                    var processedImages = processor.ApplyFilters(image);
                    for (int j = 0; j < processedImages.Count; j++)
                    {
                        var processedImage = processedImages[j];
                        Bitmap imageToSave = processedImage.Image;
                        string fileName = string.Empty;
                        if(keepFilenames)
                            fileName = processedImage.FileName;
                        else
                            fileName = processedImage.FileName + progress;

                        string outputPath = Path.Combine(outputDir, fileName) + extension;
                        Console.WriteLine(outputPath);

                        switch (settings.ImageFormat)
                        {
                            case ImageFormatExportable.JPG:
                                WriteJPG(imageToSave, outputPath);
                                break;
                            case ImageFormatExportable.PNG:
                                WritePng(imageToSave, outputPath);
                                break;
                            case ImageFormatExportable.DDS:
                                WriteDds(imageToSave, outputPath);
                                break;
                            case ImageFormatExportable.BLP:
                                WriteBLP(imageToSave, outputPath);
                                break;
                            case ImageFormatExportable.TGA:
                                WriteTga(imageToSave, outputPath);
                                break;
                            case ImageFormatExportable.BMP:
                                WriteBmp(imageToSave, outputPath);
                                break;
                            case ImageFormatExportable.WEBP:
                                WriteWebP(imageToSave, outputPath);
                                break;
                            case ImageFormatExportable.TIFF:
                                WriteTiff(imageToSave, outputPath);
                                break;
                        }

                        OnFileConverted?.Invoke(fullPath);
                    }
                }
                catch (Exception ex)
                {
                    OnError?.Invoke(fullPath, ex.Message);
                }

                progress++;
            }

            OnComplete?.Invoke();
        }

        private void WriteJPG(Bitmap image, string outputPath)
        {
            image.Save(outputPath, jpgEncoder, encoderParameters);
        }

        private void WritePng(Bitmap image, string outputPath)
        {
            image.Save(outputPath, System.Drawing.Imaging.ImageFormat.Png);
        }

        private void WriteBmp(Bitmap image, string outputPath)
        {
            image.Save(outputPath, System.Drawing.Imaging.ImageFormat.Bmp);
        }

        private void WriteTiff(Bitmap imageToConvert, string outputPath)
        {
            imageToConvert.Save(outputPath, System.Drawing.Imaging.ImageFormat.Tiff);
        }

        private void WriteTga(Bitmap imageToConvert, string outputPath)
        {
            Image<Rgba32> img = BitmapConverter.ToImageSharpImage(imageToConvert);
            TgaEncoder tgaEncoder = new TgaEncoder();
            tgaEncoder.Compression = TgaCompression.None;
            tgaEncoder.BitsPerPixel = TgaBitsPerPixel.Pixel32;
            img.SaveAsTga(outputPath, tgaEncoder);
        }

        private void WriteDds(Bitmap imageToConvert, string outputPath)
        {
            using (FileStream fs = File.OpenWrite(outputPath))
            {
                Image<Rgba32> img = BitmapConverter.ToImageSharpImage(imageToConvert);
                bcEncoder.Encode(img, fs);
            }
        }

        private void WriteWebP(Bitmap imageToConvert, string outputPath)
        {
            using (WebP webp = new WebP())
            {
                webp.Save(imageToConvert, outputPath, settings.QualityWebP);
            }
        }

        /// <summary>
        /// TODO: 
        /// UNFINISHED. Currently writes standard JPEG YCbCr data into the stream.
        /// War3 uses 8 bit BGRA colors for BLP images.
        /// Additionally, the header size and size of each JPEG image are not yet determined.
        /// 
        /// EDIT:
        /// I didn't have patience to figure out how BLP works.
        /// Instead, we're using BLPLAB's command line tool to write BLP files.
        /// </summary>
        private void WriteBLP(Bitmap imageToConvert, string outputPath)
        {
            string tmpPath = Directory.GetCurrentDirectory() + "/tmpFile.tga";
            WriteTga(imageToConvert, tmpPath);

            string blpPath = Path.Combine(Directory.GetCurrentDirectory(), "Resources/BLPLAB/blplabcl.exe");
            Process p = new Process();
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.CreateNoWindow = true;
            startInfo.FileName = blpPath;

            int type = (int)settings.BlpType;
            int q = settings.BlpType == BlpType.Compressed ? settings.BlpQuality : settings.BlpPalettedColors;
            int mm = settings.BlpMipmapCount;
            string opt1 = string.Empty;
            string opt2 = string.Empty;
            if(
                (settings.BlpType == BlpType.Compressed && settings.BlpMergeHeaders) ||
                (settings.BlpType == BlpType.Paletted && settings.BlpCompressPalette)
                )
            {
                opt1 = "-opt1";
            }
            if(
                (settings.BlpType == BlpType.Compressed && settings.BlpProgressiveEncoding) ||
                (settings.BlpType == BlpType.Paletted && settings.BlpErrorDiffusion)
                )
            {
                opt2 = "-opt2";
            }

            startInfo.Arguments = $"\"{tmpPath}\" \"{outputPath}\" -type{type} -q{q} -mm{mm} {opt1} {opt2}";
            p.StartInfo = startInfo;
            p.Start();
            p.WaitForExit();
            p.Kill();

            return;

            // --- BELOW WAS EXPERIMENTAL --- //

            // --- DEFINE FILE CONTENTS --- //

            int magic = 0x31504c42; // BLP1
            int formatVersion = 0; // JPEG compression, put 1 for DirectX compression/uncompressed (Palettized).
            int alphaDepth = 8; // Should be 0, 1, 4, or 8, and default to 0 if invalid.
            int colorEncoding = formatVersion == 0 ? 0 : 1;
            int width = imageToConvert.Width;
            int height = imageToConvert.Height;
            // flag for alpha channel and team colors (usually 3, 4 or 5)
            // 3 and 4 means color and alpha information for paletted files
            // 5 means only color information, if >=5 on 'unit' textures, it won't show the team color.
            uint extra = 3;
            uint hasMipMaps = 1; // 1: true, 0: false?

            const int mipMaps = 16;
            int mipMapCount = 0; // should not be saved
            uint[] mipMapOffset = new uint[mipMaps];
            for (int i = 0; i < mipMaps; i++) // user should be able to set own mipmap count.
            {
                // Define offset for each mipmap here.

                if (mipMapOffset[i] != 0)
                    mipMapCount++;
                else
                    break;
            }
            uint[] mipMapSizes = new uint[mipMaps];
            for (int i = 0; i < mipMaps; i++)
            {
                // Define size for each mipmap here.
                // I assume the size is equal to the space between each offset.
            }

            /// Below comment is from War3Net:
            // When encoding is 1, there is no image compression and we have to read a color palette.
            // This palette always exists when the formatVersion is set to FileContent.Direct, even when it's not used.

            if (colorEncoding == 1)
            {
                const int paletteSize = 256;
                uint[] colorPalette = new uint[paletteSize];

                // 
            }
            else //JPG
            {

            }


            // --- WRITE FILE CONTENTS --- //
            Stream file = new MemoryStream();
            StreamWriter writer = new StreamWriter(file);
            using (FileStream fs = new FileStream(outputPath, FileMode.Create))
            {
                fs.Write(BitConverter.GetBytes(magic), 0, 4);
                fs.Write(BitConverter.GetBytes(formatVersion), 0, 4);
                fs.Write(BitConverter.GetBytes(alphaDepth), 0, 4);
                fs.Write(BitConverter.GetBytes(width), 0, 4);
                fs.Write(BitConverter.GetBytes(height), 0, 4);
                fs.Write(BitConverter.GetBytes(extra), 0, 4);
                fs.Write(BitConverter.GetBytes(hasMipMaps), 0, 4);
                for (int i = 0; i < mipMaps; i++)
                {
                    writer.Write(mipMapOffset[i]);
                }
                for (int i = 0; i < mipMaps; i++)
                {
                    writer.Write(mipMapSizes[i]);
                }
                if (colorEncoding == 1) // Direct
                {
                    /* This type of encoding only has 256 colors in the encoded palette.
                     * As a result, image quality will drop significantly in complex images.
                     * This is unsupported for now.
                     */
                }
                else // JPG
                {
                    ImageCodecInfo info = null;
                    ImageCodecInfo[] codecs = ImageCodecInfo.GetImageEncoders();
                    foreach (ImageCodecInfo codec in codecs)
                    {
                        if (codec.FormatID == System.Drawing.Imaging.ImageFormat.Jpeg.Guid)
                        {
                            info = codec;
                        }
                    }
                    System.Drawing.Imaging.Encoder myEncoder = System.Drawing.Imaging.Encoder.Quality;
                    EncoderParameters parameters = new EncoderParameters(1);
                    int quality = 100;
                    var encoderParameter = new EncoderParameter(myEncoder, quality);
                    parameters.Param[0] = encoderParameter;

                    Stream stream = new MemoryStream();
                    imageToConvert.Save(stream, info, parameters);

                    // testing mipmaps
                    Bitmap mip1 = new Bitmap(imageToConvert, new System.Drawing.Size(64, 64));
                    Bitmap mip2 = new Bitmap(imageToConvert, new System.Drawing.Size(32, 32));

                    BitmapSource bmp = BitmapConverter.ToBitmapSource(imageToConvert);
                    BitmapSource bmp1 = BitmapConverter.ToBitmapSource(mip1);
                    BitmapSource bmp2 = BitmapConverter.ToBitmapSource(mip2);

                    BitmapFrame f0 = BitmapFrame.Create(bmp);
                    BitmapFrame f1 = BitmapFrame.Create(bmp1);
                    BitmapFrame f2 = BitmapFrame.Create(bmp2);
                    List<BitmapFrame> frames = new List<BitmapFrame>
                    {
                        f0,
                        f1,
                        f2
                    };

                    var jpgEncoder = new JpegBitmapEncoder();
                    jpgEncoder.Frames = frames;

                    // define mipmap offset and sizes
                    uint offset = (uint)fs.Length;
                    for (int i = 0; i < frames.Count; i++)
                    {
                        mipMapOffset[i] = offset;
                        mipMapSizes[i] = (uint)frames[i].PixelWidth * (uint)frames[i].PixelHeight;
                        offset += mipMapSizes[i];
                    }
                    for (int i = 0; i < mipMaps; i++)
                    {
                        fs.Write(BitConverter.GetBytes(mipMapOffset[i]), 0, 4);
                    }
                    for (int i = 0; i < mipMaps; i++)
                    {
                        fs.Write(BitConverter.GetBytes(mipMapSizes[i]), 0, 4);
                    }


                    jpgEncoder.Save(fs);
                }
            }
        }
    }
}
