using SoBesedkaDB;
using SoBesedkaDB.Implementations;
using SoBesedkaDB.Interfaces;
using SoBesedkaDB.Views;
using System;
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
            if (user == null)
                return;
            user.isAdmin = true;

            try
            {
                var response = APIClient.PostRequest("api/User/UpdElement", user);
                if (response.Result.IsSuccessStatusCode)
                {
                    MessageBox.Show($"Пользователь {user.UserFIO} - администратор", "Сообщение", MessageBoxButton.OK, MessageBoxImage.Information);
                    Data.UpdateUsers();
                }
                else
                {
                    throw new Exception(APIClient.GetError(response));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
