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

        public ProfileWindow(DataSamples data)
        {
            InitializeComponent();
            Uservice = new UserService(new SoBesedkaDBContext());
            Mservice = new MeetingService(new SoBesedkaDBContext());
            Data = data;
            FIOTextBox.Text = Data.CurrentUser.UserFIO;
            FIOTextBox.Focusable = false;
            LoginTextBox.Text = Data.CurrentUser.UserLogin;
            LoginTextBox.Focusable = false;
            EmailTextBox.Text = Data.CurrentUser.UserMail;
            EmailTextBox.Focusable = false;
        }

        private void ChangeProfileButton_Click(object sender, RoutedEventArgs e)
        {
            if (ChangeProfileButton.Content.ToString() == "Редактировать")
            {
                FIOTextBox.Focusable = true;
                LoginTextBox.Focusable = true;
                EmailTextBox.Focusable = true;
                ChangeProfileButton.Content = "Сохранить изменения";
            }
            else {
                var user = Data.CurrentUser;
                user.UserFIO = FIOTextBox.Text;
                user.UserLogin = LoginTextBox.Text;
                user.UserMail = EmailTextBox.Text;
                Uservice.UpdElement(Uservice.ConvertViewToUser(user));
                FIOTextBox.Focusable = false;
                LoginTextBox.Focusable = false;
                EmailTextBox.Focusable = false;
                ChangeProfileButton.Content = "Редактировать";
            }
        
        }

        private void UserMeetingsButton_Click(object sender, RoutedEventArgs e)
        {
            Data.UserMeetings = new List<MeetingViewModel>(Mservice.GetListUserCreatedMeetings(Data.CurrentUser.Id));
            MeetingsListBox.ItemsSource = Data.UserMeetings;
        }

        private void InvitesButton_Click(object sender, RoutedEventArgs e)
        {
            Data.UserMeetings = new List<MeetingViewModel>(Mservice.GetListUserInvites(Data.CurrentUser.Id));
            MeetingsListBox.ItemsSource = Data.UserMeetings;
        }
    }
}
