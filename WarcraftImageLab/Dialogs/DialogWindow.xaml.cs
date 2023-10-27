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
using System.Windows.Shapes;

namespace WarcraftImageLab.Dialogs
{
    /// <summary>
    /// Interaction logic for DialogWindow.xaml
    /// </summary>
    public partial class DialogWindow : Window
    {
        public bool OK;

        public DialogWindow(string title, string message)
        {
            Owner = MainWindow.GetMainWindow();

            InitializeComponent();

            this.Title = title;
            this.textBlockMessage.Text = message;
        }

        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            OK = true;
            this.Close();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
