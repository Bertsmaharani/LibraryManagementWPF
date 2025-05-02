using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Text.Json;
using System.IO;
using Microsoft.Win32;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Windows.Input;

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
            DataContext = this;
        }
        public List<Book> books = new List<Book>();
        private string FilePath;
        private void ButtonAddBook_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(TextBoxIdBook.Text) || string.IsNullOrEmpty(TextBoxTitle.Text) ||
                string.IsNullOrEmpty(TextBoxYear.Text) || string.IsNullOrEmpty(TextBoxNameReader.Text) ||
                string.IsNullOrEmpty(TextBoxAuthor.Text) || string.IsNullOrEmpty(ComboBoxStatus.Text))
                {
                    System.Windows.MessageBox.Show("Все поля должны быть заполнены!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return; 
                }
                int id = int.Parse(TextBoxIdBook.Text);
                int year = int.Parse(TextBoxYear.Text);
                Book book = new Book
                {
                    ID = id,
                    Title = TextBoxTitle.Text,
                    Author = TextBoxAuthor.Text,
                    Year = year,
                    IsIssued = ComboBoxStatus.Text == "Доступна",
                    NameReader = TextBoxNameReader.Text
                };
                books.Add(book);
                SaveBooks(); 
                ClearInputFields();
            }
            catch (FormatException ex)
            {
                System.Windows.MessageBox.Show($"Ошибка при вводе: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            UpdateBookList();
            SaveBooks();
        }
        private void ButtonDeleteBook_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(TextBoxIdBook.Text))
                {
                    throw new Exception("Поле Id книги обязательно для заполнения!");
                }
                int id = Convert.ToInt32(TextBoxIdBook.Text);
                books.RemoveAll(b => b.ID == id);
                UpdateBookList();
                SaveBooks();
                ClearInputFields();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ButtonGiveOut_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(TextBoxIdBook.Text) || string.IsNullOrEmpty(TextBoxNameReader.Text))
                {
                    throw new Exception("Поля Id книги и ФИО обязательно для заполнения!");
                }
                int id = Convert.ToInt32(TextBoxIdBook.Text);
                string name = Convert.ToString(TextBoxNameReader.Text);
                var book = books.FirstOrDefault(b => b.ID == id);
                if (book != null && book.IsIssued)
                {
                    book.IsIssued = false;
                    UpdateBookList();
                }
                else if (book == null || !book.IsIssued)
                {
                    throw new Exception("Книга не доступна для выдачи!");
                }
                SaveBooks();
                ClearInputFields();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void ButtonReturnBook_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(TextBoxIdBook.Text))
                {
                    throw new Exception("Поле Id книги обязательно для заполнения!");
                }
                int id = Convert.ToInt32(TextBoxIdBook.Text);
                var book = books.FirstOrDefault(b => b.ID == id);
                if (book != null && !book.IsIssued)
                {
                    book.IsIssued = true;
                    UpdateBookList();
                }
                else if(book.IsIssued)
                {
                    throw new Exception("Книга не может быть вовзращена, потому что она уже доступна!");
                }
                SaveBooks();
                ClearInputFields();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void SaveBooks()
        {
            try
            {
                string json = JsonSerializer.Serialize<List<Book>>(books, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(FilePath, json);
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Ошибка при сохранении: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void UpdateBookList()
        {
            BookListView.ItemsSource = null;
            BookListView.ItemsSource = books;
        }
        private void ClearInputFields()
        {
            TextBoxIdBook.Text = string.Empty;
            TextBoxTitle.Text = string.Empty;
            TextBoxAuthor.Text = string.Empty;
            TextBoxYear.Text = string.Empty;
            TextBoxNameReader.Text = string.Empty;
        }

        private void ButtonLoadBooks_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var openFileDialog = new Microsoft.Win32.OpenFileDialog
                {
                    Filter = "JSON files (*.json)|*.json",
                    DefaultExt = ".json",
                    InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
                };

                if (openFileDialog.ShowDialog() == true)
                {
                    FilePath = openFileDialog.FileName;
                    if (File.Exists(FilePath) && FilePath.Length != 0)
                    {
                        string json = File.ReadAllText(FilePath);
                        books = JsonSerializer.Deserialize<List<Book>>(json);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Ошибка при загрузке: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ButtonSaveBooks_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var saveFileDialog = new Microsoft.Win32.SaveFileDialog
                {
                    Filter = "JSON files (*.json)|*.json",
                    DefaultExt = ".json",
                    AddExtension = true,
                    InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
                };

                if (saveFileDialog.ShowDialog() == true)
                {
                    string json = JsonSerializer.Serialize<List<Book>>(books, new JsonSerializerOptions { WriteIndented = true });
                    File.WriteAllText(saveFileDialog.FileName, json);
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Ошибка при сохранении: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ButtonExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void TextBox_PreviewIdInput(object sender, TextCompositionEventArgs e)
        {
            var regex = new Regex("[^0-9]+"); 
            e.Handled = regex.IsMatch(e.Text);
        }
        private void TextBox_PreviewAuthorInput(object sender, TextCompositionEventArgs e)
        {
            var regex = new Regex("[^а-яА-ЯЁёa-zA-Z]+");
            e.Handled = regex.IsMatch(e.Text);
        }
        private void TextBox_PreviewYearInput(object sender, TextCompositionEventArgs e)
        {
            var regex = new Regex("[^0-9]+"); 
            e.Handled = regex.IsMatch(e.Text);
        }
        private void TextBox_PreviewReaderNameInput(object sender, TextCompositionEventArgs e)
        {
            var regex = new Regex("[^а-яА-ЯЁёa-zA-Z]+");
            e.Handled = regex.IsMatch(e.Text);
        }
        private void BookListView_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (BookListView.SelectedItem is Book selectedItem)
            {
                TextBoxIdBook.Text = selectedItem.ID.ToString();
                TextBoxTitle.Text = selectedItem.Title;
                TextBoxAuthor.Text = selectedItem.Author;
                TextBoxYear.Text = selectedItem.Year.ToString();
                TextBoxNameReader.Text = selectedItem.NameReader;   
                if (selectedItem.IsIssued == true)
                {
                    ComboBoxStatus.Text = "Доступна";
                }
                else
                {
                    ComboBoxStatus.Text = "Не доступна";
                }
            }
        }

        private void ButtonEditBook_Click(object sender, RoutedEventArgs e)
        {
            if (BookListView.SelectedItem is Book selectedItem)
            {
                selectedItem.ID = Convert.ToInt32(TextBoxIdBook.Text);
                selectedItem.Title = TextBoxTitle.Text;
                selectedItem.Author = TextBoxAuthor.Text;
                selectedItem.Year = Convert.ToInt32(TextBoxYear.Text);
                selectedItem.IsIssued = Convert.ToBoolean(ComboBoxStatus);
                selectedItem.NameReader = TextBoxNameReader.Text;
                BookListView.Items.Refresh();
            }
        }
    }
}