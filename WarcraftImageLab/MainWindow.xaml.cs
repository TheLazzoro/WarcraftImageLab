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
using WarcraftImageLabV2.Content;

namespace WarcraftImageLabV2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static MainWindow instance;
        private Settings settings;

        public MainWindow()
        {
            InitializeComponent();
            instance = this;

            settings = Settings.Load();
            this.Left = settings.WindowX;
            this.Top = settings.WindowY;
            this.Width = settings.WindowWidth;
            this.Height = settings.WindowHeight;

            ChangeTab(Model.TabMenuEnum.Import, btnTabImport);
        }

        /// <summary>
        /// This function only exists because of WPF wizardry.
        /// With a debug build 'Application.Current.MainWindow' works, but not in release.
        /// </summary>
        public static MainWindow GetMainWindow()
        {
            return instance;
        }

        private void btnTabImport_Click(object sender, RoutedEventArgs e)
        {
            ChangeTab(Model.TabMenuEnum.Import, (Button)e.Source);
        }

        private void btnTabFilters_Click(object sender, RoutedEventArgs e)
        {
            ChangeTab(Model.TabMenuEnum.Filters, (Button)e.Source);
        }

        private void btnTabExport_Click(object sender, RoutedEventArgs e)
        {
            ChangeTab(Model.TabMenuEnum.Export, (Button)e.Source);
        }

        private void btnAbout_Click(object sender, RoutedEventArgs e)
        {
            AboutWindow aboutWindow = new AboutWindow();
            aboutWindow.ShowDialog();
        }

        private void ChangeTab(Model.TabMenuEnum tab, Button selected)
        {
            mainControl.ChangeTab(tab);

            Style style = this.FindResource("btnMenu") as Style;
            Style styleSelected = this.FindResource("btnOrange") as Style;

            btnTabImport.Style = style;
            btnTabExport.Style = style;
            btnTabFilters.Style = style;

            selected.Style = styleSelected;
        }

        private void Window_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);

                for (int i = 0; i < files.Length; i++)
                {
                    mainControl.AddFileSystemEntry(files[i]);
                }
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            settings.WindowX = (int)this.Left;
            settings.WindowY = (int)this.Top;
            settings.WindowWidth = (int)this.Width;
            settings.WindowHeight = (int)this.Height;
            Settings.Save();
        }

    }
}
