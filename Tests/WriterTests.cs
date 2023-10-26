using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WarcraftImageLabV2;
using WarcraftImageLabV2.ImageProcessing;
using WarcraftImageLabV2.ImageProcessing.Enums;
using WarcraftImageLabV2.ImageProcessing.Read;
using Xunit;

namespace Tests
{
    public class WriterTests
    {
        string outputDir;
        string file;

        public WriterTests()
        {
            outputDir = Path.Combine(Directory.GetCurrentDirectory(), "WriteTests");
            if (!Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            file = Path.Combine(Directory.GetCurrentDirectory(), "TestImages/LivingBomb.png");
        }

        [Theory]
        [InlineData(ImageFormatExportable.BLP)]
        [InlineData(ImageFormatExportable.BMP)]
        [InlineData(ImageFormatExportable.DDS)]
        [InlineData(ImageFormatExportable.JPG)]
        [InlineData(ImageFormatExportable.PNG)]
        [InlineData(ImageFormatExportable.TGA)]
        [InlineData(ImageFormatExportable.TIFF)]
        [InlineData(ImageFormatExportable.WEBP)]
        public void WriteImages(ImageFormatExportable format)
        {
            Settings settings = Settings.Load();
            settings.ImageFormat = format;

            string extension = "." + format.ToString().ToLower();
            string[] files = new string[] { file };
            string fileName = Path.GetFileNameWithoutExtension(file);
            
            Writer writer = new Writer(fileName, outputDir, files);
            writer.Write();
            string expectedPath = Path.Combine(outputDir, fileName) + extension;
            Bitmap image = Reader.ReadImageFile(expectedPath);

            Assert.True(File.Exists(expectedPath));
            Assert.NotNull(image);
        }
    }
}
