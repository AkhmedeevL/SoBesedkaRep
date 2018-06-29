using SoBesedkaDB;
using SoBesedkaModels;
using SoBesedkaDB.Implementations;
using SoBesedkaDB.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

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
