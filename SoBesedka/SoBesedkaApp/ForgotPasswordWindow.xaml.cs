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
            EmailLabel_MouseDown(EmailLabel, null);
        }
        private void Label_MouseEnter(object sender, MouseEventArgs e)
        {
            ((Label)sender).BorderBrush = new SolidColorBrush(Colors.Black);
            ((Label)sender).BorderThickness = new Thickness(0, 0, 0, 1.0);
            Cursor = Cursors.Hand;
        }

        private void Label_MouseLeave(object sender, MouseEventArgs e)
        {
            ((Label)sender).BorderThickness = new Thickness(0);
            Cursor = Cursors.Arrow;
        }

        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            dynamic sndr;
            dynamic label;
            //if (sender is TextBox)
            //{
                sndr = (TextBox)sender;
                if (((TextBox)sndr).Text != String.Empty)
                    return;
                label = EmailLabel;
                var labelAnim = new ThicknessAnimation()
                {
                    From = label.Margin,
                    To = new Thickness(label.Margin.Left, label.Margin.Top - 25, label.Margin.Right, label.Margin.Bottom),
                    Duration = TimeSpan.FromSeconds(0.2)
                };
                label.BeginAnimation(MarginProperty, labelAnim);
            //}


            sndr.Padding = new Thickness(1, 10, 1, 0);
            var anim = new DoubleAnimation()
            {
                From = sndr.Height,
                To = 40.0,
                Duration = TimeSpan.FromSeconds(0.2)
            };
            sndr.BeginAnimation(HeightProperty, anim);
            var anim2 = new ThicknessAnimation()
            {
                From = sndr.Margin,
                To = new Thickness(sndr.Margin.Left, sndr.Margin.Top - 5, sndr.Margin.Right, sndr.Margin.Bottom),
                Duration = TimeSpan.FromSeconds(0.2)
            };
            sndr.BeginAnimation(MarginProperty, anim2);
        }

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            dynamic sndr;
           // if (sender is TextBox)
            //{
                sndr = (TextBox)sender;
                if (((TextBox)sndr).Text != String.Empty)
                    return;
                var a = new ThicknessAnimation()
                {
                    From = EmailLabel.Margin,
                    To = new Thickness(EmailLabel.Margin.Left, EmailLabel.Margin.Top + 25, EmailLabel.Margin.Right, EmailLabel.Margin.Bottom),
                    Duration = TimeSpan.FromSeconds(0.2)
                };
                EmailLabel.BeginAnimation(MarginProperty, a);
           // }


            sndr.Padding = new Thickness(1, 5, 1, 0);
            var anim = new DoubleAnimation()
            {
                From = sndr.Height,
                To = 30.0,
                Duration = TimeSpan.FromSeconds(0.2)
            };
            sndr.BeginAnimation(HeightProperty, anim);
            var anim2 = new ThicknessAnimation()
            {
                From = sndr.Margin,
                To = new Thickness(sndr.Margin.Left, sndr.Margin.Top + 5, sndr.Margin.Right, sndr.Margin.Bottom),
                Duration = TimeSpan.FromSeconds(0.2)
            };
            sndr.BeginAnimation(MarginProperty, anim2);
        }



        private void EmailLabel_MouseDown(object sender, MouseButtonEventArgs e)
        {
            EmailTextBox.Focus();
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
    }
}
