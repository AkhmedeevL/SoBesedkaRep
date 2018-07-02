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
        public MeetingWindow(DataSamples data, MeetingViewModel meeting)
        {
            InitializeComponent();
            MaskedTextBox startTimeMaskedTextBox = new MaskedTextBox("00:00")
            {
                Dock = DockStyle.Fill,
                Width = (int)host.Width,
                Height = (int)host.Height,
                Font = new System.Drawing.Font("Segoe UI", 12),
                BorderStyle = BorderStyle.FixedSingle 
            };
            host.Child = startTimeMaskedTextBox;

            Data = data;
            DataContext = data;
            TitleTextBox.Text = meeting.MeetingName;
            SubjTextBox.Text = meeting.MeetingTheme;
            DescriptionTextBox.Text = meeting.MeetingDescription;
            DatePicker.SelectedDate = meeting.StartTime.Date;

            startTimeMaskedTextBox.Text = meeting.StartTime.ToShortTimeString();

            TimeStartTextBox.Text = meeting.StartTime.ToShortTimeString();
            var t = meeting.EndTime - meeting.StartTime;
            DlitTextBox.Text = t.ToString("hh\\:mm");
            InvitedUsersListBox.ItemsSource = meeting.UserMeetings;

        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                List<UserMeeting> userMeetings = new List<UserMeeting>();
                foreach (UserViewModel user in InvitedUsersListBox.Items)
                {
                    userMeetings.Add(new UserMeeting
                    {
                        UserId = user.Id
                    });
                }

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

                if (DatePicker.SelectedDate != null &&
                    !string.IsNullOrEmpty(TimeStartTextBox.Text) &&
                    !string.IsNullOrEmpty(DlitTextBox.Text))
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
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OKCancel);
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
            if (users.ShowDialog() == true)
            {
                InvitedUsersListBox.ItemsSource = users.SelectedUsers;
            }
        }
    }
}
