using GroomMate.Models;
using System;
using System.ComponentModel.DataAnnotations.Schema;
namespace GroomMate.Models
{
    public class Appointment
    {
        public int AppointmentID { get; set; }
        public int UserID { get; set; }
        public int ServiceID { get; set; }
        public DateTime AppointmentDate { get; set; }
        public string Status { get; set; } // e.g., "Pending", "Confirmed", "Completed", "Rejected"

        // NEW: Add a nullable foreign key for the assigned staff member
        public int? StaffId { get; set; }

        // Navigation properties
        public virtual User User { get; set; }
        public virtual Service Service { get; set; }

        // NEW: Navigation property to the assigned staff member
        [ForeignKey("StaffId")]
        public virtual User Staff { get; set; }
    }
}
