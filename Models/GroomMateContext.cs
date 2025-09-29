using System.Data.Entity;

namespace GroomMate.Models
{
    public class GroomMateContext : DbContext
    {
        // --- ADD THIS CONSTRUCTOR ---
        // This line forces Entity Framework to use the connection string
        // from Web.config that is named "GroomMateConnectionString".
        public GroomMateContext() : base("name=GroomMateConnectionString")
        {
        }

        // Your DbSets for Users, Appointments, etc. remain the same.
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<Feedback> Feedbacks { get; set; }
    }
}
