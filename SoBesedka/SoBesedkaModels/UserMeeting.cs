using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoBesedkaModels
{
    public class UserMeeting
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public int MeetingId { get; set; }

        public virtual User User { get; set; }
        
        public virtual Meeting Meeting { get; set; }
    }
}