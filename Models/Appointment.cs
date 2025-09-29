using System;
using System.ComponentModel.DataAnnotations.Schema; // Add this using statement

namespace GroomMate.Models
{
    public class Appointment
    {
        public int AppointmentID { get; set; }
        public int UserID { get; set; }
        public int ServiceID { get; set; }
        public DateTime AppointmentDate { get; set; }
        public string Status { get; set; } // Pending, Confirmed, Rejected, Completed
        public int? StaffID { get; set; }

        [ForeignKey("UserID")] // This maps UserID to the Customer property
        public virtual User Customer { get; set; }

        public virtual Service Service { get; set; }

        [ForeignKey("StaffID")] // This maps StaffID to the Staff property
        public virtual User Staff { get; set; }

        public virtual Feedback Feedback { get; set; }
    }
}
