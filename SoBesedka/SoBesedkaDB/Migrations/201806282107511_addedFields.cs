namespace SoBesedkaDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addedFields : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Rooms", "RoomAdress", c => c.String(nullable: false));
            AddColumn("dbo.Rooms", "Description", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Rooms", "Description");
            DropColumn("dbo.Rooms", "RoomAdress");
        }
    }
}
