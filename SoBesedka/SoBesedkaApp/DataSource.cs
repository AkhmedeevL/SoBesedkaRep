using SoBesedkaDB.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace SoBesedkaApp
{
    public class DataSource : INotifyPropertyChanged
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
                //JObject message = (JObject) JsonConvert.DeserializeObject(ex.Message);
                //throw new Exception(message["ExceptionMessage"].Value<string>());
                return false;
            }
        }

        public bool UpdElement(object element)
        {
            var controller = element.GetType().Name;
            try
            {
                var response = APIClient.PostRequest($"api/{controller}/UpdElement", element);
                if (!response.Result.IsSuccessStatusCode)
                {
                    throw new Exception(APIClient.GetError(response));
                }
                return true;
            }
            catch (Exception ex)
            {
                //JObject message = (JObject) JsonConvert.DeserializeObject(ex.Message);
                //throw new Exception(message["ExceptionMessage"].Value<string>());
                return false;
            }
        }

        public bool AddElement(object element)
        {
            var controller = element.GetType().Name;
            try
            {
                var response = APIClient.PostRequest($"api/{controller}/AddElement", element);
                if (!response.Result.IsSuccessStatusCode)
                {
                    throw new Exception(APIClient.GetError(response));
                }
                return true;
            }
            catch (Exception ex)
            {
                //JObject message = (JObject) JsonConvert.DeserializeObject(ex.Message);
                //throw new Exception(message["ExceptionMessage"].Value<string>());
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
                //JObject message = (JObject) JsonConvert.DeserializeObject(ex.Message);
                //throw new Exception(message["ExceptionMessage"].Value<string>());
                return false;
            }
        }

        public DataSource()
        {
            APIClient.Connect();
            try
            {
                UpdateRooms();
                UpdateUsers();
                UserMeetings = new List<MeetingViewModel>();
                CurrentWeek = new DateTime[7];
                DateTime currentDay = DateTime.Now.Date;
                int daysToAdd = ((int) System.DayOfWeek.Monday - (int) currentDay.DayOfWeek - 7) % 7;
                CurrentWeek[0] = currentDay.AddDays(daysToAdd);

                for (int i = 1; i < 7; i++)
                {
                    CurrentWeek[i] = CurrentWeek[i - 1] + TimeSpan.FromDays(1);
                }
            }
            catch (Exception e)
            {
                throw new Exception("");
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
                JObject message = (JObject) JsonConvert.DeserializeObject(ex.Message);
                throw new Exception(message["ExceptionMessage"].Value<string>());
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
                JObject message = (JObject) JsonConvert.DeserializeObject(ex.Message);
                throw new Exception(message["ExceptionMessage"].Value<string>());
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
                    var date = CurrentWeek[i].Date.ToString(CultureInfo.InvariantCulture);
                    var response = APIClient.GetRequest($"api/Meeting/GetListOfDay/?roomId={CurrentRoom.Id}&day={date}");
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
                    JObject message = (JObject) JsonConvert.DeserializeObject(ex.Message);
                    throw new Exception(message["ExceptionMessage"].Value<string>());
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

        public string GetHashString(string s)
        {
            //переводим строку в байт-массим  
            byte[] bytes = Encoding.UTF8.GetBytes(s);

            //создаем объект для получения средст шифрования  
            MD5CryptoServiceProvider CSP =
                new MD5CryptoServiceProvider();

            //вычисляем хеш-представление в байтах  
            byte[] byteHash = CSP.ComputeHash(bytes);

            string hash = string.Empty;

            //формируем одну цельную строку из массива  
            foreach (byte b in byteHash)
                hash += string.Format("{0:x2}", b);

            return hash;
        }
    }
}
