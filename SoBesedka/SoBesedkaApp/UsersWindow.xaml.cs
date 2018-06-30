using SoBesedkaDB;
using SoBesedkaDB.Implementations;
using SoBesedkaDB.Interfaces;
using SoBesedkaDB.Views;
using System.Windows;

namespace SoBesedkaApp
{
    /// <summary>
    /// Логика взаимодействия для UsersWindow.xaml
    /// </summary>
    public partial class UsersWindow : Window
    {
        DataSamples Data;
        public UsersWindow(DataSamples data)
        {
            Data = data;
            DataContext = Data;
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void AdminButton_Click(object sender, RoutedEventArgs e)
        {
            UserViewModel user = (UserViewModel)listBoxUsers.SelectedItem;
            user.isAdmin = true;
            Data.Uservice.UpdElement(Data.Uservice.ConvertViewToUser(user));
        }
    }
}
