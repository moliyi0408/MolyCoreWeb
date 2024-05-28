using MolyCoreWeb.Models.DBEntitiy;
using MolyCoreWeb.Models.DTOs;

namespace MolyCoreWeb.Services
{
    public interface IUserService : IService<UserDto>
    {
        UserDto Authenticate(UserDto userDto);
        Task<IEnumerable<User>> GetAllUserAsync();
        Task SignInAsync(UserDto userDto, bool isPersistent);
        Task SignOutAsync();

    }
}
