﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoBesedkaDB.Views
{
    public class RoomViewModel
    {
        public int Id { get; set; }

        public string RoomName { get; set; }

        public string RoomAdress { get; set; }

        public string Description { get; set; }

        public List<MeetingViewModel> Meetings { get; set; }

        public override string ToString()
        {
            return RoomName;
        }

    }
}
