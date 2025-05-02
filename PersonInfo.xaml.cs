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
    /// Логика взаимодействия для PersonInfo.xaml
    /// </summary>
    public partial class PersonInfo : Page
    {
        public PersonInfo()
        {
            InitializeComponent();
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
                if (book != null && book.IsIssued && book.Quantity > 0 && book.Quantity < book.MaxQuantity)
                {
                    book.Quantity++;
                    UpdateBookList();
                }
                else if (book != null && !book.IsIssued && book.Quantity == 0 && book.Quantity < book.MaxQuantity)
                {
                    book.IsIssued = true;
                    book.Quantity++;
                    UpdateBookList();
                }
                else if (book.Quantity == book.MaxQuantity)
                {
                    throw new Exception("Книга не может быть вовзращена, потому что достигнуто максимальное количество!");
                }
                SaveBooks();
                ClearInputFields();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
