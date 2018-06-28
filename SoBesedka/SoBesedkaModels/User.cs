using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SoBesedkaModels
{
    public class User
    {
        
        public int Id { get; set; }
        
        [Required]
        public string UserFIO { get; set; }

        [Required]
        public string UserMail { get; set; }

        [Required]
        public string UserLogin { get; set; }

        [Required]
        public string UserPassword { get; set; }

        [Required]
        public bool isAdmin { get; set; }

        [ForeignKey("UserId")]
        public virtual List<UserMeeting> UserMeetings { get; set; }
    }
}
