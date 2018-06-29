namespace SoBesedkaDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class dateTime : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Meetings", "StartTime", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Meetings", "EndTime", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Meetings", "EndTime", c => c.String(nullable: false));
            AlterColumn("dbo.Meetings", "StartTime", c => c.String(nullable: false));
        }
    }
}
