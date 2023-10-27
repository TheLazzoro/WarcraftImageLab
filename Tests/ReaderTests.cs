using Microsoft.VisualStudio.TestTools.UnitTesting;
using WarcraftImageLab.ImageProcessing.Read;

namespace Tests
{
    [TestClass]
    public class ReaderTests
    {
        [DataTestMethod]
        [DataRow("6bf_blackrock_nova.blp")]
        [DataRow("ATCABTNCurse2.blp")]
        [DataRow("ATCCircleofRenewal.blp")]
        [DataRow("city_cliffdirt.dds")]
        [DataRow("ImageToConvert.png")]
        [DataRow("LivingBomb.png")]
        [DataRow("paladin-04e740dbc5882a8d358d086a88c960d18ac79c2a0583ad5843c1735e10eff231.svg")]
        [DataRow("Power.jpg")]
        [DataRow("RAW_CANON_1DM2.CR2")]
        public void ReadImages(string fileName)
        {
            string dir = Path.Combine(Directory.GetCurrentDirectory(), "TestImages");
            string fullPath = Path.Combine(dir, fileName);
            Reader.ReadImageFile(fullPath);
        }

        [TestMethod]
        public void ReadNonImage()
        {
            string dir = Path.Combine(Directory.GetCurrentDirectory(), "TestImages");
            string fullPath = Path.Combine(dir, "notAnImage.txt");

            Assert.ThrowsException<Exception>(() => Reader.ReadImageFile(fullPath));
        }
    }
}