using SoBesedkaDB;
using SoBesedkaDB.Implementations;
using SoBesedkaDB.Interfaces;
using SoBesedkaDB.Views;
using SoBesedkaModels;
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
    /// Логика взаимодействия для RoomsWindow.xaml
    /// </summary>
    public partial class RoomsWindow : Window
    {
        public IRoomService Rservice;
        DataSamples Data;
        public RoomsWindow()
        {
            Rservice = new RoomService(new SoBesedkaDBContext());
            InitializeComponent();
            Data = new DataSamples();
            DataContext = Data;
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
            var roomwndow = new CreateRoomWindow();
            roomwndow.Show();
            
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            RoomViewModel room = (RoomViewModel)RoomsListBox.SelectedItem;
            Rservice.DelElement(room.Id);
            var wnd = new RoomsWindow();
            wnd.Show();
            Close();

        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Data.RaisePropertyChanged("Rooms");
        }
    }
}
