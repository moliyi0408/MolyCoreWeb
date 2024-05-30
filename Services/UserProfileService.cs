using MolyCoreWeb.Models.DBEntitiy;
using MolyCoreWeb.Repositorys;

namespace MolyCoreWeb.Services
{
    public class UserProfileService : IUserProfileService
    {

        private readonly IRepository<UserProfile> _userProfileRepository;

        public UserProfileService(IRepository<UserProfile> userProfileRepository)
        {
            _userProfileRepository = userProfileRepository;
        }

        public Task Create(UserProfile entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(UserProfile entity)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<UserProfile>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<UserProfile> GetByIdAsync(int id)
        {
            return await _userProfileRepository.GetByIdAsync(id);
        }

  

        public IQueryable<UserProfile> Reads()
        {
            throw new NotImplementedException();
        }

        public Task Update(UserProfile entity)
        {
            throw new NotImplementedException();
        }
    }
}
