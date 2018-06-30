using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace SoBesedkaApp
{
    /// <summary>
    /// Логика взаимодействия для RegWindow.xaml
    /// </summary>
    public partial class RegWindow : Window
    {
        public RegWindow()
        {
            InitializeComponent();
        }

        private void EnterButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(FIOTextBox.Text))
            {
                MessageBox.Show("Введите ФИО", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (string.IsNullOrEmpty(LoginTextBox.Text))
            {
                MessageBox.Show("Введите логин", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (string.IsNullOrEmpty(EmailTextBox.Text))
            {
                MessageBox.Show("Введите E-mail", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (string.IsNullOrEmpty(PasswordTextBox.Password))
            {
                MessageBox.Show("Введите пароль", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (PasswordTextBox.Password != SecondPasswordTextBox.Password)
            {
                MessageBox.Show("Введённые пароли не совпадают", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
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
            if (PasswordTextBox.Password == SecondPasswordTextBox.Password)
                Uservice.AddElement(new User
                {
                    UserFIO = FIOTextBox.Text,
                    UserMail = EmailTextBox.Text,
                    UserLogin = LoginTextBox.Text,
                    UserPassword = PasswordTextBox.Password,
                    isAdmin = false
                });
            MessageBox.Show("Вы успешно зарегистрировались, используйте введённые данные для входа", "Успешно", MessageBoxButton.OK, MessageBoxImage.None);
            Close();
            var aut = new AuthWindow();
            aut.Show();
        }

        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            dynamic sndr;
            if (sender is TextBox)
            {
                sndr = (TextBox)sender;
                if (sndr.Text != String.Empty)
                    return;
            }
            else
            {
                sndr = (PasswordBox)sender;
                if (sndr.Password != String.Empty)
                    return;
            }
            sndr.Padding = new Thickness(1, 10, 1, 0);
            var anim = new DoubleAnimation()
            {
                From = sndr.Height,
                To = 40.0,
                Duration = TimeSpan.FromSeconds(0.2)
            };
            sndr.BeginAnimation(HeightProperty, anim);
            var label = (Label)sndr.Tag;

            var a = new ThicknessAnimation()
            {
                From = label.Margin,
                To = new Thickness(label.Margin.Left, label.Margin.Top - 25, label.Margin.Right, label.Margin.Bottom),
                Duration = TimeSpan.FromSeconds(0.2)
            };
            label.BeginAnimation(MarginProperty, a);
        }

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            dynamic sndr;
            if (sender is TextBox)
            {
                sndr = (TextBox)sender;
                if (sndr.Text != String.Empty)
                    return;
            }
            else
            {
                sndr = (PasswordBox)sender;
                if (sndr.Password != String.Empty)
                    return;
            }
            sndr.Padding = new Thickness(1, 5, 1, 0);
            var anim = new DoubleAnimation()
            {
                From = sndr.Height,
                To = 30.0,
                Duration = TimeSpan.FromSeconds(0.2)
            };
            sndr.BeginAnimation(HeightProperty, anim);
            var label = (Label)sndr.Tag;

            var a = new ThicknessAnimation
            {
                From = label.Margin,
                To = new Thickness(label.Margin.Left, label.Margin.Top + 25, label.Margin.Right, label.Margin.Bottom),
                Duration = TimeSpan.FromSeconds(0.2)
            };
            a.Completed += anim_Completed;
            label.BeginAnimation(MarginProperty, a);
        }

        private void anim_Completed(object sender, EventArgs e)
        {

            //RegGrid.Children.Remove((Label)sndr.Tag);
        }

        private void Label_GotFocus(object sender, RoutedEventArgs e)
        {
            var label = (Label)sender;

            var a = new ThicknessAnimation
            {
                From = label.Margin,
                To = new Thickness(label.Margin.Left, label.Margin.Top - 25, label.Margin.Right, label.Margin.Bottom),
                Duration = TimeSpan.FromSeconds(0.2)
            };

            ((TextBox)label.Tag).Focus();
        }

        private void label_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var sndr = (Label)sender;
            if (sndr.Tag is TextBox)
                ((TextBox)sndr.Tag).Focus();
            else
                ((PasswordBox)sndr.Tag).Focus();
        }
    }
}
