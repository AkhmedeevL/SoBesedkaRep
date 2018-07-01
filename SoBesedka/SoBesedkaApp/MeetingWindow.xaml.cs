using SoBesedkaDB;
using SoBesedkaDB.Implementations;
using SoBesedkaDB.Interfaces;
using SoBesedkaDB.Views;
using SoBesedkaModels;
using System;
using System.Collections.Generic;
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
            try
            {
                List<UserMeeting> userMeetings = new List<UserMeeting>();
                //тут должно быть заполнение листа с участниками
                if (DatePicker.SelectedDate != null &&
                    !string.IsNullOrEmpty(TimeStartTextBox.Text) &&
                    !string.IsNullOrEmpty(DlitTextBox.Text))
                {
                    var response = APIClient.PostRequest("api/Meeting/AddElement", new Meeting
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
                    if (response.Result.IsSuccessStatusCode)
                    {
                        MessageBox.Show("Сохранение прошло успешно", "Сообщение", MessageBoxButton.OK, MessageBoxImage.Information);
                        Close();
                    }
                    else
                    {
                        throw new Exception(APIClient.GetError(response));
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OKCancel);
            }
            Close();
            Data.UpdateMeetings();
            //Data.Uservice.SendEmail(Data.CurrentUser.UserMail, "Оповещение о создании мероприятия",
                //String.Format("Мероприятие успешно добавлено. \n Название: {0}. \n Тема: {1}. \n Время: {2}. \n Место: {3}", TitleTextBox.Text, SubjTextBox.Text, TimeStartTextBox.Text, Data.CurrentRoom.RoomName));

        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
