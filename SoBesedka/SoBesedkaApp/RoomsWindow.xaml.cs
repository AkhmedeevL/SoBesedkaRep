using SoBesedkaDB;
using SoBesedkaDB.Implementations;
using SoBesedkaDB.Interfaces;
using SoBesedkaDB.Views;
using System;
using System.Windows;

namespace SoBesedkaApp
{
    /// <summary>
    /// Логика взаимодействия для RoomsWindow.xaml
    /// </summary>
    public partial class RoomsWindow : Window
    {
        DataSamples Data;
        public RoomsWindow(DataSamples data)
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
