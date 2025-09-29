namespace GroomMate.Models
{
    public class User
    {
        public int UserID { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public int RoleID { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }

        // --- NEW PROPERTY ---
        // This will be used to mark a user as deleted instead of permanently removing them.
        public bool IsDeleted { get; set; }

        public virtual Role Role { get; set; }
    }
}
