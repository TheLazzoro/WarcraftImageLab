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

namespace WarcraftImageLab.Export
{
    public partial class SettingsWebPControl : UserControl
    {
        Settings settings;

        public SettingsWebPControl()
        {
            InitializeComponent();

            settings = Settings.Load();
            sliderQuality.Value = settings.QualityWebP;
        }

        private void sliderQuality_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (settings == null)
                return;

            settings.QualityWebP = (int)sliderQuality.Value * 10;
            textblockQuality.Text = (settings.QualityWebP / 10).ToString();
        }
    }
}
