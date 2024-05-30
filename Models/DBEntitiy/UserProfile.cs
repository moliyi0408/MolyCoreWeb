namespace MolyCoreWeb.Models.DBEntitiy
{
    public class UserProfile
    {
        public int ProfileId { get; set; }
        public int UserId { get; set; }
        public string Email { get; set; } = string.Empty;
        public string DateOfBirth { get; set; } = string.Empty;

        public User? User { get; set; }
    }
}
