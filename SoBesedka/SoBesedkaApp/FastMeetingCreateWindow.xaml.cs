using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SoBesedkaDB.Views;
using SoBesedkaModels;
using System;
using System.Collections.Generic;
using System.Windows;
using MaskedTextBox = System.Windows.Forms.MaskedTextBox;

namespace SoBesedkaApp
{
    /// <summary>
    /// Логика взаимодействия для FastMeetingCreateWindow.xaml
    /// </summary>
    public partial class FastMeetingCreateWindow : Window
    {
        DataSource Data;
        List<UserViewModel> InvitedUsers;

        private MaskedTextBox startTimeMaskedTextBox;
        private MaskedTextBox durationMaskedTextBox;
        public FastMeetingCreateWindow(DataSource data)
        {

            InitializeComponent();
            Data = data;
            DataContext = Data;
            startTimeMaskedTextBox = new MaskedTextBox("00:00")
            {
                Dock = System.Windows.Forms.DockStyle.Fill,
                Width = (int)host1.Width,
                Height = (int)host1.Height,
                Font = new System.Drawing.Font("Segoe UI", 12),
                BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
            };
            host1.Child = startTimeMaskedTextBox;
            startTimeMaskedTextBox.TextChanged += Time_Changed;

            durationMaskedTextBox = new MaskedTextBox("00:00")
            {
                Dock = System.Windows.Forms.DockStyle.Fill,
                Width = (int)host2.Width,
                Height = (int)host2.Height,
                Font = new System.Drawing.Font("Segoe UI", 12),
                BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
            };
            host2.Child = durationMaskedTextBox;
            durationMaskedTextBox.TextChanged += Time_Changed;
        }

        private void Time_Changed(object sender, EventArgs e)
        {
            if (DatePicker.SelectedDate == null)
                return;
            DateTime currentDay = DatePicker.SelectedDate.Value;
            DateTime start, end;
            List<RoomViewModel> rooms;
            try
            {
                start = currentDay + DateTime.Parse(startTimeMaskedTextBox.Text).TimeOfDay;
                end = currentDay + DateTime.Parse(startTimeMaskedTextBox.Text).TimeOfDay + DateTime.Parse(durationMaskedTextBox.Text).TimeOfDay;
            }
            catch (Exception)
            {
                return;
            }
            try
            {
                var response = APIClient.PostRequest("api/Room/GetAvailableRooms", new Meeting { StartTime = start, EndTime = end });
                if (response.Result.IsSuccessStatusCode)
                {
                    rooms = APIClient.GetElement<List<RoomViewModel>>(response);
                }
                else
                {
                    throw new Exception(APIClient.GetError(response));
                }
            }
            catch (Exception ex)
            {
                JObject message = (JObject)JsonConvert.DeserializeObject(ex.Message);
                RoomsFindTextBox.Text = message["ExceptionMessage"].Value<string>();
                return;
            }
            RoomsFindTextBox.Text = "";
            RoomsComboBox.ItemsSource = rooms;
            RoomsComboBox.Items.Refresh();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (TimeSpan.Parse(startTimeMaskedTextBox.Text) > TimeSpan.FromHours(17) ||
                    TimeSpan.Parse(startTimeMaskedTextBox.Text) + TimeSpan.Parse(durationMaskedTextBox.Text) >
                    TimeSpan.FromHours(17) ||
                    TimeSpan.Parse(startTimeMaskedTextBox.Text) < TimeSpan.FromHours(8))
                {
                    MessageBox.Show("Мероприятия проводятся с 8:00 до 17:00", "Внимание", MessageBoxButton.OK,
                        MessageBoxImage.Warning);
                    return;
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show("Неверный формат времени", "Внимание", MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                return;
            }
            try
            {
                if (DatePicker.SelectedDate.Value + DateTime.Parse(startTimeMaskedTextBox.Text).TimeOfDay <= DateTime.Now)
                {
                    MessageBox.Show("Время, на которое Вы хотите назвачить мероприятие, уже прошло", "Ошибка", MessageBoxButton.OK);
                    return;
                }
                var repDays = "0000000";
                var userMeetings = new List<UserMeeting>();
                foreach (UserViewModel user in InvitedUsersListBox.Items)
                {
                    userMeetings.Add(new UserMeeting
                    {
                        UserId = user.Id
                    });
                }
                if (DatePicker.SelectedDate != null &&
                    !string.IsNullOrEmpty(startTimeMaskedTextBox.Text) &&
                    !string.IsNullOrEmpty(durationMaskedTextBox.Text) &&
                    !string.IsNullOrEmpty(TitleTextBox.Text) &&
                    !string.IsNullOrEmpty(SubjTextBox.Text) &&
                    !string.IsNullOrEmpty(DescriptionTextBox.Text))
                {
                    Data.AddElement(new Meeting
                    {
                        MeetingName = TitleTextBox.Text,
                        MeetingTheme = SubjTextBox.Text,
                        MeetingDescription = DescriptionTextBox.Text,
                        StartTime = DatePicker.SelectedDate.Value +
                                    DateTime.Parse(startTimeMaskedTextBox.Text).TimeOfDay,
                        EndTime = DatePicker.SelectedDate.Value +
                                  DateTime.Parse(startTimeMaskedTextBox.Text).TimeOfDay +
                                  DateTime.Parse(durationMaskedTextBox.Text).TimeOfDay,
                        UserMeetings = userMeetings,
                        RoomId = Data.CurrentRoom.Id,
                        CreatorId = Data.CurrentUser.Id,
                        RepeatingDays = repDays
                    });
                    MessageBox.Show("Добавлено", "Успех", MessageBoxButton.OK);
                }
                else
                {
                    MessageBox.Show("Заполните все поля", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OKCancel);
                return;
            }
            DialogResult = true;
            Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            var users = new AddUsers(Data);
            foreach (var user in InvitedUsersListBox.Items)
            {
                users.SelectedUsers.Add((UserViewModel)user);
            }
            users.OnPropertyChanged("SelectedUsers");
            users.SelectedUsersListBox.Items.Refresh();
            if (users.ShowDialog() == true)
            {
                InvitedUsersListBox.Items.Clear();
                foreach (var user in users.SelectedUsers)
                {
                    InvitedUsersListBox.Items.Add(user);
                }
            }
        }
    }
}
