using System;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace WarcraftImageLabV2.ImageProcessing
{
    internal class Processor
    {
        private Settings settings;
        private bool isPreview;
        private string outputFileName;

        public Processor(string outputFileName, bool isPreview = false)
        {
            settings = Settings.Load();
            this.outputFileName = outputFileName;
            this.isPreview = isPreview;
        }

        /// <summary>
        /// Returns a list of images with the applied filters.
        /// Selecting multiple wc3 borders results in multiple images,
        /// hence it returns a list.
        /// </summary>
        public List<ProcessedImage> ApplyFilters(Bitmap image)
        {
            if (image == null)
            {
                return null;
            }

            image = Resize(image);
            List<ProcessedImage> images = AddIconBorders(image);

            return images;
        }

        private Bitmap Resize(Bitmap source)
        {
            if(!settings.Resize)
            {
                return source;
            }

            int width = settings.WidthNew;
            int height = settings.HeightNew;
            Size size = new Size(width, height);
            Bitmap resized = new Bitmap(source, size);
            return resized;
        }

        private List<ProcessedImage> AddIconBorders(Bitmap source)
        {
            List<ProcessedImage> list = new List<ProcessedImage>();

            bool isNone = settings.BorderMode == BorderModeEnum.None;

            bool notClassicCompatible =
                settings.BorderMode == BorderModeEnum.Classic &&
                source.Width != 64 &&
                source.Height != 64;

            bool notReforgedCompatible =
                settings.BorderMode == BorderModeEnum.Reforged &&
                source.Width != 64 &&
                source.Height != 64;

            if (isNone || notClassicCompatible || notReforgedCompatible)
            {
                var processedImage = new ProcessedImage(source, outputFileName);
                list.Add(processedImage);
                return list;
            }

            WC3IconFactory processor = new WC3IconFactory(settings.BorderMode);

            if (settings.BorderBTN && !(isPreview && list.Count > 0))
            {
                Bitmap sourceCopy = (Bitmap)source.Clone();
                Bitmap border = processor.GetBorder(IconType.BTN);
                AddIconBorderStandard(sourceCopy, border, IconType.BTN);
                var processedImage = new ProcessedImage(sourceCopy, "BTN" + outputFileName);
                list.Add(processedImage);
            }
            if (settings.BorderPAS && !(isPreview && list.Count > 0))
            {
                Bitmap sourceCopy = (Bitmap)source.Clone();
                Bitmap border = processor.GetBorder(IconType.PAS);
                AddIconBorderStandard(sourceCopy, border, IconType.PAS);
                var processedImage = new ProcessedImage(sourceCopy, "PAS" + outputFileName);
                list.Add(processedImage);
            }
            if (settings.BorderATC && !(isPreview && list.Count > 0))
            {
                Bitmap sourceCopy = (Bitmap)source.Clone();
                Bitmap border = processor.GetBorder(IconType.ATC);
                AddIconBorderStandard(sourceCopy, border, IconType.ATC);
                var processedImage = new ProcessedImage(sourceCopy, "ATC" + outputFileName);
                list.Add(processedImage);
            }
            if (settings.BorderInfocard && !(isPreview && list.Count > 0))
            {
                Bitmap sourceCopy = (Bitmap)source.Clone();
                Bitmap border = processor.GetBorder(IconType.ATT);
                sourceCopy = AddIconBorderUPG(sourceCopy, border);
                var processedImage = new ProcessedImage(sourceCopy, "ATT" + outputFileName);
                list.Add(processedImage);
            }
            if (settings.BorderInfocardUpgrade && !(isPreview && list.Count > 0))
            {
                Bitmap sourceCopy = (Bitmap)source.Clone();
                Bitmap border = processor.GetBorder(IconType.UPG);
                sourceCopy = AddIconBorderUPG(sourceCopy, border);
                var processedImage = new ProcessedImage(sourceCopy, "UPG" + outputFileName);
                list.Add(processedImage);
            }
            if (settings.BorderDISBTN && !(isPreview && list.Count > 0))
            {
                Bitmap sourceCopy = (Bitmap)source.Clone();
                Bitmap border = processor.GetBorder(IconType.DISBTN);
                AddIconBorderStandard(sourceCopy, border, IconType.DISBTN);
                var processedImage = new ProcessedImage(sourceCopy, "DISBTN" + outputFileName);
                list.Add(processedImage);
            }
            if (settings.BorderDISPAS && !(isPreview && list.Count > 0))
            {
                Bitmap sourceCopy = (Bitmap)source.Clone();
                Bitmap border = processor.GetBorder(IconType.DISPAS);
                AddIconBorderStandard(sourceCopy, border, IconType.DISPAS);
                var processedImage = new ProcessedImage(sourceCopy, "DISPAS" + outputFileName);
                list.Add(processedImage);
            }
            if (settings.BorderDISATC && !(isPreview && list.Count > 0))
            {
                Bitmap sourceCopy = (Bitmap)source.Clone();
                Bitmap border = processor.GetBorder(IconType.DISATC);
                AddIconBorderStandard(sourceCopy, border, IconType.DISATC);
                var processedImage = new ProcessedImage(sourceCopy, "DISATC" + outputFileName);
                list.Add(processedImage);
            }

            // Failsafe if no icon is selected
            if(list.Count == 0)
            {
                var processedImage = new ProcessedImage(source, outputFileName);
                list.Add(processedImage);
            }

            return list;
        }

        private void AddIconBorderStandard(Bitmap source, Bitmap border, IconType iconType)
        {
            int width = source.Width;
            int height = source.Height;
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    Color color = source.GetPixel(x, y);
                    byte redSource = color.R;
                    byte greenSource = color.G;
                    byte blueSource = color.B;
                    byte alphaSource = color.A;

                    // Disabled icon color saturation reduction
                    if (settings.BorderMode == BorderModeEnum.Reforged && (iconType == IconType.DISBTN || iconType == IconType.DISPAS || iconType == IconType.DISATC))
                    {
                        int greyscale = (int)(redSource * 0.3 + greenSource * 0.59 + blueSource * 0.11);

                        int redDiff = greyscale - redSource;
                        int greenDiff = greyscale - greenSource;
                        int blueDiff = greyscale - blueSource;

                        // 55% greyscale
                        double redChange = redDiff * 0.55;
                        double greenChange = greenDiff * 0.55;
                        double blueChange = blueDiff * 0.55;

                        int redInt = greyscale - (int)redChange;
                        int greenInt = greyscale - (int)greenChange;
                        int blueInt = greyscale - (int)blueChange;

                        // Further desaturation towards white (5%)
                        redInt = redInt + (int)((255 - redInt) * 0.05);
                        greenInt = greenInt + (int)((255 - greenInt) * 0.05);
                        blueInt = blueInt + (int)((255 - blueInt) * 0.05);

                        byte redNew = (byte)redInt;
                        byte greenNew = (byte)greenInt;
                        byte blueNew = (byte)blueInt;

                        redSource = redNew;
                        greenSource = greenNew;
                        blueSource = blueNew;
                    }

                    Color colorBorder = border.GetPixel(x, y);

                    byte redBorder = colorBorder.R;
                    byte greenBorder = colorBorder.G;
                    byte blueBorder = colorBorder.B;
                    byte alphaBorder = colorBorder.A;

                    if (alphaBorder != 0)
                    {
                        float alphaPercent = (float)alphaBorder / 255;

                        byte redBlended = (byte)((int)redSource * (1 - alphaPercent) + (redBorder * alphaPercent));
                        byte greenBlended = (byte)((int)greenSource * (1 - alphaPercent) + (greenBorder * alphaPercent));
                        byte blueBlended = (byte)((int)blueSource * (1 - alphaPercent) + (blueBorder * alphaPercent));

                        source.SetPixel(x, y, Color.FromArgb(255, redBlended, greenBlended, blueBlended));
                    }
                }
            }
        }

        private Bitmap AddIconBorderUPG(Bitmap source, Bitmap border)
        {
            int width = source.Width;
            int height = source.Height;
            Bitmap canvas = new Bitmap(width, height);

            source = ResizeToFitUPGBorder(source);

            int widthResized = source.Width;
            int heightResized = source.Height;

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    Color color = Color.FromArgb(255, 0, 0, 0);
                    if (x < widthResized && y < heightResized)
                        color = source.GetPixel(x, y);

                    if (x < source.Width && y < source.Height)
                    {
                        canvas.SetPixel(x, y, color);
                    }

                    byte redSource = color.R;
                    byte greenSource = color.G;
                    byte blueSource = color.B;
                    byte alphaSource = color.A;

                    Color colorBorder = border.GetPixel(x, y);

                    byte redBorder = colorBorder.R;
                    byte greenBorder = colorBorder.G;
                    byte blueBorder = colorBorder.B;
                    byte alphaBorder = colorBorder.A;

                    // The outer transparent part of the actual applied border is red or (255,0,0), so we make that part fully transparent.
                    // However, there is an unknown error when displaying the final icon. The red color seems to go down to 217 in the corners and sides?
                    // This is compensated for here.
                    if (colorBorder.R > 216 && colorBorder.G == 0 && colorBorder.B == 0)
                    {
                        canvas.SetPixel(x, y, Color.FromArgb(0, 0, 0, 0)); // The border's red color becomes 100% transparent.
                    }
                    else if (alphaBorder != 0)
                    {
                        float alphaPercent = (float)alphaBorder / 255;

                        byte redBlended = (byte)((int)redSource * (1 - alphaPercent) + (redBorder * alphaPercent));
                        byte greenBlended = (byte)((int)greenSource * (1 - alphaPercent) + (greenBorder * alphaPercent));
                        byte blueBlended = (byte)((int)blueSource * (1 - alphaPercent) + (blueBorder * alphaPercent));

                        canvas.SetPixel(x, y, Color.FromArgb(255, redBlended, greenBlended, blueBlended));
                    }
                }
            }

            return canvas;
        }

        private Bitmap ResizeToFitUPGBorder(Bitmap bitmap)
        {
            int size = settings.BorderMode == BorderModeEnum.Classic ? 56 : 222;
            Bitmap resized = new Bitmap(bitmap, new Size(size, size));

            return resized;
        }
    }
}
