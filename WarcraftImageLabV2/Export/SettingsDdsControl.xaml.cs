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
using WarcraftImageLabV2.ImageProcessing.Enums;

namespace WarcraftImageLabV2.Export
{
    /// <summary>
    /// Interaction logic for SettingsDdsControl.xaml
    /// </summary>
    public partial class SettingsDdsControl : UserControl
    {
        Settings settings;

        public SettingsDdsControl()
        {
            InitializeComponent();

            settings = Settings.Load();
            foreach (CompressionDDS ddsCompression in Enum.GetValues(typeof(CompressionDDS)))
            {
                string comboboxText = string.Empty;
                switch (ddsCompression)
                {
                    case CompressionDDS.BC1:
                        comboboxText = "BC1 (DXT1), RGB | no alpha";
                        break;
                    case CompressionDDS.BC1a:
                        comboboxText = "BC1 (DXT1), RGBA | 1 bit alpha";
                        break;
                    case CompressionDDS.BC2:
                        comboboxText = "BC2 (DXT2 or DXT3), RGBA | explicit alpha";
                        break;
                    case CompressionDDS.BC3:
                        comboboxText = "BC3 (DXT4 or DXT5), RGBA | interpolated alpha";
                        break;
                    default:
                        break;
                }

                comboboxCompression.Items.Add(new ComboBoxItem
                {
                    Content = comboboxText,
                });
            }
            comboboxCompression.SelectedIndex = (int)settings.CompressionDDS;

            checkboxMipmaps.IsChecked = settings.GenerateMipmapsDDS;
            switch (settings.QualityDDS)
            {
                case QualityDDS.Fastest:
                    radbtnFastest.IsChecked = true;
                    break;
                case QualityDDS.Balanced:
                    radbtnBalanced.IsChecked = true;
                    break;
                case QualityDDS.Highest:
                    radbtnHighest.IsChecked = true;
                    break;
                default:
                    break;
            }
        }

        private void comboboxCompression_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            settings.CompressionDDS = (CompressionDDS)comboboxCompression.SelectedIndex;
        }

        private void checkboxMipmaps_Click(object sender, RoutedEventArgs e)
        {
            settings.GenerateMipmapsDDS = (bool)checkboxMipmaps.IsChecked;
        }

        private void radbtnFastest_Click(object sender, RoutedEventArgs e)
        {
            settings.QualityDDS = QualityDDS.Fastest;
        }

        private void radbtnBalanced_Click(object sender, RoutedEventArgs e)
        {
            settings.QualityDDS = QualityDDS.Balanced;
        }

        private void radbtnHighest_Click(object sender, RoutedEventArgs e)
        {
            settings.QualityDDS = QualityDDS.Highest;
        }

    }
}
