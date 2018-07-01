using SoBesedkaDB;
using SoBesedkaDB.Implementations;
using SoBesedkaDB.Interfaces;
using SoBesedkaDB.Views;
using SoBesedkaModels;
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

        public ProfileWindow(DataSamples data)
        {
            InitializeComponent();
            Data = data;
            FIOTextBox.Text = Data.CurrentUser.UserFIO;
            FIOTextBox.Focusable = false;
            FIOTextBox.IsEnabled = false;
            LoginTextBox.Text = Data.CurrentUser.UserLogin;
            LoginTextBox.Focusable = false;
            LoginTextBox.IsEnabled = false;
            EmailTextBox.Text = Data.CurrentUser.UserMail;
            EmailTextBox.Focusable = false;
            EmailTextBox.IsEnabled = false;
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
                ChangeProfileButton.Content = "Сохранить изменения";

            }
            else {
                var user = Data.CurrentUser;
                user.UserFIO = FIOTextBox.Text;
                user.UserLogin = LoginTextBox.Text;
                user.UserMail = EmailTextBox.Text;

                try
                {
                    var response = APIClient.PostRequest("api/User/UpdElement", new User
                    {
                        Id = user.Id,
                        UserFIO = user.UserFIO,
                        UserLogin = user.UserLogin,
                        UserMail = user.UserMail
                    });
                    
                    if (!response.Result.IsSuccessStatusCode)
                    {
                        throw new Exception(APIClient.GetError(response));
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }

                FIOTextBox.Focusable = false;
                FIOTextBox.IsEnabled = false;
                LoginTextBox.Focusable = false;
                LoginTextBox.IsEnabled = false;
                EmailTextBox.Focusable = false;
                EmailTextBox.IsEnabled = false;
                ChangeProfileButton.Content = "Редактировать";
            }
        
        }

        private void UserMeetingsButton_Click(object sender, RoutedEventArgs e)
        {
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

            MeetingsListBox.ItemsSource = Data.UserMeetings;
        }

        private void InvitesButton_Click(object sender, RoutedEventArgs e)
        {
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

            //Data.UserMeetings = new List<MeetingViewModel>(Mservice.GetListUserInvites(Data.CurrentUser.Id));
            MeetingsListBox.ItemsSource = Data.UserMeetings;
        }
    }
}
