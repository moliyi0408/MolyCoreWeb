using System.ComponentModel.DataAnnotations;

namespace MolyCoreWeb.Models.ViewModels
{
    public class UserViewModel
    {
        [Required]
        public string UserName { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;
    }
}
