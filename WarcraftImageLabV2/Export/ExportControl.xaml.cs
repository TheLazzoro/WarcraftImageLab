using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WarcraftImageLabV2.ImageProcessing.ImageSettings;

namespace WarcraftImageLabV2.Export
{
    public partial class ExportControl : UserControl
    {
        private Settings settings;
        private UserControl emptySettingsControl;
        private UserControl currentFormatSettingsControl;

        public ExportControl()
        {
            InitializeComponent();

            emptySettingsControl = new UserControl();
            Grid.SetRow(emptySettingsControl, 4);
            Grid.SetColumn(emptySettingsControl, 2);
            gridGroupBox.Children.Add(emptySettingsControl);

            settings = Settings.Load();
            foreach (var imageFormat in Enum.GetNames(typeof(ImageFormatExportable)))
            {
                comboboxFormat.Items.Add(new ComboBoxItem
                {
                    Content = imageFormat.ToString(),
                });
            }
            comboboxFormat.SelectedIndex = (int)settings.ImageFormat;
            checkBoxKeepFilename.IsChecked = settings.KeepFilename;
        }

        private void EnableExportButton()
        {
            btnExportAll.IsEnabled = !string.IsNullOrEmpty(textblockOutputDir.Text);
        }

        private void btnChooseOutput_Click(object sender, RoutedEventArgs e)
        {
            using (var dialog = new System.Windows.Forms.FolderBrowserDialog())
            {
                System.Windows.Forms.DialogResult result = dialog.ShowDialog();
                if (dialog.SelectedPath != "")
                {
                    textblockOutputDir.Text = dialog.SelectedPath;
                    EnableExportButton();
                }
            }
        }

        private void comboboxFormat_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ImageFormatExportable format = (ImageFormatExportable)(comboboxFormat.SelectedIndex);
            settings.ImageFormat = format;
            textBlockExtension.Text = "." + format.ToString().ToLower();

            if (currentFormatSettingsControl != null)
            {
                currentFormatSettingsControl.Visibility = Visibility.Hidden;
            }
            switch (format)
            {
                case ImageFormatExportable.JPG:
                    currentFormatSettingsControl = jpgControl;
                    break;
                case ImageFormatExportable.PNG:
                    currentFormatSettingsControl = new UserControl();
                    break;
                case ImageFormatExportable.DDS:
                    currentFormatSettingsControl = ddsControl;
                    break;
                case ImageFormatExportable.BLP:
                    currentFormatSettingsControl = new UserControl();
                    break;
                case ImageFormatExportable.TGA:
                    currentFormatSettingsControl = new UserControl();
                    break;
                case ImageFormatExportable.BMP:
                    currentFormatSettingsControl = new UserControl();
                    break;
                case ImageFormatExportable.WEBP:
                    currentFormatSettingsControl = new UserControl();
                    break;
                case ImageFormatExportable.TIFF:
                    currentFormatSettingsControl = new UserControl();
                    break;
                default:
                    break;
            }
            currentFormatSettingsControl.Visibility = Visibility.Visible;
        }

        private void checkBoxKeepFilename_Click(object sender, RoutedEventArgs e)
        {
            settings.KeepFilename = (bool)checkBoxKeepFilename.IsChecked;
            textboxFilename.IsEnabled = !settings.KeepFilename;
        }

        private void btnExportAll_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
