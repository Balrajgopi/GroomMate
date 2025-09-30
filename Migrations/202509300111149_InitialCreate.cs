namespace GroomMate.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
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
                        StaffId = c.Int(),
                        AppointmentDate = c.DateTime(nullable: false),
                        Status = c.String(),
                    })
                .PrimaryKey(t => t.AppointmentID)
                .ForeignKey("dbo.Services", t => t.ServiceID, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.StaffId)
                .ForeignKey("dbo.Users", t => t.UserID, cascadeDelete: true)
                .Index(t => t.UserID)
                .Index(t => t.ServiceID)
                .Index(t => t.StaffId);
            
            CreateTable(
                "dbo.Feedbacks",
                c => new
                    {
                        AppointmentID = c.Int(nullable: false),
                        Rating = c.Int(nullable: false),
                        Comments = c.String(),
                    })
                .PrimaryKey(t => t.AppointmentID)
                .ForeignKey("dbo.Appointments", t => t.AppointmentID)
                .Index(t => t.AppointmentID);
            
            CreateTable(
                "dbo.Services",
                c => new
                    {
                        ServiceID = c.Int(nullable: false, identity: true),
                        ServiceName = c.String(),
                        Description = c.String(),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ServiceID);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        UserID = c.Int(nullable: false, identity: true),
                        Username = c.String(),
                        Password = c.String(),
                        FullName = c.String(),
                        Email = c.String(),
                        IsDeleted = c.Boolean(nullable: false),
                        RoleID = c.Int(nullable: false),
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
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Appointments", "UserID", "dbo.Users");
            DropForeignKey("dbo.Appointments", "StaffId", "dbo.Users");
            DropForeignKey("dbo.Users", "RoleID", "dbo.Roles");
            DropForeignKey("dbo.Appointments", "ServiceID", "dbo.Services");
            DropForeignKey("dbo.Feedbacks", "AppointmentID", "dbo.Appointments");
            DropIndex("dbo.Users", new[] { "RoleID" });
            DropIndex("dbo.Feedbacks", new[] { "AppointmentID" });
            DropIndex("dbo.Appointments", new[] { "StaffId" });
            DropIndex("dbo.Appointments", new[] { "ServiceID" });
            DropIndex("dbo.Appointments", new[] { "UserID" });
            DropTable("dbo.Roles");
            DropTable("dbo.Users");
            DropTable("dbo.Services");
            DropTable("dbo.Feedbacks");
            DropTable("dbo.Appointments");
        }
    }
}
