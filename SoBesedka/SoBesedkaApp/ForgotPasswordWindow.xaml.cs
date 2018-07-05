using System;
using System.Text.RegularExpressions;
using System.Windows;

namespace SoBesedkaApp
{
    /// <summary>
    /// Логика взаимодействия для ForgotPasswordWindow.xaml
    /// </summary>
    public partial class ForgotPasswordWindow : Window
    {
        DataSource Data;

        public ForgotPasswordWindow(DataSource data)
        {
            InitializeComponent();
            Data = data;
        }

        private void EnterButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(EmailTextBox.Text))
            {
                MessageBox.Show("Введите Ваш E-mail адрес", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            try
            {
                string mail = EmailTextBox.Text;
                if (!string.IsNullOrEmpty(mail))
                {
                    if (!Regex.IsMatch(mail, @"^(?("")(""[^""]+?""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                    @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9]{2,17}))$"))
                    {
                        MessageBox.Show("Неверный формат электронной почты", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                }

                if (Data.RestoringPassword(mail))
                {
                    MessageBox.Show("Временный пароль выслан на указанный E-mail адрес", "Сообщение", MessageBoxButton.OK, MessageBoxImage.Information);
                    Close();
                }
                else
                {
                    MessageBox.Show("Пользовтеля с указанной почтой не существует", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                MessageBox.Show(ex.InnerException.Message);
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            EmailTextBox.Focus();
        }
    }
}
