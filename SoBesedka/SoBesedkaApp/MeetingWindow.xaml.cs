using SoBesedkaDB;
using SoBesedkaDB.Implementations;
using SoBesedkaDB.Interfaces;
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
        IMeetingService Mservice;
        public MeetingWindow(DataSamples data)
        {
            InitializeComponent();
            Data = data;
            DataContext = data;
            Mservice = new MeetingService(new SoBesedkaDBContext());
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            Mservice.AddElement(new SoBesedkaModels.Meeting
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
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
