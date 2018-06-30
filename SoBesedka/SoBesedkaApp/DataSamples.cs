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
        public DataSamples()
        {
            context = new SoBesedkaDBContext();
            
            Uservice = new UserService(context);
            Rservice = new RoomService(context);
            Mservice = new MeetingService(context);

            Users = new List<UserViewModel>(Uservice.GetList());

            Rooms = new List<RoomViewModel>(Rservice.GetList());

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

    public class Event
    {
        public List<Member> Members { get; }

        public Event(string title, string topic, string desc, DateTime timeStart, DateTime timeEnd)
        {
            Title = title;
            Topic = topic;
            Desc = desc;
            TimeStart = timeStart;
            TimeEnd = timeEnd;
            Height = (int)Math.Sqrt((timeEnd - timeStart).TotalMinutes * 80);
            Members = new List<Member>();
        }

        public string Desc { get; set; }
        public string Topic { get; set; }
        public string Title { get; set; }
        public DateTime TimeStart { get; set; }
        public DateTime TimeEnd { get; set; }
        public int Height { get; set; }

        public void AddMember(Member member)
        {
            Members.Add(member);
        }

        public override string ToString()
        {
            return $"{Title}\n{TimeStart.ToShortTimeString()}-{TimeEnd.ToShortTimeString()}";
        }
    }

    public class Member
    {
        public Member(string name)
        {
            Name = name;
        }

        public string Name { get; private set; }

        public override string ToString()
        {
            return Name;
        }
    }
}
