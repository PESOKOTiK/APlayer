using System.IO;
using System.Windows;

namespace media_player
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            if (e.Args.Length == 1)
            {
                var file = new FileInfo(e.Args[0]);
                MessageBox.Show(file.FullName.ToString());
                if (file.Exists)
                {
                    MessageBox.Show("File yes");
                    MainWindow window = new MainWindow(file);
                    window.Show();
                }
                else
                {
                    MessageBox.Show("Fileno");
                    MainWindow window = new MainWindow();
                    window.Show();
                }
            }
            
        }
    }
}
