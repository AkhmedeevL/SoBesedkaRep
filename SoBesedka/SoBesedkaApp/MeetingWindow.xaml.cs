using SoBesedkaDB;
using SoBesedkaDB.Implementations;
using SoBesedkaDB.Interfaces;
using SoBesedkaDB.Views;
using System;
using System.Windows;
using System.Windows.Controls;

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
            Data = data;
            DataContext = data;
            TimeStartTextBox.Text = meeting.StartTime.ToShortTimeString();
            var t = (meeting.EndTime - meeting.StartTime);
            DlitTextBox.Text = t.ToString("hh\\:mm");
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            Data.Mservice.AddElement(new SoBesedkaModels.Meeting
            {
                MeetingName = TitleTextBox.Text,
                MeetingTheme = SubjTextBox.Text,
                MeetingDescription = DescriptionTextBox.Text,
                StartTime = DatePicker.SelectedDate.Value + DateTime.Parse(TimeStartTextBox.Text).TimeOfDay,
                EndTime = DatePicker.SelectedDate.Value + DateTime.Parse(TimeStartTextBox.Text).TimeOfDay + DateTime.Parse(DlitTextBox.Text).TimeOfDay,
                UserMeetings = null,
                RoomId = Data.CurrentRoom.Id,
                CreatorId = Data.CurrentUser.Id
            });
            Close();
            Data.UpdateMeetings();
            Data.Uservice.SendEmail(Data.CurrentUser.UserMail, "Оповещение о создании мероприятия",
                String.Format("Мероприятие успешно добавлено. \n Название: {0}. \n Тема: {1}. \n Время: {2}. \n Место: {3}", TitleTextBox.Text, SubjTextBox.Text, TimeStartTextBox.Text, Data.CurrentRoom.RoomName));

        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
