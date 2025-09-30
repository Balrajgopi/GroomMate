using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GroomMate.Models
{
    public class User
    {
        [Key]
        public int UserID { get; set; }

        public string Username { get; set; }
        public string Password { get; set; } // Reminder: Hash this in a real application
        public string FullName { get; set; }
        public string Email { get; set; }
        public bool IsDeleted { get; set; } = false;

        [ForeignKey("Role")]
        public int RoleID { get; set; }
        public virtual Role Role { get; set; }

        // --- NAVIGATION PROPERTIES ---

        // A collection of appointments where this user is the CUSTOMER
        [InverseProperty("User")]
        public virtual ICollection<Appointment> AppointmentsAsCustomer { get; set; }

        // A collection of appointments where this user is the STAFF member
        [InverseProperty("Staff")]
        public virtual ICollection<Appointment> AppointmentsAsStaff { get; set; }
    }
}
