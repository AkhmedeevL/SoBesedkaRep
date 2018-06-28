using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoBesedkaDB.Views
{
   public class UserViewModel
    {
        public int Id { get; set; }

        public string UserFIO { get; set; }


        public string UserMail { get; set; }

        public string UserLogin { get; set; }

        public string UserPassword { get; set; }

        public bool isAdmin { get; set; }

        public override string ToString()
        {
            return UserFIO;
        }
    }
}
