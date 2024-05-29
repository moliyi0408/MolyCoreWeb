using MolyCoreWeb.Models.DBEntitiy;
using MolyCoreWeb.Models.DTOs;
using System.Threading.Tasks;
using static MolyCoreWeb.Services.UserService;

namespace MolyCoreWeb.Services
{
    public interface IUserService : IService<User>
    {
        Task DeleteAsync(int id);
    }
}
