using SoBesedkaDB.Views;
using System;
using System.Windows;
using System.Windows.Controls;

namespace SoBesedkaApp
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DataSamples Data;
        
        public MainWindow(DataSamples data)
        {
            InitializeComponent();
            Data = data;
            DataContext = Data;
            Data.CurrentRoom = (RoomViewModel)ListBox1.SelectedItem;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            for(int i = 0; i < 7; i++)
            {
                Data.CurrentWeek[i] += TimeSpan.FromDays(7);
            }
            Data.RaisePropertyChanged("CurrentWeek");
            Data.UpdateMeetings();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < 7; i++)
            {
                Data.CurrentWeek[i] -= TimeSpan.FromDays(7);
            }
            Data.RaisePropertyChanged("CurrentWeek");
            Data.UpdateMeetings();
        }


        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            MeetingWindow meetingwindow = new MeetingWindow(Data);
            meetingwindow.Show();

        }

        private void Event_Click(object sender, RoutedEventArgs e)
        {
            var meetingInfo = new MeetingInfo((MeetingViewModel)((Button)sender).Tag);
            meetingInfo.Show();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            MessageBoxResult res = MessageBox.Show("Вы действительно хотите выйти?",
                                             "Выход",
                                             MessageBoxButton.YesNo,
                                             MessageBoxImage.Question);
            if (res != MessageBoxResult.Yes)
            {
                    e.Cancel = true;
            }

        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            var wnd = new UsersWindow(Data);
            wnd.Show();
        }

        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            var wnd = new RoomsWindow();
            wnd.Show();
        }

        private void ListBox1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Data.CurrentRoom = (RoomViewModel)ListBox1.SelectedItem;
            Data.UpdateMeetings();
        }
    }
}
