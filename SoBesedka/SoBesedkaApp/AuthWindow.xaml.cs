using SoBesedkaDB.Interfaces;
using SoBesedkaDB.Views;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace SoBesedkaApp
{
    /// <summary>
    /// Логика взаимодействия для AuthWindow.xaml
    /// </summary>
    public partial class AuthWindow : Window
    {
        DataSamples Data;
        public AuthWindow()
        {
            InitializeComponent();
            
            Data = new DataSamples();
        }

        private void EnterButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(LoginTextBox.Text))
            {
                MessageBox.Show("Введите логин", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (string.IsNullOrEmpty(PasswordTextBox.Password))
            {
                MessageBox.Show("Введите пароль", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            var user = new UserViewModel();
            //открываем главное окно по кнопке входа
            if (Data.SignIn(LoginTextBox.Text, PasswordTextBox.Password))
            {
                MainWindow mainwindow = new MainWindow(Data);
                mainwindow.Show();
                Closing -= Window_Closing;
                Close();
            }
            else {
                MessageBox.Show("Неверный логин или пароль");
            }
        }

        private void RegLabel_Click(object sender, MouseButtonEventArgs e)
        {
            //открываем окно регистрации
            RegWindow regwindow = new RegWindow(Data);
            regwindow.Show();
        }
        private void ForgotPasswordLabel_Click(object sender, MouseButtonEventArgs e)
        {
            //открываем окно восстановления пароля
            ForgotPasswordWindow fpwindow = new ForgotPasswordWindow(Data);
            fpwindow.Show();
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

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoginTextBox.Focus();
        }
    }
}
