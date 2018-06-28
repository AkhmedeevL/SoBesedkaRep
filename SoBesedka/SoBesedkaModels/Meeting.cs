using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SoBesedkaModels
{
   public class Meeting
    {
        public int Id { get; set; }

        [Required]
        public string MeetingName { get; set; }

        [Required]
        public string MeetingDescription { get; set; }

        [Required]
        public string MeetingTheme { get; set; }

        [Required]
        public string CreatorId { get; set; }

        [Required]
        public string StartTime { get; set; }

        [Required]
        public string EndTime { get; set; }

        [Required]
        public string RoomId { get; set; }

        public virtual Room Room { get; set; }
        [ForeignKey("MeetingId")]
        public virtual List<UserMeeting> UserMeetings { get; set; }
    }
}
