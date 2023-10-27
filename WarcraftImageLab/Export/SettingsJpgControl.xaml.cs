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

namespace WarcraftImageLabV2.Export
{
    /// <summary>
    /// Interaction logic for SettingsJpgControl.xaml
    /// </summary>
    public partial class SettingsJpgControl : UserControl
    {
        Settings settings;

        public SettingsJpgControl()
        {
            InitializeComponent();

            settings = Settings.Load();
            sliderQuality.Value = settings.QualityJPG;
        }

        private void sliderQuality_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (settings == null)
                return;

            settings.QualityJPG = (int)sliderQuality.Value * 10;
            textblockQuality.Text = (settings.QualityJPG / 10).ToString();
        }
    }
}
