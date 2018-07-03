using System.Collections.Generic;
using SoBesedkaDB.Views;
using System.Windows;
using CheckBox = System.Windows.Controls.CheckBox;

namespace SoBesedkaApp
{
    /// <summary>
    /// Логика взаимодействия для MeetingInfo.xaml
    /// </summary>
    public partial class MeetingInfo : Window
    {
        DataSamples Data;
        MeetingViewModel Meeting;
        List<UserViewModel> InvitedUsers;

        public MeetingInfo(DataSamples data, MeetingViewModel meeting)
        {
            InitializeComponent();

            Meeting = meeting;
            Data = data;
            DataContext = data;
            TitleTextBox.Text = Meeting.MeetingName;
            SubjTextBox.Text = Meeting.MeetingTheme;
            DescriptionTextBox.Text = Meeting.MeetingDescription;
            DatePicker.Text = Meeting.StartTime.Date.ToShortDateString();
            int i = 0;
            if (Meeting.RepeatingDays != null)
                foreach (CheckBox cb in CheckBoxContainer.Children)
                {
                    if (Meeting.RepeatingDays[i] == '1')
                        cb.IsChecked = true;
                    i++;
                }
            Data.UpdateUsers();
            if (Meeting.UserMeetings != null)
                foreach (UserMeetingViewModel um in Meeting.UserMeetings)
                {
                    var user = Data.Users.Find(u => u.Id == um.UserId);
                    if (user != null)
                        InvitedUsersListBox.Items.Add(user);
                }

            TimeStartTextBox.Text = meeting.StartTime.ToString("hh\\:mm") + " - " + meeting.EndTime.ToString("hh\\:mm");
        }
    }
}
