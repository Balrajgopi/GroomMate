namespace GroomMate.Migrations
{
    using GroomMate.Models;
    using System;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<GroomMate.Models.GroomMateContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(GroomMate.Models.GroomMateContext context)
        {
            // --- STEP 1: CLEAN UP EXISTING DATA TO PREVENT ERRORS ---
            // This is a common strategy for development seeds to ensure a clean slate.
            // It disables all constraints, truncates tables, and then re-enables constraints.
            // Note: Add a check for the Feedbacks table to be truncated.
            context.Database.ExecuteSqlCommand("EXEC sp_MSforeachtable @command1='ALTER TABLE ? NOCHECK CONSTRAINT ALL'");
            context.Feedbacks.RemoveRange(context.Feedbacks);
            context.Appointments.RemoveRange(context.Appointments);
            context.Users.RemoveRange(context.Users);
            context.Roles.RemoveRange(context.Roles);
            context.Services.RemoveRange(context.Services);
            context.Database.ExecuteSqlCommand("EXEC sp_MSforeachtable @command1='ALTER TABLE ? WITH CHECK CHECK CONSTRAINT ALL'");
            context.SaveChanges();

            // --- STEP 2: SEED ROLES ---
            var adminRole = new Role { RoleName = "Admin" };
            var staffRole = new Role { RoleName = "Staff" };
            var customerRole = new Role { RoleName = "Customer" };

            context.Roles.AddOrUpdate(r => r.RoleName, adminRole, staffRole, customerRole);
            context.SaveChanges();

            // --- STEP 3: SEED USERS ---
            context.Users.AddOrUpdate(u => u.Username,
                new User { FullName = "Admin User", Username = "admin", Password = "password", Email = "admin@groommate.com", RoleID = adminRole.RoleID },
                new User { FullName = "Staff Member", Username = "staff", Password = "password", Email = "staff@groommate.com", RoleID = staffRole.RoleID },
                new User { FullName = "Balraj Gopi", Username = "balraj", Password = "password", Email = "balraj@example.com", RoleID = customerRole.RoleID }
            );
            context.SaveChanges();

            // Retrieve users after saving to ensure they have IDs for foreign key assignment
            var staffUser = context.Users.First(u => u.Username == "staff");
            var customerUser = context.Users.First(u => u.Username == "balraj");

            // --- STEP 4: SEED SERVICES ---
            context.Services.AddOrUpdate(s => s.ServiceName,
                new Service { ServiceName = "Classic Haircut", Description = "A timeless, classic haircut.", Price = 150m, IsActive = true },
                new Service { ServiceName = "Beard Trim", Description = "Shape and trim your beard to perfection.", Price = 80m, IsActive = true },
                new Service { ServiceName = "Hair Wash", Description = "Relaxing hair wash and conditioning.", Price = 100m, IsActive = true }
            );
            context.SaveChanges();

            // Retrieve services after saving for foreign key assignment
            var haircutService = context.Services.First(s => s.ServiceName == "Classic Haircut");

            // --- STEP 5: SEED APPOINTMENTS ---
            context.Appointments.AddOrUpdate(a => a.AppointmentDate,
                new Appointment
                {
                    UserID = customerUser.UserID,
                    ServiceID = haircutService.ServiceID,
                    StaffId = staffUser.UserID,
                    AppointmentDate = DateTime.Now.AddDays(2),
                    Status = "Confirmed"
                }
            );
            context.SaveChanges();
        }
    }
}
