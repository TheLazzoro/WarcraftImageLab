using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Media;
using System.Threading.Tasks;
using System.Windows;
using WarcraftImageLabV2.Dialogs;

namespace WarcraftImageLabV2
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            SystemSounds.Hand.Play();

            if (!Directory.Exists(Path.Combine(Directory.GetCurrentDirectory(), "errors")))
                Directory.CreateDirectory(Path.Combine(Directory.GetCurrentDirectory(), "errors"));

            File.WriteAllText(Path.Combine(Directory.GetCurrentDirectory(), "errors/Error_" + DateTime.Now.ToString("yyyy/MM/dd HH.mm.ss") + ".txt"), e.Exception.StackTrace);
            MessageWindow window = new MessageWindow("Error", e.Exception.Message + "\n\nError log saved.");
            window.ShowDialog();
            Application.Current.Shutdown();
            e.Handled = true;
        }
    }
}
