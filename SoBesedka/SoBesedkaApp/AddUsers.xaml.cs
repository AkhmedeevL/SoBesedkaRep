﻿using SoBesedkaApp.Annotations;
using SoBesedkaDB.Views;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace SoBesedkaApp
{
    /// <summary>
    /// Логика взаимодействия для AddUsers.xaml
    /// </summary>
    public partial class AddUsers : Window, INotifyPropertyChanged
    {
        private DataSource Data;
        public List<UserViewModel> SelectedUsers { get; set; }


        public AddUsers(DataSource data)
        {
            Data = data;
            InitializeComponent();

            SelectedUsers = new List<UserViewModel>();
            AllUsersListBox.DataContext = Data;
            SelectedUsersListBox.DataContext = this;
            SelectedUsersListBox.Items.Refresh();
            AllUsersListBox.SelectionMode = SelectionMode.Multiple;
            SelectedUsersListBox.SelectionMode = SelectionMode.Multiple;
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            foreach (UserViewModel SelectedUserToAdd in AllUsersListBox.SelectedItems)
            {
                UserViewModel user = (UserViewModel)SelectedUserToAdd;
                if (user == null)
                {
                    AllUsersListBox.SelectedItems.Clear();
                    return;
                }
                if (SelectedUsers.Any(u => u.Id == user.Id))
                {
                    AllUsersListBox.SelectedItems.Clear();
                    return;
                }
                SelectedUsers.Add(user);
                OnPropertyChanged("SelectedUsers");
                SelectedUsersListBox.Items.Refresh();
            }
            AllUsersListBox.SelectedItems.Clear();

        }

        private void Remove_Click(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < SelectedUsersListBox.SelectedItems.Count;)
            {
                UserViewModel user = (UserViewModel)SelectedUsersListBox.SelectedItems[0];
                if (user == null)
                    return;
                SelectedUsers.Remove(user);
                OnPropertyChanged("SelectedUsers");
                SelectedUsersListBox.Items.Refresh();
            }
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

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var filteredUsers = Data.Users
                .Where(user => user.UserFIO.ToLower().Contains(FilterTextBox.Text.ToLower()))
                .ToList();
            AllUsersListBox.ItemsSource = filteredUsers;
            AllUsersListBox.Items.Refresh();
        }

        private void AllUsersListBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            UserViewModel user = (UserViewModel)AllUsersListBox.SelectedItem;
            if (user == null)
            {
                AllUsersListBox.SelectedItems.Clear();
                return;
            }
            if (SelectedUsers.Any(u => u.Id == user.Id))
            {
                AllUsersListBox.SelectedItems.Clear();
                return;
            }
            SelectedUsers.Add(user);
            OnPropertyChanged("SelectedUsers");
            SelectedUsersListBox.Items.Refresh();
        }

        private void SelectedUsersListBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            UserViewModel user = (UserViewModel)SelectedUsersListBox.SelectedItem;
            if (user == null)
                return;
            SelectedUsers.Remove(user);
            OnPropertyChanged("SelectedUsers");
            SelectedUsersListBox.Items.Refresh();
        }
    }
}
