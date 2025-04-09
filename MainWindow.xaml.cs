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
using System.Text.Json;
using System.IO;

namespace LibraryManagementWPF
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            LoadBooks(); 
            DataContext = this;
        }
        public List<Book> books = new List<Book>();
        private void ButtonAddBook_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(TextBoxIdBook.Text) || string.IsNullOrEmpty(TextBoxTitle.Text) ||
                string.IsNullOrEmpty(TextBoxYear.Text) || string.IsNullOrEmpty(TextBoxQuantity.Text) ||
                string.IsNullOrEmpty(TextBoxAuthor.Text) || string.IsNullOrEmpty(ComboBoxStatus.Text))
                {
                    MessageBox.Show("Все поля должны быть заполнены!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return; 
                }

                int id = int.Parse(TextBoxIdBook.Text);
                int year = int.Parse(TextBoxYear.Text);
                int quantity = int.Parse(TextBoxQuantity.Text);

                Book book = new Book
                {
                    ID = id,
                    Title = TextBoxTitle.Text,
                    Author = TextBoxAuthor.Text,
                    Year = year,
                    Quantity = quantity,
                    IsIssued = ComboBoxStatus.Text == "Доступна"
                };
                books.Add(book);
                SaveBooks(); 
                ClearInputFields();

            }
            catch (FormatException ex)
            {
                MessageBox.Show($"Ошибка при вводе: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            UpdateBookList();
        }
        private void ButtonDeleteBook_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ButtonGiveOut_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ButtonRerutnBook_Click(object sender, RoutedEventArgs e)
        {

        }
        private void SaveBooks()
        {
            string filePath = "BooksDataFile.json"; 
            try
            {
                string json = JsonSerializer.Serialize<List<Book>>(books, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(filePath, json);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void UpdateBookList()
        {
            BookListView.ItemsSource = books;
        }
        private void ClearInputFields()
        {
            TextBoxIdBook.Text = string.Empty;
            TextBoxTitle.Text = string.Empty;
            TextBoxAuthor.Text = string.Empty;
            TextBoxYear.Text = string.Empty;
            TextBoxQuantity.Text = string.Empty;
        }
    }
}