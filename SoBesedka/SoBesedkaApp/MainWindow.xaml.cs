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
            if (!Data.CurrentUser.isAdmin) {
                UsersMenuItem.Visibility = Visibility.Hidden;
                RoomsMenuItem.Visibility = Visibility.Hidden;
            }

            // Raise the routed event "selected"
            RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
        }

        private void NextWeek_Click(object sender, RoutedEventArgs e)
        {
            for(int i = 0; i < 7; i++)
            {
                Data.CurrentWeek[i] += TimeSpan.FromDays(7);
            }
            Data.RaisePropertyChanged("CurrentWeek");
            Data.UpdateMeetings();
        }

        private void PrevWeek_Click(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < 7; i++)
            {
                Data.CurrentWeek[i] -= TimeSpan.FromDays(7);
            }
            Data.RaisePropertyChanged("CurrentWeek");
            Data.UpdateMeetings();
        }

        private void Event_Click(object sender, RoutedEventArgs e)
        {
            var btn = (Button) sender;
            if (((MeetingViewModel) btn.Tag).CreatorId == Data.CurrentUser.Id || Data.CurrentUser.isAdmin == true || ((MeetingViewModel)btn.Tag).Id == 0)
            {
                var meetingwnd = new MeetingWindow(Data, (MeetingViewModel) btn.Tag);
                if (meetingwnd.ShowDialog() == true)
                     Data.UpdateMeetings();
                return;
            }
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
            else
            {
                Environment.Exit(0);
            }

        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            var wnd = new UsersWindow(Data);
            wnd.Show();
        }

        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            var wnd = new RoomsWindow(Data);
            wnd.Show();
        }

        private void ListBox1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ListBox1.SelectedItem == null)
                return;
            Data.CurrentRoom = (RoomViewModel)ListBox1.SelectedItem;
            Data.UpdateMeetings();
        }

        private void ProfileMenuItem_Click(object sender, RoutedEventArgs e)
        {
            var wnd = new ProfileWindow(Data);
            wnd.Show();
        }
    }
}
