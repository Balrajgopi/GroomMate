namespace GroomMate.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddStaffIdToAppointment : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Appointments", "Feedback_FeedbackID", "dbo.Feedbacks");
            DropForeignKey("dbo.Appointments", "UserID", "dbo.Users");
            DropIndex("dbo.Appointments", new[] { "UserID" });
            DropIndex("dbo.Appointments", new[] { "StaffID" });
            DropIndex("dbo.Appointments", new[] { "Feedback_FeedbackID" });
            AddColumn("dbo.Appointments", "User_UserID", c => c.Int());
            CreateIndex("dbo.Appointments", "StaffId");
            CreateIndex("dbo.Appointments", "User_UserID");
            AddForeignKey("dbo.Appointments", "User_UserID", "dbo.Users", "UserID");
            DropColumn("dbo.Appointments", "Feedback_FeedbackID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Appointments", "Feedback_FeedbackID", c => c.Int());
            DropForeignKey("dbo.Appointments", "User_UserID", "dbo.Users");
            DropIndex("dbo.Appointments", new[] { "User_UserID" });
            DropIndex("dbo.Appointments", new[] { "StaffId" });
            DropColumn("dbo.Appointments", "User_UserID");
            CreateIndex("dbo.Appointments", "Feedback_FeedbackID");
            CreateIndex("dbo.Appointments", "StaffID");
            CreateIndex("dbo.Appointments", "UserID");
            AddForeignKey("dbo.Appointments", "UserID", "dbo.Users", "UserID", cascadeDelete: true);
            AddForeignKey("dbo.Appointments", "Feedback_FeedbackID", "dbo.Feedbacks", "FeedbackID");
        }
    }
}
