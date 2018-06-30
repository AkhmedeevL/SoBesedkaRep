namespace SoBesedkaDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Meetings",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        MeetingName = c.String(nullable: false),
                        MeetingDescription = c.String(nullable: false),
                        MeetingTheme = c.String(nullable: false),
                        CreatorId = c.Int(nullable: false),
                        StartTime = c.String(nullable: false),
                        EndTime = c.String(nullable: false),
                        RoomId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Rooms", t => t.RoomId, cascadeDelete: true)
                .Index(t => t.RoomId);
            
            CreateTable(
                "dbo.UserMeetings",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        MeetingId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Meetings", t => t.MeetingId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.MeetingId);
            
            CreateTable(
                "dbo.Rooms",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        RoomName = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserFIO = c.String(nullable: false),
                        UserMail = c.String(nullable: false),
                        UserLogin = c.String(nullable: false),
                        UserPassword = c.String(nullable: false),
                        isAdmin = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserMeetings", "UserId", "dbo.Users");
            DropForeignKey("dbo.Meetings", "RoomId", "dbo.Rooms");
            DropForeignKey("dbo.UserMeetings", "MeetingId", "dbo.Meetings");
            DropIndex("dbo.UserMeetings", new[] { "MeetingId" });
            DropIndex("dbo.UserMeetings", new[] { "UserId" });
            DropIndex("dbo.Meetings", new[] { "RoomId" });
            DropTable("dbo.Users");
            DropTable("dbo.Rooms");
            DropTable("dbo.UserMeetings");
            DropTable("dbo.Meetings");
        }
    }
}
