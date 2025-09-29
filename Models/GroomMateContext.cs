using System.Data.Entity;

namespace GroomMate.Models
{
    public class GroomMateContext : DbContext
    {
        public GroomMateContext() : base("name=GroomMateConnectionString")
        {
        }

        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<Feedback> Feedbacks { get; set; }
    }
}
