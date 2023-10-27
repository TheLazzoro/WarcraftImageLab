using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WarcraftImageLabV2.ImageProcessing.Enums;

namespace WarcraftImageLabV2
{
    public class Settings
    {
        private static Settings instance;
        private static readonly string filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Warcraft Image Lab/settings.json");

        #region Window settings

        public int WindowX = 100;
        public int WindowY = 100;
        public int WindowWidth = 1000;
        public int WindowHeight = 600;

        #endregion

        #region Filter settings

        [JsonIgnore]
        public BorderModeEnum BorderMode = BorderModeEnum.None;
        [JsonIgnore]
        public bool BorderBTN = false;
        [JsonIgnore]
        public bool BorderPAS = false;
        [JsonIgnore]
        public bool BorderATC = false;
        [JsonIgnore]
        public bool BorderInfocard = false;
        [JsonIgnore]
        public bool BorderInfocardUpgrade = false;
        [JsonIgnore]
        public bool BorderDISBTN = false;
        [JsonIgnore]
        public bool BorderDISPAS = false;
        [JsonIgnore]
        public bool BorderDISATC = false;

        [JsonIgnore]
        public bool Resize = false;
        [JsonIgnore]
        public int WidthNew = 16;
        [JsonIgnore]
        public int HeightNew = 16;

        #endregion

        #region Export settings

        public ImageFormatExportable ImageFormat = ImageFormatExportable.JPG;
        public int QualityJPG = 5;
        public int QualityWebP = 5;
        public CompressionDDS CompressionDDS = CompressionDDS.BC1;
        public QualityDDS QualityDDS = QualityDDS.Balanced;
        public bool GenerateMipmapsDDS = true;
        public BlpType BlpType = BlpType.Compressed;
        public int BlpQuality = 80;
        public int BlpPalettedColors = 256;
        public bool BlpMergeHeaders = true;
        public bool BlpProgressiveEncoding = false;
        public bool BlpCompressPalette = true;
        public bool BlpErrorDiffusion = false;
        public int BlpMipmapCount = 1;

        public bool KeepFilename = false;

        #endregion


        private Settings() { }

        public static Settings Load()
        {
            if (instance != null)
                return instance;

            Settings settings;
            if (File.Exists(filePath))
            {
                var file = File.ReadAllText(filePath);
                settings = JsonConvert.DeserializeObject<Settings>(file);
            }
            else
            {
                settings = new Settings();
            }
            instance = settings;

            return settings;
        }

        public static void Save()
        {
            string dir = Path.GetDirectoryName(filePath);
            string fileName = Path.GetFileName(filePath);

            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            File.WriteAllText(Path.Combine(dir, fileName), JsonConvert.SerializeObject(instance, Formatting.Indented));
        }
    }
}
