using System;
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
        public int CreatorId { get; set; }

        [Required]
        public DateTime StartTime { get; set; }

        [Required]
        public DateTime EndTime { get; set; }

        [Required]
        public int RoomId { get; set; }

        [ForeignKey("MeetingId")]
        public virtual List<UserMeeting> UserMeetings { get; set; }

        public List<DayOfWeek> RepeatingDays { get; set; }
    }
}
