using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoBesedkaApp
{
    public class DataSamples
    {
        
        public List<Room> rooms { get; set; }
        public DataSamples()
        {

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
            
            var event1 = new Event("Мероприятие", "Тема мероприятия", "Описание описание описание описание описание описание описание описание", DateTime.Now, DateTime.MaxValue);
            event1.AddMember(members[0]);
            event1.AddMember(members[1]);
            var event2 = new Event("Собрание", "Тема собрания", "Описание описание описание описание описание описание описание описание", DateTime.Now, DateTime.MaxValue);
            event2.AddMember(members[0]);
            event2.AddMember(members[1]);
            event2.AddMember(members[2]);
            var event3 = new Event("Митинг", "Тема митинга", "Описание описание описание описание описание описание описание описание", DateTime.Now, DateTime.MaxValue);
            event3.AddMember(members[3]);
            event3.AddMember(members[2]);
            event3.AddMember(members[0]);
            rooms[0].Events.Add(event1);
            rooms[0].Events.Add(new Event("", "", "", DateTime.Now, DateTime.Now));
            rooms[0].Events.Add(event2);
            rooms[0].Events.Add(new Event("", "", "", DateTime.Now, DateTime.Now));
            rooms[1].Events.Add(event3);

        }
    }

    public class Room
    {
        public List<Event> Events { get; set; }
        public string Name { get; private set; }
        public Room(string name)
        {
            Name = name;
            Events = new List<Event>();
        }
        public override string ToString()
        {
            return Name;
        }
    }

    public class Event
    {
        private string _title;
        private string _topic;
        private string _desc;
        private DateTime _timeStart;
        private DateTime _timeEnd;
        public List<Member> Members { get; }

        public Event(string title, string topic, string desc, DateTime timeStart, DateTime timeEnd)
        {
            _title = title;
            _topic = topic;
            _desc = desc;
            _timeStart = timeStart;
            _timeEnd = timeEnd;
            Members = new List<Member>();
        }

        public string Desc { get => _desc; set => _desc = value; }
        public string Topic { get => _topic; set => _topic = value; }
        public string Title { get => _title; set => _title = value; }
        public DateTime TimeStart { get => _timeStart; set => _timeStart = value; }
        public DateTime TimeEnd { get => _timeEnd; set => _timeEnd = value; }

        public void AddMember(Member member)
        {
            Members.Add(member);
        }

        public override string ToString()
        {
            return $"{Title}      {TimeStart.ToShortTimeString()}-{TimeEnd.ToShortTimeString()}";
        }
    }

    public class Member
    {
        public Member(string name)
        {
            Name = name;
        }

        public string Name { get; private set;}

        public override string ToString()
        {
            return Name;
        }
    }
}
