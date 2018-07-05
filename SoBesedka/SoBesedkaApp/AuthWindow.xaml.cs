using SoBesedkaDB.Views;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace SoBesedkaApp
{
    /// <summary>
    /// Логика взаимодействия для AuthWindow.xaml
    /// </summary>
    public partial class AuthWindow : Window
    {
        public bool indicator = false;
        DataSource Data;
        public AuthWindow()
        {
            InitializeComponent();

            Data = new DataSource();
            if (System.IO.File.Exists(@"login.txt") && System.IO.File.Exists(@"password.txt"))
            {
                LoginTextBox.Text = System.IO.File.ReadAllText(@"login.txt");
                PasswordTextBox.Password = Uncoding(System.IO.File.ReadAllText(@"password.txt"));
                SavePassCheckBox.IsChecked = true;
                LoginLabel.Margin = new Thickness(72, 92, 0, 0);
                PassLabel.Margin = new Thickness(72, 162, 0, 0);
                indicator = true;
            }
        }

        public static string Coding(string password)     //процедура "Шифрование". используем шифр Виженера.
        {
            string key = "vtyzpjdenbujhm";
            string all = @"`1234567890-=~!@#$%^&*()_+qwertyuiop[]QWERTYUIOP{}asdfghjkl;'\ASDFGHJKL:""|ZXCVBNM<>?zxcvbnm,./№ёЁйцукенгшщзхъЙЦУКЕНГШЩЗХЪфывапролджэФЫВАПРОЛДЖЭячсмитьбюЯЧСМИТЬБЮ";//все символы, которые могут быть использовани при вводе пароляя
            string st; int center;                           //объявление новых переменных.
            string leftSlice, rightSlice, cPass = "";

            if (key.Length > password.Length)               //если длина строки пароля (ключа для входа в программу и для шифрования)>длины строки пароля (какого-либо сайта и т.д.),
            {
                key = key.Substring(0, password.Length);    //то переменная key обрежется и станет равной длинне пароля 
            }
            else                                            // Иначе повторять ключ (ключключключклю), пока не станет равным длинне пароля
                for (int i = 0; key.Length < password.Length; i++)
                {
                    key = key + key.Substring(i, 1);
                }
            //основной цикл шифрования
            for (int i = 0; i < password.Length; i++)
            {//находим центр строки all (центр - это будущий первый символ строки со сдвигом)
                center = all.IndexOf(key.Substring(i, 1));
                leftSlice = all.Substring(center); //берем левую часть будущей строки со сдвигом
                rightSlice = all.Substring(0, center);// затем правую
                st = leftSlice + rightSlice;// формируем строку со сдвигом
                center = all.IndexOf(password.Substring(i, 1));// теперь в переменную center запишем индекс очередного символа шифруемой строки
                cPass += st.Substring(center, 1);    //поскольку индексы символа из строки со сдвигом и из обычной строки совпадают, то нужный нам символ берется по такому же индексу
            }

            return cPass;
        }

        public static string Uncoding(string password)        //процедура "Расшифрование"
        {
            string key = "vtyzpjdenbujhm";
            // строка all содержит все символы, которые можно вводить с русской и англ раскладки клавиатуры
            string all = @"`1234567890-=~!@#$%^&*()_+qwertyuiop[]QWERTYUIOP{}asdfghjkl;'\ASDFGHJKL:""|ZXCVBNM<>?zxcvbnm,./№ёЁйцукенгшщзхъЙЦУКЕНГШЩЗХЪфывапролджэФЫВАПРОЛДЖЭячсмитьбюЯЧСМИТЬБЮ";
            //строка st со сдвигом по ключу (в качестве ключа используем наш пароль для входа)
            string st; int center; // центр указывает на индекс символа, до которого идет сдвиг по ключу.
            string leftSlice, rightSlice, cPass = ""; //leftSlice, rightSlice - правый срез, левый срез. из них составляется строка со сдвигом st. 

            //если пароль короче ключа - обрезаем ключ
            if (key.Length > password.Length)
            {
                key = key.Substring(0, password.Length);
            }
            //Иначе повторяем ключ, пока он не примет длинну пароля.
            else
                for (int i = 0; key.Length < password.Length; i++)
                {
                    key = key + key.Substring(i, 1);
                }
            // основной цикл расшифрования.
            for (int i = 0; i < password.Length; i++)
            {
                //находим центр строки all (центр - это будущий первый символ строки со сдвигом)
                center = all.IndexOf(key.Substring(i, 1));
                leftSlice = all.Substring(center); //берем левую часть будущей строки со сдвигом
                rightSlice = all.Substring(0, center);// затем правую
                st = leftSlice + rightSlice; // формируем строку со сдвигом
                center = st.IndexOf(password.Substring(i, 1)); // теперь в переменную center запишем индекс очередного символа расшифроввываемой строки
                cPass += all.Substring(center, 1); //поскольку индексы символа из строки со сдвигом и из обычной строки совпадают, то нужный нам символ берется по такому же индексу
            }
            return cPass; //возвращаем расшифрованный пароль.
        }


        private void EnterButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(LoginTextBox.Text))
            {
                MessageBox.Show("Введите логин ", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (string.IsNullOrEmpty(PasswordTextBox.Password))
            {
                MessageBox.Show("Введите пароль", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            var user = new UserViewModel();
            //открываем главное окно по кнопке входа
            if (Data.SignIn(LoginTextBox.Text, Data.GetHashString(PasswordTextBox.Password)))
            {
                MainWindow mainwindow = new MainWindow(Data);
                mainwindow.Show();
                if (SavePassCheckBox.IsChecked == true)
                {
                    System.IO.File.WriteAllText(@"login.txt", LoginTextBox.Text);
                    System.IO.File.WriteAllText(@"password.txt", Coding(PasswordTextBox.Password));
                }
                else
                {
                    System.IO.File.Delete(@"login.txt");
                    System.IO.File.Delete(@"password.txt");
                }
                Closing -= Window_Closing;
                Close();
            }
            else
            {
                MessageBox.Show("Неверный логин/E-mail или пароль");
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
            ((Label)sender).BorderBrush = new SolidColorBrush(Colors.Black);
            ((Label)sender).BorderThickness = new Thickness(0, 0, 0, 1.0);
            Cursor = Cursors.Hand;
        }

        private void Label_MouseLeave(object sender, MouseEventArgs e)
        {

            ((Label)sender).BorderThickness = new Thickness(0);
            Cursor = Cursors.Arrow;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (SavePassCheckBox.IsChecked == false)
            {
                if (System.IO.File.Exists(@"login.txt") && System.IO.File.Exists(@"password.txt"))
                {
                    System.IO.File.Delete(@"login.txt");
                    System.IO.File.Delete(@"password.txt");
                }
            }
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
