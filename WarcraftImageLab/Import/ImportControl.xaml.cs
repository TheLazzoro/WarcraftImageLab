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

namespace WarcraftImageLab.Import
{
    public partial class ImportControl : UserControl
    {
        public event Action OnClickImportFile;
        public event Action OnClickImportFolder;

        public ImportControl()
        {
            InitializeComponent();
        }

        private void btnImportFile_Click(object sender, RoutedEventArgs e)
        {
            OnClickImportFile?.Invoke();
        }

        private void btnImportFolder_Click(object sender, RoutedEventArgs e)
        {
            OnClickImportFolder?.Invoke();
        }
    }
}
