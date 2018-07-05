using SoBesedkaDB.Views;
using System.Windows;

namespace SoBesedkaApp
{
    /// <summary>
    /// Логика взаимодействия для RoomsWindow.xaml
    /// </summary>
    public partial class RoomsWindow : Window
    {
        DataSource Data;
        public RoomsWindow(DataSource data)
        {
            InitializeComponent();
            Data = data;
            DataContext = Data;
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            var roomwndow = new CreateRoomWindow(Data);
            roomwndow.Show();
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (RoomsListBox.SelectedItem == null)
                return;
            RoomViewModel room = (RoomViewModel)RoomsListBox.SelectedItem;
            Data.DelElement(room);
            Data.UpdateRooms();
        }

    }
}
