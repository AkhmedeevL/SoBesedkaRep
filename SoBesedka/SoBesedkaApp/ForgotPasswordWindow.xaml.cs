using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace SoBesedkaApp
{
    /// <summary>
    /// Логика взаимодействия для ForgotPasswordWindow.xaml
    /// </summary>
    public partial class ForgotPasswordWindow : Window
    {
        public ForgotPasswordWindow()
        {
            InitializeComponent();
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
                MessageBox.Show("Временный пароль выслан на указанный E-mail адрес", "Сообщение", MessageBoxButton.OK, MessageBoxImage.Information);
                Close();
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
