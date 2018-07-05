using SoBesedkaDB.Views;
using System;
using System.Collections.Generic;
using System.Windows;
using SoBesedkaModels;

namespace SoBesedkaApp
{
    /// <summary>
    /// Логика взаимодействия для ProfileWindow.xaml
    /// </summary>
    public partial class ProfileWindow : Window
    {
        DataSource Data;
        string CurrentFIO;
        string CurrentLogin;
        string CurrentEmail;
        string Password;

        public ProfileWindow(DataSource data)
        {
            InitializeComponent();
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
            Password = Data.CurrentUser.UserPassword;
            EmailTextBox.Focusable = false;
            EmailTextBox.IsEnabled = false;

            PasswordTextBox.Focusable = false;
            PasswordTextBox.IsEnabled = false;
            PasswordConfirmTextBox.IsEnabled = false;
            PasswordConfirmTextBox.Focusable = false;


            try
            {
                var response = APIClient.GetRequest("api/Meeting/GetListUserCreatedMeetings/" + Data.CurrentUser.Id);
                if (response.Result.IsSuccessStatusCode)
                {
                    var list = APIClient.GetElement<List<MeetingViewModel>>(response);
                    if (list != null)
                    {
                        Data.UserMeetings = list;
                    }
                }
                else
                {
                    throw new Exception(APIClient.GetError(response));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            MeetingsListBoxCreated.ItemsSource = Data.UserMeetings;


            try
            {
                var response = APIClient.GetRequest("api/Meeting/GetListUserInvites/" + Data.CurrentUser.Id);
                if (response.Result.IsSuccessStatusCode)
                {
                    var list = APIClient.GetElement<List<MeetingViewModel>>(response);
                    if (list != null)
                    {
                        Data.UserMeetings = list;
                    }
                }
                else
                {
                    throw new Exception(APIClient.GetError(response));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }

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
            else
            {
                string changed = ""; //выведем пользователю те поля, которые были изменены. изменяются только те поля, которые он ввёл в соответствующие текстбоксы
                var user = Data.CurrentUser;



                if (PasswordTextBox.Password != "")
                {
                    if (PasswordTextBox.Password == PasswordConfirmTextBox.Password)
                    {
                        user.UserPassword = Data.GetHashString(PasswordTextBox.Password);
                        changed += " Пароль; ";
                    }
                    else
                    {
                        MessageBox.Show("Введённые пароли не совпадают", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                }
                else
                {
                    user.UserPassword = Password;
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
                try
                {
                    //var response = APIClient.PostRequest("api/User/UpdElement", user);

                    //if (!response.Result.IsSuccessStatusCode)
                    //{
                    //    throw new Exception(APIClient.GetError(response));
                    //}
                    if (!Data.UpdElement(new User
                    {
                        Id = user.Id,
                        UserFIO = user.UserFIO,
                        UserLogin = user.UserLogin,
                        UserMail = user.UserMail,
                        UserPassword = user.UserPassword,
                        isAdmin = user.isAdmin
                    }))
                        throw new Exception("Пользователь с таким логином или email уже существует");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                MessageBox.Show(changed + " были обновлены", "Успешно изменено", MessageBoxButton.OK);

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
