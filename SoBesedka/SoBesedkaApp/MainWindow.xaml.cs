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
            //Data.UpdateMeetings();
            if (!Data.CurrentUser.isAdmin) {
                UsersMenuItem.Visibility = Visibility.Hidden;
                RoomsMenuItem.Visibility = Visibility.Hidden;
            }

            // Raise the routed event "selected"
            RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
        }

        // Register the routed event
        public static readonly RoutedEvent SelectedEvent =
            EventManager.RegisterRoutedEvent("Click", RoutingStrategy.Bubble,
            typeof(RoutedEventHandler), typeof(Button));

        // .NET wrapper
        public event RoutedEventHandler Selected
        {
            add { AddHandler(SelectedEvent, value); }
            remove { RemoveHandler(SelectedEvent, value); }
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
            MeetingWindow meetingwindow = new MeetingWindow(Data, (MeetingViewModel)((Button)sender).Tag);
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

        private void ProfileMenuItem_Click(object sender, RoutedEventArgs e)
        {
            var wnd = new ProfileWindow(Data);
            wnd.Show();
        }
    }
}
