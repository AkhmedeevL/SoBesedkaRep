using SoBesedkaDB;
using SoBesedkaModels;
using SoBesedkaDB.Implementations;
using SoBesedkaDB.Interfaces;
using System.Windows;

namespace SoBesedkaApp
{
    /// <summary>
    /// Логика взаимодействия для CreateRoomWindow.xaml
    /// </summary>
    public partial class CreateRoomWindow : Window
    {
        DataSamples Data;
        public CreateRoomWindow(DataSamples data)
        {
            InitializeComponent();
            Data = data;
            DataContext = Data;
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            Data.AddElement(new Room {
                RoomName = NameTextBox.Text,
                RoomAdress = AdressTextBox.Text,
                Description = DescriptionTextBox.Text
            });
            Data.UpdateRooms();
            Close();
        }
    }
}
