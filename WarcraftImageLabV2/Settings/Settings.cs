using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WarcraftImageLabV2.ImageProcessing.ImageSettings;

namespace WarcraftImageLabV2
{
    public class Settings
    {
        private static Settings instance;
        private static readonly string filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Warcraft Image Lab/settings.json");

        #region Filter settings

        public BorderModeEnum BorderMode = BorderModeEnum.None;
        public bool BorderBTN = false;
        public bool BorderPAS = false;
        public bool BorderATC = false;
        public bool BorderInfocard = false;
        public bool BorderInfocardUpgrade = false;
        public bool BorderDISBTN = false;
        public bool BorderDISPAS = false;
        public bool BorderDISATC = false;

        public bool Resize = false;
        public int WidthNew = 4;
        public int HeightNew = 4;

        #endregion

        #region Export settings

        public ImageFormatExportable ImageFormat = ImageFormatExportable.JPG;
        public int QualityJPG = 5;
        public CompressionDDS CompressionDDS = CompressionDDS.BC1;
        public QualityDDS QualityDDS = QualityDDS.Balanced;
        public bool GenerateMipmapsDDS = true;

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
