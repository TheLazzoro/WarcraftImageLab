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

namespace WarcraftImageLabV2.Filters
{
    public partial class FiltersControl : UserControl
    {
        public event Action OnFiltersChanged;
        Settings settings;

        public FiltersControl()
        {
            InitializeComponent();

            settings = Settings.Load();
            switch (settings.BorderMode)
            {
                case BorderModeEnum.None:
                    radBtnNone.IsChecked = true;
                    break;
                case BorderModeEnum.Classic:
                    radBtnClassic.IsChecked = true;
                    break;
                case BorderModeEnum.Reforged:
                    radBtnReforged.IsChecked = true;
                    break;
                default:
                    radBtnNone.IsChecked = true;
                    break;
            }

            checkBTN.IsChecked = settings.BorderBTN;
            checkPAS.IsChecked = settings.BorderPAS;
            checkATC.IsChecked = settings.BorderATC;
            checkInfocard.IsChecked = settings.BorderInfocard;
            checkInfocardUpgrade.IsChecked = settings.BorderInfocardUpgrade;
            checkDISBTN.IsChecked = settings.BorderDISBTN;
            checkDISPAS.IsChecked = settings.BorderDISPAS;
            checkDISATC.IsChecked = settings.BorderDISATC;

            textboxWidth.Text = settings.WidthNew.ToString();
            textboxHeight.Text = settings.HeightNew.ToString();
        }

        private void radBtnNone_Click(object sender, RoutedEventArgs e)
        {
            textblockMessage.Text = "";
            settings.BorderMode = BorderModeEnum.None;
            OnFiltersChanged?.Invoke();
        }

        private void radBtnClassic_Click(object sender, RoutedEventArgs e)
        {
            textblockMessage.Text = "Only applies to 64x64 images.";
            settings.BorderMode = BorderModeEnum.Classic;
            OnFiltersChanged?.Invoke();
        }

        private void radBtnReforged_Click(object sender, RoutedEventArgs e)
        {
            textblockMessage.Text = "Only applies to 256x256 images.";
            settings.BorderMode = BorderModeEnum.Reforged;
            OnFiltersChanged?.Invoke();
        }

        private void checkBTN_Click(object sender, RoutedEventArgs e)
        {
            settings.BorderBTN = (bool)checkBTN.IsChecked;
            OnFiltersChanged?.Invoke();
        }

        private void checkPAS_Click(object sender, RoutedEventArgs e)
        {
            settings.BorderPAS = (bool)checkPAS.IsChecked;
            OnFiltersChanged?.Invoke();
        }

        private void checkATC_Click(object sender, RoutedEventArgs e)
        {
            settings.BorderATC = (bool)checkATC.IsChecked;
            OnFiltersChanged?.Invoke();
        }

        private void checkInfocard_Click(object sender, RoutedEventArgs e)
        {
            settings.BorderInfocard = (bool)checkInfocard.IsChecked;
            OnFiltersChanged?.Invoke();
        }

        private void checkInfocardUpgrade_Click(object sender, RoutedEventArgs e)
        {
            settings.BorderInfocardUpgrade = (bool)checkInfocardUpgrade.IsChecked;
            OnFiltersChanged?.Invoke();
        }

        private void checkDISBTN_Click(object sender, RoutedEventArgs e)
        {
            settings.BorderDISBTN = (bool)checkDISBTN.IsChecked;
            OnFiltersChanged?.Invoke();
        }

        private void checkDISPAS_Click(object sender, RoutedEventArgs e)
        {
            settings.BorderDISPAS = (bool)checkDISPAS.IsChecked;
            OnFiltersChanged?.Invoke();
        }

        private void checkDISATC_Click(object sender, RoutedEventArgs e)
        {
            settings.BorderDISATC = (bool)checkDISATC.IsChecked;
            OnFiltersChanged?.Invoke();
        }

        private void radBtnNone_Checked(object sender, RoutedEventArgs e)
        {
            EnableWC3IconMenu(false);
        }

        private void radBtnClassic_Checked(object sender, RoutedEventArgs e)
        {
            EnableWC3IconMenu(true);
        }

        private void radBtnReforged_Checked(object sender, RoutedEventArgs e)
        {
            EnableWC3IconMenu(true);
        }


        private void EnableWC3IconMenu(bool doEnable)
        {
            checkBTN.IsEnabled = doEnable;
            checkPAS.IsEnabled = doEnable;
            checkATC.IsEnabled = doEnable;
            checkInfocard.IsEnabled = doEnable;
            checkInfocardUpgrade.IsEnabled = doEnable;
            checkDISBTN.IsEnabled = doEnable;
            checkDISPAS.IsEnabled = doEnable;
            checkDISATC.IsEnabled = doEnable;
        }

        private void checkboxResize_Click(object sender, RoutedEventArgs e)
        {
            settings.Resize = (bool)checkboxResize.IsChecked;
            textboxWidth.IsEnabled = (bool)checkboxResize.IsChecked;
            textboxHeight.IsEnabled = (bool)checkboxResize.IsChecked;
            OnFiltersChanged?.Invoke();
        }

        private void textboxWidth_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(textboxWidth.Text))
                return;

            settings.WidthNew = int.Parse(textboxWidth.Text);
            OnFiltersChanged?.Invoke();
        }

        private void textboxHeight_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(textboxHeight.Text))
                return;

            settings.HeightNew = int.Parse(textboxHeight.Text);
            OnFiltersChanged?.Invoke();
        }
    }
}
