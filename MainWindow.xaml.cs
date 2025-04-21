using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Text.Json;
using System.IO;
using Microsoft.Win32;
using System.Windows.Forms;

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
        private string FilePath;
        private void ButtonAddBook_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(TextBoxIdBook.Text) || string.IsNullOrEmpty(TextBoxTitle.Text) ||
                string.IsNullOrEmpty(TextBoxYear.Text) || string.IsNullOrEmpty(TextBoxQuantity.Text) ||
                string.IsNullOrEmpty(TextBoxAuthor.Text) || string.IsNullOrEmpty(ComboBoxStatus.Text))
                {
                    System.Windows.MessageBox.Show("Все поля должны быть заполнены!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
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
                TextBoxIdBook.Text = string.Empty;
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
                if (string.IsNullOrEmpty(TextBoxIdBook.Text))
                {
                    throw new Exception("Поле Id книги обязательно для заполнения!");
                }
                int id = Convert.ToInt32(TextBoxIdBook.Text);
                var book = books.FirstOrDefault(b => b.ID == id);
                if (book != null && book.IsIssued && book.Quantity == 1)
                {
                    book.IsIssued = false;
                    book.Quantity--;
                    UpdateBookList();
                }
                else if (book != null && book.IsIssued && book.Quantity > 1)
                {
                    book.Quantity--;
                    UpdateBookList();
                }
                TextBoxIdBook.Text = string.Empty;
                SaveBooks();
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
                if (book != null && book.IsIssued && book.Quantity > 0)
                {
                    book.Quantity++;
                    UpdateBookList();
                }
                else if (book != null && !book.IsIssued && book.Quantity == 0)
                {
                    book.IsIssued = true;
                    book.Quantity++;
                    UpdateBookList();
                }
                TextBoxIdBook.Text = string.Empty;
                SaveBooks();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void LoadBooks()
        {
            try
            { 
                if (File.Exists(FilePath) && FilePath.Length != 0)
                {
                    string json = File.ReadAllText(FilePath);
                    books = JsonSerializer.Deserialize<List<Book>>(json);
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Ошибка при загрузке: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            UpdateBookList();
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
            TextBoxQuantity.Text = string.Empty;
        }

        private void ButtonLoadBooks_Click(object sender, RoutedEventArgs e)
        {
            LoadBooks();
        }

        private void ButtonSaveBooks_Click(object sender, RoutedEventArgs e)
        {
            SaveBooks();    
        }

        private void ButtonExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void ButtonSelectFile_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog
            {
                Title = "Выбор файла",
                Filter = "Все файлы (*.*)|*.*"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                System.Windows.MessageBox.Show("Выбранный файл: " + openFileDialog.FileName);
            }
        }

        private void ButtonSelectPath_Click(object sender, RoutedEventArgs e)
        {
            using (System.Windows.Forms.FolderBrowserDialog folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog())
            {
                folderBrowserDialog.Description = "Выберите папку";

                if (folderBrowserDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    FilePath = folderBrowserDialog.SelectedPath; 
                    System.Windows.MessageBox.Show("Выбранная папка: " + folderBrowserDialog.SelectedPath);
                }
            }
        }
    }
}