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
        private DateTime currentWeek;
        public DateTime CurrentWeek
        {
            get { return currentWeek; }
            set
            {
                currentWeek = value;
                RaisePropertyChanged("CurrentWeek");
            }
        }
        public List<Room> rooms;
        public List<Room> Rooms { get => rooms; set
            {
                rooms = value;
                RaisePropertyChanged("Rooms");
            }
        }
        public DataSamples()
        {
            CurrentWeek = DateTime.Now;

            var members = new List<Member>()
            {
                new Member("Пётр Петров"),
                new Member("Иван Иванов"),
                new Member("Сидр Сидоров"),
                new Member("Вася Васильев")
            };
            rooms = new List<Room>
            {
                new Room("Переговорка 1"),
                new Room("Переговорка 2"),
            };

            var event1 = new Event("Мероприятие", "Тема мероприятия", "Описание описание описание описание описание описание описание описание", DateTime.Now.Date + TimeSpan.FromHours(8), DateTime.Now.Date + TimeSpan.FromHours(9));
            event1.AddMember(members[0]);
            event1.AddMember(members[1]);
            var event2 = new Event("Собрание", "Тема собрания", "Описание описание описание описание описание описание описание описание", DateTime.Now.Date + TimeSpan.FromHours(10), DateTime.Now.Date + TimeSpan.FromHours(12));
            event2.AddMember(members[0]);
            event2.AddMember(members[1]);
            event2.AddMember(members[2]);
            var event3 = new Event("Митинг", "Тема митинга", "Описание описание описание описание описание описание описание описание", DateTime.Now.Date + TimeSpan.FromHours(11), DateTime.Now.Date + TimeSpan.FromHours(14));
            event3.AddMember(members[3]);
            event3.AddMember(members[2]);
            event3.AddMember(members[0]);
            rooms[0].Week[0].Add(event1);
            rooms[0].Week[0].Add(new Event("", "", "", DateTime.Now.Date + TimeSpan.FromHours(9), DateTime.Now.Date + TimeSpan.FromHours(10)));
            rooms[0].Week[0].Add(event2);
            rooms[0].Week[1].Add(new Event("", "", "", DateTime.Now.Date + TimeSpan.FromHours(8), DateTime.Now.Date + TimeSpan.FromHours(11)));
            rooms[0].Week[1].Add(event3);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        // Для удобства обернем событие в метод с единственным параметром - имя изменяемого свойства
        public void RaisePropertyChanged(string propertyName)
        {
            // Если кто-то на него подписан, то вызывем его
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class Room
    {
        public List<List<Event>> Week { get; set; }
        public string Name { get; private set; }
        public Room(string name)
        {
            Name = name;
            Week = new List<List<Event>>();
            for (int i = 0; i < 7; i++)
                Week.Add(new List<Event>());
        }
        public override string ToString()
        {
            return Name;
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
            Height = (int)(timeEnd - timeStart).TotalMinutes;
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
