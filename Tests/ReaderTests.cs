using WarcraftImageLabV2.ImageProcessing;

namespace Tests
{
    public class ReaderTests
    {
        [Theory]
        [InlineData("6bf_blackrock_nova.blp")]
        [InlineData("ATCABTNCurse2.blp")]
        [InlineData("ATCCircleofRenewal.blp")]
        [InlineData("city_cliffdirt.dds")]
        [InlineData("ImageToConvert.png")]
        [InlineData("LivingBomb.png")]
        [InlineData("paladin-04e740dbc5882a8d358d086a88c960d18ac79c2a0583ad5843c1735e10eff231.svg")]
        [InlineData("Power.jpg")]
        [InlineData("RAW_CANON_1DM2.CR2")]
        public void ReadImages(string fileName)
        {
            string dir = Path.Combine(Directory.GetCurrentDirectory(), "TestImages");
            string fullPath = Path.Combine(dir, fileName);
            Reader.ReadImageFile(fullPath);
        }

        [Fact]
        public void ReadNonImage()
        {
            string dir = Path.Combine(Directory.GetCurrentDirectory(), "TestImages");
            string fullPath = Path.Combine(dir, "notAnImage.txt");

            Assert.Throws<ArgumentException>(() => Reader.ReadImageFile(fullPath));
        }
    }
}