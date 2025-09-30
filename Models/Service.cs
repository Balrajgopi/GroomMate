using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GroomMate.Models
{
    public class Service
    {
        [Key]
        public int ServiceID { get; set; }
        public string ServiceName { get; set; }
        public string Description { get; set; }

        [Column(TypeName = "decimal")]
        public decimal Price { get; set; }

        public bool IsActive { get; set; }

        public virtual ICollection<Appointment> Appointments { get; set; }
    }
}
