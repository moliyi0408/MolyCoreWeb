namespace MolyCoreWeb.Models.DTOs
{
    public class UserProfileDto
    {
        public int ProfileId { get; set; }
        public int UserId { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string DateOfBirth { get; set; } = string.Empty;
    }
}
