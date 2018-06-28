using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoBesedkaModels
{
    public class Room
    {
        public int Id { get; set; }

        [Required]
        public string RoomName { get; set; }

        [ForeignKey("RoomId")]
        public virtual List<Meeting> Meetings { get; set; }
    }
}
