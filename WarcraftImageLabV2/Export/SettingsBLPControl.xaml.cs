using Microsoft.VisualBasic;
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
    public partial class SettingsBLPControl : UserControl
    {
        Settings settings;

        public SettingsBLPControl()
        {
            InitializeComponent();

            settings = Settings.Load();
            sliderQuality.Value = settings.BlpQuality;
            sliderPalette.Value = settings.BlpPalettedColors;
            textboxMipmapCount.Text = settings.BlpMipmapCount.ToString();

            foreach (BlpType blpType in Enum.GetValues(typeof(BlpType)))
            {
                string comboboxText = string.Empty;
                switch (blpType)
                {
                    case BlpType.Compressed:
                        comboboxText = "Compressed (JPEG)";
                        break;
                    case BlpType.Paletted:
                        comboboxText = "Paletted";
                        break;
                }

                comboboxBlpType.Items.Add(new ComboBoxItem
                {
                    Content = comboboxText,
                });
            }
            comboboxBlpType.SelectedIndex = (int)settings.BlpType;
        }

        private void comboboxBlpType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            settings.BlpType = (BlpType)comboboxBlpType.SelectedIndex;
            switch (settings.BlpType)
            {
                case BlpType.Compressed:
                    textblockQuality.Visibility = Visibility.Visible;
                    sliderQuality.Visibility = Visibility.Visible;
                    textblockPalette.Visibility = Visibility.Hidden;
                    sliderPalette.Visibility = Visibility.Hidden;
                    break;
                case BlpType.Paletted:
                    textblockQuality.Visibility = Visibility.Hidden;
                    sliderQuality.Visibility = Visibility.Hidden;
                    textblockPalette.Visibility = Visibility.Visible;
                    sliderPalette.Visibility = Visibility.Visible;
                    break;
            }
            UpdateQualityText();
        }

        private void sliderQuality_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (settings == null)
                return;

            settings.BlpQuality = (int)sliderQuality.Value;
            UpdateQualityText();
        }

        private void sliderPalette_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (settings == null)
                return;

            settings.BlpPalettedColors = (int)sliderPalette.Value;
            UpdateQualityText();
        }

        private void UpdateQualityText()
        {
            switch (settings.BlpType)
            {
                case BlpType.Compressed:
                    textblockQualityNumber.Text = (settings.BlpQuality).ToString() + "%";
                    break;
                case BlpType.Paletted:
                    textblockQualityNumber.Text = (settings.BlpPalettedColors).ToString() + " colors";
                    break;
            }
        }

        int previousNumber = 1;
        private void textboxMipmapCount_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                int mipmapCount = int.Parse(textboxMipmapCount.Text);
                if (mipmapCount < 1)
                {
                    mipmapCount = 1;
                    textboxMipmapCount.Text = previousNumber.ToString();
                }
                else if (mipmapCount > 15)
                {
                    mipmapCount = 15;
                    textboxMipmapCount.Text = previousNumber.ToString();
                }

                previousNumber = mipmapCount;
                settings.BlpMipmapCount = mipmapCount;
            }
            catch (Exception)
            {
                textboxMipmapCount.Text = previousNumber.ToString();
            }
        }
    }
}
