using SoBesedkaModels;
using System.Windows;

namespace SoBesedkaApp
{
    /// <summary>
    /// Логика взаимодействия для CreateRoomWindow.xaml
    /// </summary>
    public partial class CreateRoomWindow : Window
    {
        DataSource Data;
        public CreateRoomWindow(DataSource data)
        {
            InitializeComponent();
            Data = data;
            DataContext = Data;
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(AdressTextBox.Text) &&
                !string.IsNullOrEmpty(DescriptionTextBox.Text) &&
                !string.IsNullOrEmpty(NameTextBox.Text))
            {
                Data.AddElement(new Room
                {
                    RoomName = NameTextBox.Text,
                    RoomAdress = AdressTextBox.Text,
                    Description = DescriptionTextBox.Text
                });
                Data.UpdateRooms();
                Close();
            }
            else
            {
                MessageBox.Show("Заполните все поля", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
