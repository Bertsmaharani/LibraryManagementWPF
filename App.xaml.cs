using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;

namespace LibraryManagementWPF
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : System.Windows.Application
    {
        public static new MainWindow MainWindow { get; set; }
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var authWindow = new Authentification();

            if (authWindow != null)
            {

                bool? result = authWindow.ShowDialog();
                if (result == true)
                { 
                    if (MainWindow == null)
                    {
                        MainWindow = new MainWindow(); 
                        MainWindow.Show(); 
                    }
                }
                else
                {
                    MessageBox.Show("Авторизация не удалась.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    Shutdown();
                }
            }
            else
            {
                MessageBox.Show("Ошибка при создании окна авторизации.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                Shutdown();
            }
        }
    }
}
