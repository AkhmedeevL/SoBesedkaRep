using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SoBesedkaApp
{
    /// <summary>
    /// Логика взаимодействия для AuthWindow.xaml
    /// </summary>
    public partial class AuthWindow : Window
    {
        public AuthWindow()
        {
            InitializeComponent();
            LoginLabel_MouseDown(LoginLabel, null);
        }

        private void EnterButton_Click(object sender, RoutedEventArgs e)
        {
            //открываем главное окно по кнопке входа
            MainWindow mainwindow = new MainWindow();
            mainwindow.Show();
            Closing -= Window_Closing;
            Close();
        }

        private void RegLabel_Click(object sender, MouseButtonEventArgs e)
        {
            //открываем окно регистрации
            RegWindow regwindow = new RegWindow();
            regwindow.Show();
        }

        private void Label_MouseEnter(object sender, MouseEventArgs e)
        {
            ((Label) sender).BorderBrush = new SolidColorBrush(Colors.Black);
            ((Label) sender).BorderThickness = new Thickness(0, 0, 0, 1.0);
            Cursor = Cursors.Hand;
        }

        private void Label_MouseLeave(object sender, MouseEventArgs e)
        {
            ((Label) sender).BorderThickness = new Thickness(0);
            Cursor = Cursors.Arrow;
        }

        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            dynamic sndr;
            dynamic label;
            if (sender is TextBox)
            {
                sndr = (TextBox) sender;
                if (((TextBox) sndr).Text != String.Empty)
                    return;
                label = LoginLabel;
                var labelAnim = new ThicknessAnimation()
                {
                    From = label.Margin,
                    To = new Thickness(label.Margin.Left, label.Margin.Top - 25, label.Margin.Right, label.Margin.Bottom),
                    Duration = TimeSpan.FromSeconds(0.2)
                };
                label.BeginAnimation(MarginProperty, labelAnim);
            }
            else
            {
                sndr = (PasswordBox) sender;
                if (((PasswordBox) sndr).Password != String.Empty)
                    return;
                label = PassLabel;
                var labelAnim = new ThicknessAnimation()
                {
                    From = label.Margin,
                    To = new Thickness(label.Margin.Left, label.Margin.Top - 25, label.Margin.Right, label.Margin.Bottom),
                    Duration = TimeSpan.FromSeconds(0.2)
                };
                label.BeginAnimation(MarginProperty, labelAnim);
            }

            sndr.Padding = new Thickness(1,10,1,0);
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
            if (sender is TextBox)
            {
                sndr = (TextBox) sender;
                if (((TextBox) sndr).Text != String.Empty)
                    return;
                var a = new ThicknessAnimation()
                {
                    From = LoginLabel.Margin,
                    To = new Thickness(LoginLabel.Margin.Left, LoginLabel.Margin.Top + 25, LoginLabel.Margin.Right, LoginLabel.Margin.Bottom),
                    Duration = TimeSpan.FromSeconds(0.2)
                };
                LoginLabel.BeginAnimation(MarginProperty, a);
            }
            else
            {
                sndr = (PasswordBox) sender;
                if (((PasswordBox) sndr).Password != String.Empty)
                    return;
                var a = new ThicknessAnimation()
                {
                    From = PassLabel.Margin,
                    To = new Thickness(PassLabel.Margin.Left, PassLabel.Margin.Top + 25, PassLabel.Margin.Right, PassLabel.Margin.Bottom),
                    Duration = TimeSpan.FromSeconds(0.2)
                };
                PassLabel.BeginAnimation(MarginProperty, a);
            }

            sndr.Padding = new Thickness(1,5,1,0);
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

        private void PassLabel_MouseDown(object sender, MouseButtonEventArgs e)
        {
            PasswordTextBox.Focus();
        }

        private void LoginLabel_MouseDown(object sender, MouseButtonEventArgs e)
        {
            LoginTextBox.Focus();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            MessageBoxResult res = MessageBox.Show("Вы действительно хотите выйти?",
                                 "Выход",
                                 MessageBoxButton.YesNo,
                                 MessageBoxImage.Question);
            if (res != MessageBoxResult.Yes)
            {
                e.Cancel = true;
            }
        }
    }
}
