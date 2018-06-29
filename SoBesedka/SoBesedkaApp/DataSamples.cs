
using SoBesedkaDB;
using SoBesedkaDB.Implementations;
using SoBesedkaDB.Interfaces;
using SoBesedkaDB.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        private DateTime[] currentWeek;
        public DateTime[] CurrentWeek
        {
            get { return currentWeek; }
            set
            {
                currentWeek = value;
                RaisePropertyChanged("CurrentWeek");
            }
        }

        public UserViewModel CurrentUser { get; set; }
        public RoomViewModel CurrentRoom { get; set; }
        public List<List<MeetingViewModel>> CurrentWeekMeetings { get; set; }
        public List<List<int>> PanelElementHeight { get; set; }

        public IUserService Uservice;
        public IRoomService Rservice;
        public MeetingService Mservice;

        SoBesedkaDBContext context;

        public List<RoomViewModel> Rooms { get; set; }
        public List<UserViewModel> Users { get; set; }
        public List<MeetingViewModel> UserMeetings { get; set; }

        public DataSamples()
        {
            context = new SoBesedkaDBContext();
            
            Uservice = new UserService(context);
            Rservice = new RoomService(context);
            Mservice = new MeetingService(context);

            Users = new List<UserViewModel>(Uservice.GetList());

            Rooms = new List<RoomViewModel>(Rservice.GetList());

            UserMeetings = new List<MeetingViewModel>();

            CurrentWeek = new DateTime[7];
            for (int i = 0; i < 7; i++)
            {
                CurrentWeek[i] = DateTime.Now + TimeSpan.FromDays(i);
                //PanelElementHeight.Add(new List<int>());
            }

        }

        public void UpdateMeetings()
        {
            CurrentWeekMeetings = new List<List<MeetingViewModel>>();
            for (int i = 0; i < 7; i++)
            {
                CurrentWeekMeetings.Add(Mservice.GetListOfDay(CurrentRoom.Id, CurrentWeek[i]));
                
            }
            RaisePropertyChanged("CurrentWeekMeetings");
            RaisePropertyChanged("PanelElementHeight");
        }
    }

    

    
}
