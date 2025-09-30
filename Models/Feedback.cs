using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GroomMate.Models
{
    public class Feedback
    {
        [Key, ForeignKey("Appointment")]
        public int AppointmentID { get; set; }

        public int Rating { get; set; }
        public string Comments { get; set; }

        public virtual Appointment Appointment { get; set; }
    }
}
