using SoBesedkaDB;
using SoBesedkaDB.Implementations;
using SoBesedkaDB.Interfaces;
using SoBesedkaDB.Views;
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
using System.Windows.Shapes;

namespace SoBesedkaApp
{
    /// <summary>
    /// Логика взаимодействия для ProfileWindow.xaml
    /// </summary>
    public partial class ProfileWindow : Window
    {
        DataSamples Data;
        IUserService Uservice;
        IMeetingService Mservice;
        string CurrentFIO;
        string CurrentLogin;
        string CurrentEmail;

        public ProfileWindow(DataSamples data)
        {
            InitializeComponent();
            Uservice = new UserService(new SoBesedkaDBContext());
            Mservice = new MeetingService(new SoBesedkaDBContext());
            Data = data;
            FIOTextBox.Text = Data.CurrentUser.UserFIO;
            CurrentFIO = Data.CurrentUser.UserFIO;
            FIOTextBox.Focusable = false;
            FIOTextBox.IsEnabled = false;
            LoginTextBox.Text = Data.CurrentUser.UserLogin;
            CurrentLogin = Data.CurrentUser.UserLogin;
            LoginTextBox.Focusable = false;
            LoginTextBox.IsEnabled = false;
            EmailTextBox.Text = Data.CurrentUser.UserMail;
            CurrentEmail = Data.CurrentUser.UserMail;
            EmailTextBox.Focusable = false;
            EmailTextBox.IsEnabled = false;

            PasswordTextBox.Focusable = false;
            PasswordTextBox.IsEnabled = false;
            PasswordConfirmTextBox.IsEnabled = false;
            PasswordConfirmTextBox.Focusable = false;


            Data.UserMeetings = new List<MeetingViewModel>(Mservice.GetListUserCreatedMeetings(Data.CurrentUser.Id));
            MeetingsListBoxCreated.ItemsSource = Data.UserMeetings;


            Data.UserMeetings = new List<MeetingViewModel>(Mservice.GetListUserInvites(Data.CurrentUser.Id));
            MeetingsListBoxInvited.ItemsSource = Data.UserMeetings;
        }

        private void ChangeProfileButton_Click(object sender, RoutedEventArgs e)
        {
            if (ChangeProfileButton.Content.ToString() == "Редактировать")
            {
                FIOTextBox.Focusable = true;
                FIOTextBox.IsEnabled = true;
                LoginTextBox.Focusable = true;
                LoginTextBox.IsEnabled = true;
                EmailTextBox.Focusable = true;
                EmailTextBox.IsEnabled = true;

                PasswordTextBox.Focusable = true;
                PasswordTextBox.IsEnabled = true;
                PasswordConfirmTextBox.IsEnabled = true;
                PasswordConfirmTextBox.Focusable = true;
                ChangeProfileButton.Content = "Сохранить изменения";

            }
            else {
                string changed = ""; //выведем пользователю те поля, которые были изменены. изменяются только те поля, которые он ввёл в соответствующие текстбоксы
                var user = Data.CurrentUser;
                


                    if (PasswordTextBox.Password != null)
                    {
                        if (PasswordTextBox.Password == PasswordConfirmTextBox.Password)
                        {
                            user.UserPassword = PasswordTextBox.Password;
                            changed += " Пароль; ";
                        }
                        else
                        {
                            MessageBox.Show("Введённые пароли не совпадают", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }
                    }
                    if (FIOTextBox.Text != CurrentFIO)
                    {
                        user.UserFIO = FIOTextBox.Text;
                        changed += " ФИО; ";
                    }
                    if (LoginTextBox.Text != CurrentLogin)
                    {
                        user.UserLogin = LoginTextBox.Text;
                        changed += " Логин; ";
                    }
                    if (EmailTextBox.Text != CurrentEmail)
                    {
                        user.UserMail = EmailTextBox.Text;
                        changed += " E-mail; ";
                    }
                    Uservice.UpdElement(Uservice.ConvertViewToUser(user));
                    MessageBox.Show(changed + " были обновлены", "Успешно изменено", MessageBoxButton.OK, MessageBoxImage.Error);

                    ChangeProfileButton.Content = "Редактировать";
                    FIOTextBox.Focusable = false;
                    LoginTextBox.Focusable = false;
                    EmailTextBox.Focusable = false;
                    PasswordTextBox.Focusable = false;
                    PasswordConfirmTextBox.Focusable = false;

            
            }

        }


    }
}
