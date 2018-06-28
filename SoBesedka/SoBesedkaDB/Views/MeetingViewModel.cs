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

        
        public string CreatorId { get; set; }

        
        public string StartTime { get; set; }

        
        public string EndTime { get; set; }

        
        public string RoomId { get; set; }
    }
}
