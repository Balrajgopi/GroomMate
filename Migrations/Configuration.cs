namespace GroomMate.Migrations
{
    using GroomMate.Models;
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
            // Step 1: Seed Roles
            context.Roles.AddOrUpdate(
                r => r.RoleName,
                new Role { RoleName = "Admin" },
                new Role { RoleName = "Staff" },
                new Role { RoleName = "Customer" }
            );
            context.SaveChanges();

            // Step 2: Seed Admin User
            if (!context.Users.Any(u => u.Username == "admin"))
            {
                var adminRole = context.Roles.Single(r => r.RoleName == "Admin");
                var adminUser = new User
                {
                    FullName = "Admin User",
                    Username = "admin",
                    Password = "password",
                    Email = "admin@groommate.com",
                    RoleID = adminRole.RoleID
                };
                context.Users.Add(adminUser);
            }

            // NEW: Step 3: Seed Staff User
            if (!context.Users.Any(u => u.Username == "staff"))
            {
                var staffRole = context.Roles.Single(r => r.RoleName == "Staff");
                var staffUser = new User
                {
                    FullName = "Staff Member",
                    Username = "staff",
                    Password = "password",
                    Email = "staff@groommate.com",
                    RoleID = staffRole.RoleID
                };
                context.Users.Add(staffUser);
            }
            context.SaveChanges();

            // Step 4: Seed Services
            context.Services.AddOrUpdate(
                s => s.ServiceName,
                new Service { ServiceName = "Classic Haircut", Description = "A timeless, classic haircut.", Price = 250m },
                new Service { ServiceName = "Beard Trim", Description = "Shape and trim your beard to perfection.", Price = 150m },
                new Service { ServiceName = "Hair Wash", Description = "Relaxing hair wash and conditioning.", Price = 100m },
                new Service { ServiceName = "Royal Shave", Description = "A traditional hot towel shave.", Price = 120m },
                new Service { ServiceName = "Detox Facial", Description = "A cleansing and rejuvenating facial treatment.", Price = 500m }
            );
            context.SaveChanges();
        }
    }
}
