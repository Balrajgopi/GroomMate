namespace GroomMate.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialSetup : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Appointments",
                c => new
                    {
                        AppointmentID = c.Int(nullable: false, identity: true),
                        UserID = c.Int(nullable: false),
                        ServiceID = c.Int(nullable: false),
                        AppointmentDate = c.DateTime(nullable: false),
                        Status = c.String(),
                        StaffID = c.Int(),
                        Feedback_FeedbackID = c.Int(),
                    })
                .PrimaryKey(t => t.AppointmentID)
                .ForeignKey("dbo.Users", t => t.UserID, cascadeDelete: true)
                .ForeignKey("dbo.Feedbacks", t => t.Feedback_FeedbackID)
                .ForeignKey("dbo.Services", t => t.ServiceID, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.StaffID)
                .Index(t => t.UserID)
                .Index(t => t.ServiceID)
                .Index(t => t.StaffID)
                .Index(t => t.Feedback_FeedbackID);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        UserID = c.Int(nullable: false, identity: true),
                        Username = c.String(),
                        Password = c.String(),
                        RoleID = c.Int(nullable: false),
                        FullName = c.String(),
                        Email = c.String(),
                    })
                .PrimaryKey(t => t.UserID)
                .ForeignKey("dbo.Roles", t => t.RoleID, cascadeDelete: true)
                .Index(t => t.RoleID);
            
            CreateTable(
                "dbo.Roles",
                c => new
                    {
                        RoleID = c.Int(nullable: false, identity: true),
                        RoleName = c.String(),
                    })
                .PrimaryKey(t => t.RoleID);
            
            CreateTable(
                "dbo.Feedbacks",
                c => new
                    {
                        FeedbackID = c.Int(nullable: false, identity: true),
                        AppointmentID = c.Int(nullable: false),
                        Rating = c.Int(nullable: false),
                        Comments = c.String(),
                    })
                .PrimaryKey(t => t.FeedbackID);
            
            CreateTable(
                "dbo.Services",
                c => new
                    {
                        ServiceID = c.Int(nullable: false, identity: true),
                        ServiceName = c.String(),
                        Description = c.String(),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.ServiceID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Appointments", "StaffID", "dbo.Users");
            DropForeignKey("dbo.Appointments", "ServiceID", "dbo.Services");
            DropForeignKey("dbo.Appointments", "Feedback_FeedbackID", "dbo.Feedbacks");
            DropForeignKey("dbo.Appointments", "UserID", "dbo.Users");
            DropForeignKey("dbo.Users", "RoleID", "dbo.Roles");
            DropIndex("dbo.Users", new[] { "RoleID" });
            DropIndex("dbo.Appointments", new[] { "Feedback_FeedbackID" });
            DropIndex("dbo.Appointments", new[] { "StaffID" });
            DropIndex("dbo.Appointments", new[] { "ServiceID" });
            DropIndex("dbo.Appointments", new[] { "UserID" });
            DropTable("dbo.Services");
            DropTable("dbo.Feedbacks");
            DropTable("dbo.Roles");
            DropTable("dbo.Users");
            DropTable("dbo.Appointments");
        }
    }
}
