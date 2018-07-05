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
        DataSource Data;
        public UsersWindow(DataSource data)
        {
            Data = data;
            DataContext = Data;
            InitializeComponent();
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

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                UserViewModel user = (UserViewModel)listBoxUsers.SelectedItem;
                if (user == null)
                    return;
                if (user.Id == Data.CurrentUser.Id)
                {
                    MessageBox.Show("Нельзя удалить текущего пользователя", "Внимание", MessageBoxButton.OK,
                        MessageBoxImage.Warning);
                    return;
                }
                Data.DelElement(user);
                MessageBox.Show($"Пользователь {user.UserFIO} удален", "Сообщение", MessageBoxButton.OK, MessageBoxImage.Information);
                Data.UpdateUsers();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void RemoveAdminButton_Click(object sender, RoutedEventArgs e)
        {
            UserViewModel user = (UserViewModel)listBoxUsers.SelectedItem;
            if (user == null)
                return;
            user.isAdmin = false;

            try
            {
                var response = APIClient.PostRequest("api/User/UpdElement", user);
                if (response.Result.IsSuccessStatusCode)
                {
                    MessageBox.Show($"Пользователь {user.UserFIO} - удалён из списка администраторов", "Сообщение", MessageBoxButton.OK, MessageBoxImage.Information);
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

        private void listBoxUsers_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            UserViewModel user = (UserViewModel)listBoxUsers.SelectedItem;
            if (user == null)
                return;

            if (user.Id == Data.CurrentUser.Id)
            {
                DeleteButton.IsEnabled = false;
                AdminButton.IsEnabled = false;
                RemoveAdminButton.IsEnabled = false;
            }

            if (user.Id != Data.CurrentUser.Id && user.isAdmin)
            {
                DeleteButton.IsEnabled = true;
                RemoveAdminButton.IsEnabled = true;
                AdminButton.IsEnabled = false;
            }

            if (user.Id != Data.CurrentUser.Id && !user.isAdmin)
            {
                AdminButton.IsEnabled = true;
                DeleteButton.IsEnabled = true;
                RemoveAdminButton.IsEnabled = false;
            }

        }
    }
}
