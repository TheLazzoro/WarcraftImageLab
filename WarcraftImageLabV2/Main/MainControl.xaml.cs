using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
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
using WarcraftImageLabV2.Dialogs;
using WarcraftImageLabV2.Export;
using WarcraftImageLabV2.Filters;
using WarcraftImageLabV2.ImageProcessing;
using WarcraftImageLabV2.Import;
using WarcraftImageLabV2.Model;

namespace WarcraftImageLabV2.Content
{
    public partial class MainControl : UserControl
    {
        private MainControlViewModel viewModel;

        private ImportControl importControl;
        private FiltersControl filtersControl;
        private ExportControl exportControl;

        private UserControl currentTab;

        public MainControl()
        {
            InitializeComponent();

            viewModel = new MainControlViewModel();
            this.DataContext = viewModel;

            importControl = new();
            filtersControl = new();
            exportControl = new();

            Grid.SetColumn(importControl, 0);
            Grid.SetColumn(filtersControl, 0);
            Grid.SetColumn(exportControl, 0);
            Grid.SetRow(importControl, 0);
            Grid.SetRow(filtersControl, 0);
            Grid.SetRow(exportControl, 0);
            ChangeTab(TabMenuEnum.Import);

            importControl.OnClickImportFile += ImportControl_OnClickImportFile;
            importControl.OnClickImportFolder += ImportControl_OnClickImportFolder;
            viewModel.OnFileAdded += ViewModel_OnFileAdded;
        }

        public void ChangeTab(TabMenuEnum tabToShow)
        {
            if (currentTab != null)
            {
                grid.Children.Remove(currentTab);
            }

            UserControl control;
            switch (tabToShow)
            {
                case TabMenuEnum.Import:
                    control = importControl;
                    break;
                case TabMenuEnum.Filters:
                    control = filtersControl;
                    break;
                case TabMenuEnum.Export:
                    control = exportControl;
                    break;
                default:
                    control = importControl;
                    break;
            }

            grid.Children.Add(control);
            currentTab = control;
        }



        private void ImportControl_OnClickImportFile()
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            //openFileDialog1.InitialDirectory = "c:\\";
            //openFileDialog1.Filter = "Image Files (*.jpg, *.png. *.tiff, *.gif, *.bmp, *.tga)|*.jpg;*.png;*.tiff;*.gif;*.bmp;.tga;*";
            openFileDialog1.FilterIndex = 0;
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() == true)
            {
                string selectedFileName = openFileDialog1.FileName;
                viewModel.AddFileToList(selectedFileName);
            }
        }

        private void ImportControl_OnClickImportFolder()
        {
            using (var fbd = new System.Windows.Forms.FolderBrowserDialog())
            {
                var result = fbd.ShowDialog();

                if (result == System.Windows.Forms.DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                {
                    string directory = fbd.SelectedPath;

                    string[] files;
                    if (importControl.checkBoxSubfolders.IsChecked == true)
                    {
                        files = Directory.GetFiles(directory, "*.*", SearchOption.AllDirectories);
                    }
                    else
                    {
                        files = Directory.GetFiles(directory, "*.*", SearchOption.TopDirectoryOnly);
                    }

                    viewModel.AddFilesToList(files);
                }
            }
        }

        private void ViewModel_OnFileAdded()
        {
            textBlockItemCount.Text = "Items: " + viewModel.FileItems.Count;
        }


        private void btnClearList_Click(object sender, RoutedEventArgs e)
        {
            DialogWindow dialog = new DialogWindow("Confirm", "Are you sure you want to clear the list?");
            dialog.ShowDialog();
            if (dialog.OK)
            {
                listViewFiles.Items.Clear();
            }
        }

        private void listViewFiles_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index = listViewFiles.SelectedIndex;
            FileItem item = viewModel.FileItems[index];
            try
            {
                Bitmap bitmap = Reader.ReadImageFile(item.FullPath);
                previewControl.image.Source = BitmapConverter.ToBitmapSource(bitmap);
            }
            catch (Exception ex)
            {
                MessageWindow message = new MessageWindow("Error", ex.Message);
                message.ShowDialog();
            }
        }
    }
}
