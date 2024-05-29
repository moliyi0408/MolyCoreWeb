using MolyCoreWeb.Models.DBEntitiy;
using MolyCoreWeb.Models.DTOs;
using static MolyCoreWeb.Services.UserService;

namespace MolyCoreWeb.Services
{
    public interface IUserService : IService<UserDto>
    {
        AuthenticationResult Authenticate(UserDto userDto);
        Task<IEnumerable<User>> GetAllUserAsync();
        Task SignInAsync(UserDto userDto, bool isPersistent);
        Task SignOutAsync();

    }
}
