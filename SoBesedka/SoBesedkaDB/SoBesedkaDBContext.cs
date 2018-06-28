
using SoBesedkaModels;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;



namespace SoBesedkaDB
{
    public class SoBesedkaDBContext : DbContext
    {
        public SoBesedkaDBContext() : base("name=SoBesedkaDBContext")
        {
            Configuration.ProxyCreationEnabled = false;
            Configuration.LazyLoadingEnabled = false;
            var ensureDLLIsCopied = System.Data.Entity.SqlServer.SqlProviderServices.Instance;
        }

        public virtual DbSet<User> Users { get; set; }

        public virtual DbSet<Meeting> Meetings { get; set; }

        public virtual DbSet<Room> Rooms { get; set; }


        public virtual DbSet<UserMeeting> UserMeetings { get; set; }

    }
}
