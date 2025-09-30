using System.Data.Entity;

namespace GroomMate.Models
{
    public class GroomMateContext : DbContext
    {
        public GroomMateContext() : base("name=GroomMateConnectionString")
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<Feedback> Feedbacks { get; set; }

        // THIS METHOD MUST BE INSIDE THE CLASS
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure the precision for the Price property on the Service entity
            modelBuilder.Entity<Service>()
                .Property(s => s.Price)
                .HasPrecision(18, 2);
        }
    }
}
