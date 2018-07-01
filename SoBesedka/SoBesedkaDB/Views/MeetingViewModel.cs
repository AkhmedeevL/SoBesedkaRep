using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoBesedkaDB.Views
{
    public class MeetingViewModel
    {
        public int Id { get; set; }
        
        public string MeetingName { get; set; }
        
        public string MeetingDescription { get; set; }
        
        public string MeetingTheme { get; set; }
        
        public int CreatorId { get; set; }
        
        public DateTime StartTime { get; set; }
        
        public DateTime EndTime { get; set; }
        
        public int RoomId { get; set; }

        public string RepeatingDays { get; set; }

        public override string ToString()
        {
            return $"{MeetingName}\n{StartTime.ToShortTimeString()}-{EndTime.ToShortTimeString()}";
        }
    }
}
