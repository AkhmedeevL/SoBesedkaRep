using SoBesedkaModels;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace SoBesedkaApp
{
    /// <summary>
    /// Логика взаимодействия для RegWindow.xaml
    /// </summary>
    public partial class RegWindow : Window
    {
        DataSamples Data;
        public RegWindow(DataSamples data)
        {
            Data = data;
            DataContext = Data;
            InitializeComponent();
        }

        private void EnterButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(FIOTextBox.Text))
            {
                ErrorText.Text = "Введите ФИО";
                FIOTextBox.Focus();
                return;
            }
            if (FIOTextBox.Text.Any(c => !(char.IsLetter(c) || c == ' ')))
            {
                ErrorText.Text = "Неверный формат ФИО";
                FIOTextBox.Focus();
                return;
            }
            if (string.IsNullOrEmpty(LoginTextBox.Text))
            {
                ErrorText.Text = "Введите логин";
                LoginTextBox.Focus();
                return;
            }
            if (LoginTextBox.Text.Count(c => char.IsLetterOrDigit(c) || c == '.' || c == '-' || c== '_') != LoginTextBox.Text.Count())
            {
                ErrorText.Text = "Неверный формат логина";
                FIOTextBox.Focus();
                return;
            }
            if (string.IsNullOrEmpty(EmailTextBox.Text))
            {
                ErrorText.Text = "Введите E-mail";
                EmailTextBox.Focus();
                return;
            }
            if (string.IsNullOrEmpty(PasswordTextBox.Password))
            {
                ErrorText.Text = "Введите пароль";
                PasswordTextBox.Focus();
                return;
            }
            if (PasswordTextBox.Password != SecondPasswordTextBox.Password)
            {
                ErrorText.Text = "Введённые пароли не совпадают";
                SecondPasswordTextBox.Focus();
                return;
            }
            string mail = EmailTextBox.Text;
            if (!string.IsNullOrEmpty(mail))
            {
                if (!Regex.IsMatch(mail, @"^(?("")(""[^""]+?""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9]{2,17}))$"))
                {
                    ErrorText.Text = "Неверный формат электронной почты";
                    return;
                }
            }
            if (PasswordTextBox.Password == SecondPasswordTextBox.Password)
                try
                {
                    if (!Data.AddElement(new User
                    {
                        UserFIO = FIOTextBox.Text,
                        UserMail = EmailTextBox.Text,
                        UserLogin = LoginTextBox.Text,
                        UserPassword = Data.GetHashString(PasswordTextBox.Password),
                        isAdmin = false
                    })) throw new Exception("Не удалось зарегистрироваться");
                    Data.UpdateUsers();
                    MessageBox.Show("Вы успешно зарегистрировались, используйте введённые данные для входа", "Успешно", MessageBoxButton.OK, MessageBoxImage.None);
                    Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка");
                }
        }
    }
}
