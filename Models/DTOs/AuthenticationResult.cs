namespace MolyCoreWeb.Models.DTOs
{
    public class AuthenticationResult
    {
        public bool Success { get; set; }
        public string ErrorMessage { get; set; } = string.Empty;
        public UserDto User { get; set; } = new UserDto();
    }
}
