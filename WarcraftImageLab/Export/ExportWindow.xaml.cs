using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using WarcraftImageLabV2.ImageProcessing;

namespace WarcraftImageLabV2.Export
{
    public partial class ExportWindow : Window
    {
        private BackgroundWorker worker;
        private Writer writer;
        private int fileCount;
        private ExportWindowViewModel viewModel;

        private string outputDir;

        public ExportWindow(string outputFileName, string dir, string[] files)
        {
            Owner = MainWindow.GetMainWindow();

            InitializeComponent();

            viewModel = new ExportWindowViewModel();
            this.DataContext = viewModel;

            fileCount = files.Length;
            outputDir = dir;

            writer = new Writer(outputFileName, dir, files);
            writer.OnFileConverted += Writer_OnFileConverted;
            writer.OnError += Writer_OnError;
            writer.OnComplete += Writer_OnComplete;

            worker = new BackgroundWorker();
            worker.WorkerReportsProgress = true;
            worker.WorkerSupportsCancellation = true;
            worker.DoWork += Worker_DoWork;
            worker.ProgressChanged += Worker_ProgressChanged;
            worker.RunWorkerCompleted += Worker_RunWorkerCompleted;
            worker.RunWorkerAsync();
        }

        private void Worker_DoWork(object? sender, DoWorkEventArgs e)
        {
            writer.Write();
            worker.ReportProgress(100);
        }

        private void Writer_OnFileConverted(string obj)
        {
            worker.ReportProgress(0);
        }

        private void Writer_OnError(string fullPath, string errorMsg)
        {
            ListItemError error = new ListItemError(fullPath, errorMsg);
            worker.ReportProgress(-1, error);
        }

        private void Writer_OnComplete()
        {
            worker.ReportProgress(100);
        }

        private void Worker_ProgressChanged(object? sender, ProgressChangedEventArgs e)
        {
            if (fileCount == 0)
                return;

            float percent = (float)writer.progress / fileCount * 100;
            string textPercent = (int)percent + "%";
            progressBar.Value = percent;
            textblockProgress.Text = writer.progress + "/" + fileCount;
            textblockPercent.Text = textPercent;
            Title = "Exporting... " + textPercent;

            if (e.ProgressPercentage == -1)
            {
                var error = (ListItemError)e.UserState;
                viewModel.AddErrorToList(error);
            }
        }

        private void Worker_RunWorkerCompleted(object? sender, RunWorkerCompletedEventArgs e)
        {
            btnStop.IsEnabled = false;
            btnClose.IsEnabled = true;

            textblockProgress.Text = writer.progress + "/" + fileCount;
            if (writer.progress == fileCount)
            {
                textblockPercent.Text = "100%";
                Title = "Complete!";
            }
        }

        private void btnShowOutputFolder_Click(object sender, RoutedEventArgs e)
        {
            Process.Start("explorer.exe", outputDir);
        }

        private void btnStop_Click(object sender, RoutedEventArgs e)
        {
            writer.Cancel();
            worker.CancelAsync();
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

    }
}
