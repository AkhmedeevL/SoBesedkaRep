using SoBesedkaDB.Views;
using SoBesedkaModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using SoBesedkaDB;
using SoBesedkaDB.Implementations;

namespace SoBesedkaApp
{
    public class DataSamples : INotifyPropertyChanged
    {
        public DateTime[] CurrentWeek { get; set; }
        public UserViewModel CurrentUser { get; set; }
        public RoomViewModel CurrentRoom { get; set; }
        public List<List<MeetingViewModel>> CurrentWeekMeetings { get; set; }
        public List<RoomViewModel> Rooms { get; set; }
        public List<UserViewModel> Users { get; set; }
        public List<MeetingViewModel> UserMeetings { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        // Для удобства обернем событие в метод с единственным параметром - имя изменяемого свойства
        public void RaisePropertyChanged(string propertyName)
        {
            // Если кто-то на него подписан, то вызывем его
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public bool SignIn(string login, string password)
        {
            try
            {
                var response = APIClient.GetRequest($"api/User/SignIn/?login={login}&password={password}");
                if (!response.Result.IsSuccessStatusCode) throw new Exception(APIClient.GetError(response));
                var user = APIClient.GetElement<UserViewModel>(response);
                if (user == null)
                    return false;
                CurrentUser = user;
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool AddElement(object element)
        {
            var controller = element.GetType().Name;
            try
            {
                var response = APIClient.PostRequest($"api/{controller}/AddElement", element);
                if (!response.Result.IsSuccessStatusCode) throw new Exception(APIClient.GetError(response));
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool DelElement(object element)
        {
            var controller = element.GetType().Name.Replace("ViewModel", "");
            try
            {
                var response = APIClient.PostRequest($"api/{controller}/DelElement", element);
                if (!response.Result.IsSuccessStatusCode) throw new Exception(APIClient.GetError(response));
                return true;
            }
            catch (Exception ex)
            {
               return false;
            }
        }

        //DEBUG
        //private MeetingService mservice;

        public DataSamples()
        {
            APIClient.Connect();

            //DEBUG
            //var context = new SoBesedkaDBContext();
            //mservice = new MeetingService(context);

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
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
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
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
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
                        //var list = mservice.GetListOfDay(CurrentRoom.Id, CurrentWeek[i].Date);
                        CurrentWeekMeetings.Add(list);
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
            RaisePropertyChanged("CurrentWeekMeetings");
        }

        public bool RestoringPassword(string email) {
            try
            {
                var response = APIClient.GetRequest($"api/User/RestoringPassword/?email={email}");
                if (!response.Result.IsSuccessStatusCode) throw new Exception(APIClient.GetError(response));
                var user = APIClient.GetElement<UserViewModel>(response);
                if (user == null)
                    return false;
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
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
