using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WarcraftImageLab.Dialogs;
using WarcraftImageLab.Export;
using WarcraftImageLab.Filters;
using WarcraftImageLab.Import;
using WarcraftImageLab.Model;

namespace WarcraftImageLab.Content
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
            filtersControl.OnFiltersChanged += FiltersControl_OnFiltersChanged;
            exportControl.OnExport += ExportControl_OnExport;
            viewModel.FileItems.CollectionChanged += FileItems_CollectionChanged;

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

        public void AddFileSystemEntry(string fullPath)
        {
            if (Directory.Exists(fullPath))
            {
                string[] files = Directory.GetFiles(fullPath, "*.*", SearchOption.AllDirectories);
                viewModel.AddFilesToList(files);
            }
            else
            {
                viewModel.AddFileToList(fullPath);
            }
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

        private void FiltersControl_OnFiltersChanged()
        {
            previewControl.ReloadImage();
        }

        private void FileItems_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            textBlockItemCount.Text = "Items: " + viewModel.FileItems.Count;
        }

        private void btnClearList_Click(object sender, RoutedEventArgs e)
        {
            DialogWindow dialog = new DialogWindow("Confirm", "Are you sure you want to clear the list?");
            dialog.ShowDialog();
            if (dialog.OK)
            {
                viewModel.FileItems.Clear();
            }
        }

        private bool suppressSelection = false;
        private void listViewFiles_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (suppressSelection)
                return;

            int index = listViewFiles.SelectedIndex;
            if (listViewFiles.Items.Count == 0 || index < 0)
            {
                return;
            }

            FileItem item = viewModel.FileItems[index];
            try
            {
                previewControl.UpdateImage(item.FullPath);
            }
            catch (Exception ex)
            {
                previewControl.SetErrorMessage(ex.Message);
            }
        }

        private void ExportControl_OnExport()
        {
            string outputFileName = exportControl.textboxFilename.Text;
            string outputDir = exportControl.textblockOutputDir.Text;
            string[] files = viewModel.FileItems.Select(f => f.FullPath).ToArray();

            ExportWindow exportWindow = new ExportWindow(outputFileName, outputDir, files);
            exportWindow.ShowDialog();
        }

        private void SelectNearestItemAt(int index)
        {
            // Select next item after removal
            if (listViewFiles.Items.Count == 0)
            {
                return;
            }

            if (listViewFiles.Items.Count > index)
            {
                listViewFiles.SelectedIndex = index;
            }
            else
            {
                listViewFiles.SelectedIndex = index - 1;
            }
        }

        private void listViewFiles_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete)
            {
                suppressSelection = true;
                if (listViewFiles.SelectedItem == null)
                    return;

                int index = listViewFiles.SelectedIndex;
                int selectedItemsCount = listViewFiles.SelectedItems.Count;
                List<object> itemsToRemove = new List<object>();
                for (int i = 0; i < selectedItemsCount; i++)
                {
                    var item = listViewFiles.SelectedItems[i];
                    itemsToRemove.Add(item);
                }
                for (int i = 0; i < itemsToRemove.Count; i++)
                {
                    var item = itemsToRemove[i];
                    index = listViewFiles.Items.IndexOf(item);
                    viewModel.RemoveFileAt(index);
                }

                suppressSelection = false;
                SelectNearestItemAt(index);
            }
        }

        private void menuExport_Click(object sender, RoutedEventArgs e)
        {
            int index = listViewFiles.SelectedIndex;
            var item = viewModel.FileItems[index];
            string fileName = exportControl.textboxFilename.Text;
            string dir = exportControl.textblockOutputDir.Text;
            string[] files = new string[] { item.FullPath };

            ExportWindow exportWindow = new ExportWindow(fileName, dir, files);
            exportWindow.ShowDialog();
        }

        private void menuOpenFileLocation_Click(object sender, RoutedEventArgs e)
        {
            int index = listViewFiles.SelectedIndex;
            var item = viewModel.FileItems[index];
            string filePath = item.FullPath;
            if (File.Exists(filePath))
            {
                Process.Start("explorer.exe", "/select, " + filePath);
            }
        }

        private void menuRemoveItem_Click(object sender, RoutedEventArgs e)
        {
            int index = listViewFiles.SelectedIndex;
            viewModel.RemoveFileAt(index);
            SelectNearestItemAt(index);
        }

        private void listViewFiles_ContextMenuOpening(object sender, ContextMenuEventArgs e)
        {
            bool hasSelection = listViewFiles.SelectedItem != null;
            menuOpenFileLocation.IsEnabled = hasSelection;
            menuRemoveItem.IsEnabled = hasSelection;
        }
    }
}
