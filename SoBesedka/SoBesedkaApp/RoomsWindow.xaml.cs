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
            RoomViewModel room = (RoomViewModel)RoomsListBox.SelectedItem;
            try
            {
                var response = APIClient.PostRequest("api/Room/DelElement", room);
                if (!response.Result.IsSuccessStatusCode)
                {
                    throw new Exception(APIClient.GetError(response));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            Data.UpdateRooms();
        }

    }
}
