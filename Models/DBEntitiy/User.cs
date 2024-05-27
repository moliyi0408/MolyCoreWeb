namespace MolyCoreWeb.Models.DBEntitiy
{
    public class User
    {
        public int UserId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string Permission { get; set; } = string.Empty;

        public ICollection<UserProfile>? UserProfiles { get; set; }
    }
}
