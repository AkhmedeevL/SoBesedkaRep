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
        public IRoomService Rservice;
        DataSamples Data;
        public CreateRoomWindow()
        {
            Rservice = new RoomService(new SoBesedkaDBContext());
            InitializeComponent();
            Data = new DataSamples();
            DataContext = Data;
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            Rservice.AddElement(new Room {
                RoomName = NameTextBox.Text,
                RoomAdress = AdressTextBox.Text,
                Description = DescriptionTextBox.Text
            });
            var wnd = new RoomsWindow();
            wnd.Show();
            Close();
        }
    }
}
