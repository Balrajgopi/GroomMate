using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GroomMate.Models
{
    public class Appointment
    {
        [Key]
        public int AppointmentID { get; set; }

        // Foreign key for the customer (User)
        public int UserID { get; set; }

        // Foreign key for the Service
        public int ServiceID { get; set; }

        // Foreign key for the assigned staff member (User) - Nullable
        public int? StaffId { get; set; }

        public DateTime AppointmentDate { get; set; }
        public string Status { get; set; } // e.g., "Pending", "Confirmed", "Completed"

        // --- NAVIGATION PROPERTIES ---

        // Links the 'UserID' foreign key to the User who is the customer
        [ForeignKey("UserID")]
        [InverseProperty("AppointmentsAsCustomer")]
        public virtual User User { get; set; }

        // Links to the Service being booked
        [ForeignKey("ServiceID")]
        public virtual Service Service { get; set; }

        // Links the 'StaffId' foreign key to the User who is the staff member
        [ForeignKey("StaffId")]
        [InverseProperty("AppointmentsAsStaff")]
        public virtual User Staff { get; set; }

        // This creates a one-to-one relationship with Feedback
        public virtual Feedback Feedback { get; set; }
    }
}
