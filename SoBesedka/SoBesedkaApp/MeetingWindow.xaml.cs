using SoBesedkaDB;
using SoBesedkaDB.Implementations;
using SoBesedkaDB.Interfaces;
using SoBesedkaDB.Views;
using SoBesedkaModels;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Forms.Integration;
using CheckBox = System.Windows.Controls.CheckBox;
using MessageBox = System.Windows.MessageBox;

namespace SoBesedkaApp
{
    /// <summary>
    /// Логика взаимодействия для EventWindow.xaml
    /// </summary>
    public partial class MeetingWindow : Window
    {
        DataSamples Data;
        MeetingViewModel Meeting;
        List<UserViewModel> InvitedUsers;
        int? id;
        public MeetingWindow(DataSamples data, MeetingViewModel meeting)
        {
            InitializeComponent();
            //MaskedTextBox startTimeMaskedTextBox = new MaskedTextBox("00:00")
            //{
            //    Dock = DockStyle.Fill,
            //    Width = (int)host.Width,
            //    Height = (int)host.Height,
            //    Font = new System.Drawing.Font("Segoe UI", 12),
            //    BorderStyle = BorderStyle.FixedSingle 
            //};
            //host.Child = startTimeMaskedTextBox;
            Meeting = meeting;
            id = Meeting.Id;
            Data = data;
            DataContext = data;
            TitleTextBox.Text = Meeting.MeetingName;
            SubjTextBox.Text = Meeting.MeetingTheme;
            DescriptionTextBox.Text = Meeting.MeetingDescription;
            DatePicker.SelectedDate = Meeting.StartTime.Date;
            int i = 0;
            if (Meeting.RepeatingDays != null)
                foreach (CheckBox cb in CheckBoxContainer.Children)
                {
                    if (Meeting.RepeatingDays[i] == '1')
                        cb.IsChecked = true;
                    i++;
                }
            foreach(UserMeetingViewModel um in Meeting.UserMeetings)
            {
                var response = APIClient.GetRequest("api/User/Get/" + um.UserId);
                var user = APIClient.GetElement<UserViewModel>(response);
                if (user != null)
                    InvitedUsersListBox.Items.Add(user);
            }

            //startTimeMaskedTextBox.Text = meeting.StartTime.ToShortTimeString();

            TimeStartTextBox.Text = meeting.StartTime.ToShortTimeString();
            var t = meeting.EndTime - meeting.StartTime;
            DlitTextBox.Text = t.ToString("hh\\:mm");
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var repDays = "";
                foreach (CheckBox cb in CheckBoxContainer.Children)
                {
                    if (cb.IsChecked != null)
                        if (cb.IsChecked.Value)
                        {
                            repDays += "1";
                        }
                        else
                        {
                            repDays += "0";
                        }
                }
                var userMeetings = new List<UserMeeting>();
                foreach (UserViewModel user in InvitedUsersListBox.Items)
                {
                    if (Meeting.Id > 0)
                        userMeetings.Add(new UserMeeting
                        {
                            UserId = user.Id,
                        });
                    else
                        userMeetings.Add(new UserMeeting
                        {
                            UserId = user.Id,
                            MeetingId = Meeting.Id
                        });
                }
                if (Meeting.Id > 0)
                {
                    //Изменение
                    if (DatePicker.SelectedDate != null &&
                        !string.IsNullOrEmpty(TimeStartTextBox.Text) &&
                        !string.IsNullOrEmpty(DlitTextBox.Text) &&
                        !string.IsNullOrEmpty(TitleTextBox.Text) &&
                        !string.IsNullOrEmpty(SubjTextBox.Text) &&
                        !string.IsNullOrEmpty(DescriptionTextBox.Text))
                    {
                        var response = APIClient.PostRequest("api/Meeting/UpdElement", new Meeting
                        {
                            Id = Meeting.Id,
                            MeetingName = TitleTextBox.Text,
                            MeetingTheme = SubjTextBox.Text,
                            MeetingDescription = DescriptionTextBox.Text,
                            StartTime = DatePicker.SelectedDate.Value + DateTime.Parse(TimeStartTextBox.Text).TimeOfDay,
                            EndTime = DatePicker.SelectedDate.Value + DateTime.Parse(TimeStartTextBox.Text).TimeOfDay +
                                      DateTime.Parse(DlitTextBox.Text).TimeOfDay,
                            UserMeetings = userMeetings,
                            RoomId = Data.CurrentRoom.Id,
                            CreatorId = Data.CurrentUser.Id,
                            RepeatingDays = repDays
                        });
                    }
                }
                else
                {
                    //Добавление
                    if (DatePicker.SelectedDate != null &&
                        !string.IsNullOrEmpty(TimeStartTextBox.Text) &&
                        !string.IsNullOrEmpty(DlitTextBox.Text) &&
                        !string.IsNullOrEmpty(TitleTextBox.Text) &&
                        !string.IsNullOrEmpty(SubjTextBox.Text) &&
                        !string.IsNullOrEmpty(DescriptionTextBox.Text))
                    {
                        Data.AddElement(new Meeting
                        {
                            MeetingName = TitleTextBox.Text,
                            MeetingTheme = SubjTextBox.Text,
                            MeetingDescription = DescriptionTextBox.Text,
                            StartTime = DatePicker.SelectedDate.Value + DateTime.Parse(TimeStartTextBox.Text).TimeOfDay,
                            EndTime = DatePicker.SelectedDate.Value + DateTime.Parse(TimeStartTextBox.Text).TimeOfDay +
                                      DateTime.Parse(DlitTextBox.Text).TimeOfDay,
                            UserMeetings = userMeetings,
                            RoomId = Data.CurrentRoom.Id,
                            CreatorId = Data.CurrentUser.Id,
                            RepeatingDays = repDays
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OKCancel);
                return;
            }
            Close();
            Data.UpdateMeetings();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            var users = new AddUsers(Data);
            foreach (var user in InvitedUsersListBox.Items)
            {
                users.SelectedUsers.Add((UserViewModel)user);
            }
            users.OnPropertyChanged("SelectedUsers");
            users.SelectedUsersListBox.Items.Refresh();
            if (users.ShowDialog() == true)
            {
                InvitedUsersListBox.Items.Clear();
                foreach(var user in users.SelectedUsers)
                {
                    InvitedUsersListBox.Items.Add(user);
                }
            }
        }
    }
}
