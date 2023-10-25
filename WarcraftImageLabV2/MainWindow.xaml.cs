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

        public MainWindow()
        {
            InitializeComponent();
            instance = this;
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
            mainControl.ChangeTab(Model.TabMenuEnum.Import);
        }

        private void btnTabFilters_Click(object sender, RoutedEventArgs e)
        {
            mainControl.ChangeTab(Model.TabMenuEnum.Filters);
        }

        private void btnTabExport_Click(object sender, RoutedEventArgs e)
        {
            mainControl.ChangeTab(Model.TabMenuEnum.Export);
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
            Settings.Save();
        }
    }
}
