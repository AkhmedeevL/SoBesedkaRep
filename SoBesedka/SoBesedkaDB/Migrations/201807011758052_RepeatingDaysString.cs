namespace SoBesedkaDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RepeatingDaysString : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Meetings", "RepeatingDays", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Meetings", "RepeatingDays");
        }
    }
}
