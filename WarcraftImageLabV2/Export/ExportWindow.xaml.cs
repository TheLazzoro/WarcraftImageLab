using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Windows.Shapes;
using WarcraftImageLabV2.ImageProcessing;

namespace WarcraftImageLabV2.Export
{
    public partial class ExportWindow : Window
    {
        private BackgroundWorker worker;
        private Writer writer;
        private int fileCount;

        public ExportWindow(string outputFileName, string dir, string[] files)
        {
            InitializeComponent();

            fileCount = files.Length;

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

        private void Writer_OnError(string arg1, string arg2)
        {
            worker.ReportProgress(-1, arg1);
        }

        private void Writer_OnComplete()
        {
            
        }

        private void Worker_ProgressChanged(object? sender, ProgressChangedEventArgs e)
        {
            float percent = (float)writer.progress / fileCount * 100;
            progressBar.Value = percent;
            textblockProgress.Text = writer.progress + "/" + fileCount;
            textblockPercent.Text = (int)percent + "%";

            if (e.ProgressPercentage == -1)
            {
                listViewErrors.Items.Add(new ListViewItem
                {
                    Content = e.UserState,
                });
            }
        }

        private void Worker_RunWorkerCompleted(object? sender, RunWorkerCompletedEventArgs e)
        {
            
        }

        private void btnStop_Click(object sender, RoutedEventArgs e)
        {
            writer.Cancel();
            worker.CancelAsync();
        }
    }
}
