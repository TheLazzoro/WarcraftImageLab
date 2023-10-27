using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WarcraftImageLab;
using WarcraftImageLab.ImageProcessing;
using WarcraftImageLab.ImageProcessing.Enums;
using WarcraftImageLab.ImageProcessing.Read;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Xml.Linq;

namespace Tests
{
    [TestClass]
    public class WriterTests
    {
        static string outputDir;
        static string file;

        [ClassInitialize]
        public static void Init(TestContext context)
        {
            outputDir = Path.Combine(Directory.GetCurrentDirectory(), "WriteTests");
            if (Directory.Exists(outputDir))
            {
                Directory.Delete(outputDir, true);
            }
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
            settings.KeepFilename = true;

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
