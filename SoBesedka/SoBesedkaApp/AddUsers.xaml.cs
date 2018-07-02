using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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
using SoBesedkaApp.Annotations;
using SoBesedkaDB.Views;

namespace SoBesedkaApp
{
    /// <summary>
    /// Логика взаимодействия для AddUsers.xaml
    /// </summary>
    public partial class AddUsers : Window, INotifyPropertyChanged
    {
        private DataSamples Data;
        public List<UserViewModel> SelectedUsers { get; set; }

        public AddUsers(DataSamples data)
        {
            Data = data;
            InitializeComponent();

            SelectedUsers = new List<UserViewModel>();
            AllUsersListBox.DataContext = Data;
            SelectedUsersListBox.DataContext = this;
            SelectedUsersListBox.Items.Refresh();
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            UserViewModel user = (UserViewModel) AllUsersListBox.SelectedItem;
            if (user == null)
                return;
            if (SelectedUsers.Any(u => u.Id == user.Id))
                return;
            SelectedUsers.Add(user);
            OnPropertyChanged("SelectedUsers");
            SelectedUsersListBox.Items.Refresh();
        }

        private void Remove_Click(object sender, RoutedEventArgs e)
        {
            UserViewModel user = (UserViewModel) SelectedUsersListBox.SelectedItem;
            if (user == null)
                return;
            SelectedUsers.Remove(user);
            OnPropertyChanged("SelectedUsers");
            SelectedUsersListBox.Items.Refresh();
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        public void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
