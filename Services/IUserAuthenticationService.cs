using MolyCoreWeb.Models.DTOs;

namespace MolyCoreWeb.Services
{
    public interface IUserAuthenticationService : IService<UserDto>
    {
        AuthenticationResult Authenticate(UserDto userDto);
        Task SignInAsync(UserDto userDto, bool isPersistent);
        Task SignOutAsync();

    }
}
