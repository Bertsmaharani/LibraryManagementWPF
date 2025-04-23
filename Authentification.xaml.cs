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

namespace LibraryManagementWPF
{
    /// <summary>
    /// Логика взаимодействия для Authentification.xaml
    /// </summary>
    public partial class Authentification : Page
    {
        public Authentification()
        {
            InitializeComponent();
        }
        private void ButtonEnterAuthorization_Click(object sender, RoutedEventArgs e)
        {
            string username = TextBoxLogin.Text;
            string password = TextBoxPassword.Password;

            if (IsCredentialsValid(username, password))
            {
                MessageBox.Show("Добро пожаловать!");
                Content = null;
            }
            else
            {
                MessageBox.Show("Неверный логин или пароль.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private bool IsCredentialsValid(string username, string password)
        {
            return username == "dasha" && password == "pass";
        }
    }
}
