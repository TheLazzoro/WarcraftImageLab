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
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Xml.Linq;

namespace Tests
{
    [TestClass]
    public class WriterTests
    {
        string outputDir;
        string file;

        [TestInitialize]
        public void BeforeEach()
        {
            outputDir = Path.Combine(Directory.GetCurrentDirectory(), "WriteTests");
            if (!Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            file = Path.Combine(Directory.GetCurrentDirectory(), "TestImages/LivingBomb.png");
        }

        [DataTestMethod]
        [DataRow(ImageFormatExportable.BLP)]
        [DataRow(ImageFormatExportable.BMP)]
        [DataRow(ImageFormatExportable.DDS)]
        [DataRow(ImageFormatExportable.JPG)]
        [DataRow(ImageFormatExportable.PNG)]
        [DataRow(ImageFormatExportable.TGA)]
        [DataRow(ImageFormatExportable.TIFF)]
        [DataRow(ImageFormatExportable.WEBP)]
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

            Assert.IsTrue(File.Exists(expectedPath));
            Assert.IsNotNull(image);
        }
    }
}
