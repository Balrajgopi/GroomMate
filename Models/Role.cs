using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GroomMate.Models
{
    public class Role
    {
        [Key]
        public int RoleID { get; set; }
        public string RoleName { get; set; }

        public virtual ICollection<User> Users { get; set; }
    }
}
