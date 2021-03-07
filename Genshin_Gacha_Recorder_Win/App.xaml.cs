using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Genshine_Gacha_Recorder_Win
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private MainWindow window;

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            try
            {
                window = new MainWindow();
                window.Show();
            }
            catch(Exception exception)
            {
                System.IO.File.WriteAllText(@"./stacktrace.log", exception.ToString());
                window?.Close();
                MessageBox.Show(exception.ToString());
            }
        }
    }
}
