using SoBesedkaDB.Views;
using SoBesedkaModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;

namespace SoBesedkaApp
{
    public class DataSamples : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        // Для удобства обернем событие в метод с единственным параметром - имя изменяемого свойства
        public void RaisePropertyChanged(string propertyName)
        {
            // Если кто-то на него подписан, то вызывем его
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public DateTime[] CurrentWeek { get; set; }
        public UserViewModel CurrentUser { get; set; }
        public RoomViewModel CurrentRoom { get; set; }
        public List<List<MeetingViewModel>> CurrentWeekMeetings { get; set; }

        public bool SignIn(string login, string password)
        {
            try
            {
                var response = APIClient.GetRequest($"api/User/SignIn/?login={login}&password={password}");
                if (response.Result.IsSuccessStatusCode)
                {
                    var user = APIClient.GetElement<UserViewModel>(response);
                    if (user == null)
                        return false;
                    else
                        CurrentUser = user;
                    return true;
                }
                else
                {
                    throw new Exception(APIClient.GetError(response));
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public List<RoomViewModel> Rooms { get; set; }
        public List<UserViewModel> Users { get; set; }
        public List<MeetingViewModel> UserMeetings { get; set; }

        internal bool AddElement(User user)
        {
            var response = APIClient.PostRequest("api/User/AddElement", user);
            if (response.Result.IsSuccessStatusCode)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void AddElement(Room room)
        {

        }

        public bool AddElement(Meeting meeting)
        {
            try
            {
                var response = APIClient.PostRequest("api/Meeting/AddElement", meeting);
                if (response.Result.IsSuccessStatusCode)
                {
                    MessageBox.Show("Сохранение прошло успешно", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return true;
                }
                else
                {
                    throw new Exception(APIClient.GetError(response));
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public DataSamples()
        {
            APIClient.Connect();

            UpdateRooms();
            UpdateUsers();

            UserMeetings = new List<MeetingViewModel>();

            CurrentWeek = new DateTime[7];

            DateTime currentDay = DateTime.Now.Date;
            int daysToAdd = ((int)System.DayOfWeek.Monday - (int)currentDay.DayOfWeek - 7) % 7;
            CurrentWeek[0] = currentDay.AddDays(daysToAdd);

            for (int i = 1; i < 7; i++)
            {
                CurrentWeek[i] = CurrentWeek[i - 1] + TimeSpan.FromDays(1);
            }

        }

        public void UpdateUsers()
        {
            try
            {
                var userResponse = APIClient.GetRequest("api/User/GetList");

                if (userResponse.Result.IsSuccessStatusCode)
                {
                    var listUsers = APIClient.GetElement<List<UserViewModel>>(userResponse);
                    Users = listUsers;
                }
                else
                    throw new Exception(APIClient.GetError(userResponse));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            RaisePropertyChanged("Users");
        }

        public void UpdateRooms()
        {
            try
            {
                var roomResponse = APIClient.GetRequest("api/Room/GetList");

                if (roomResponse.Result.IsSuccessStatusCode)
                {
                    var listRooms = APIClient.GetElement<List<RoomViewModel>>(roomResponse);
                    Rooms = listRooms;
                }
                else
                    throw new Exception(APIClient.GetError(roomResponse));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            RaisePropertyChanged("Rooms");
        }

        public void UpdateMeetings()
        {
            CurrentWeekMeetings = new List<List<MeetingViewModel>>();
            for (int i = 0; i < 7; i++)
            {
                try
                {
                    var response = APIClient.GetRequest($"api/Meeting/GetListOfDay/?roomId={CurrentRoom.Id}&day={CurrentWeek[i].Date.ToString()}");
                    if (response.Result.IsSuccessStatusCode)
                    {
                        var list = APIClient.GetElement<List<MeetingViewModel>>(response);
                        CurrentWeekMeetings.Add(list);
                    }
                    else
                    {
                        throw new Exception(APIClient.GetError(response));
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            RaisePropertyChanged("CurrentWeekMeetings");
        }

        //public void SendEmail(string mailAddress, string subject, string text)
        //{
        //    MailMessage objMailMessage = new MailMessage();
        //    SmtpClient objSmtpClient = null;

        //    try
        //    {
        //        objMailMessage.From = new MailAddress("sobesedkaapp@yandex.ru");
        //        objMailMessage.To.Add(new MailAddress(mailAddress));
        //        objMailMessage.Subject = subject;
        //        objMailMessage.Body = text;
        //        objMailMessage.SubjectEncoding = System.Text.Encoding.UTF8;
        //        objMailMessage.BodyEncoding = System.Text.Encoding.UTF8;

        //        objSmtpClient = new SmtpClient("smtp.yandex.ru", 587);
        //        objSmtpClient.UseDefaultCredentials = false;
        //        objSmtpClient.EnableSsl = true;
        //        objSmtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
        //        objSmtpClient.Credentials = new NetworkCredential("sobesedkaapp@yandex.ru", "teamb123");

        //        objSmtpClient.Send(objMailMessage);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    finally
        //    {
        //        objMailMessage = null;
        //        objSmtpClient = null;
        //    }
        //}
    }
}
