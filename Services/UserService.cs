using System.Linq.Expressions;
using AutoMapper;
using MolyCoreWeb.Models.DBEntitiy;
using MolyCoreWeb.Models.DTOs;
using MolyCoreWeb.Repositorys;
using MolyCoreWeb.Services;

public class UserService : IUserService
{
    private readonly IRepository<User> _userRepository;
    private readonly IMapper _mapper;

    public UserService(IRepository<User> userRepository, IMapper mapper)
    {
        _userRepository = userRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<User>> GetAllUserAsync()
    {
        var users = await _userRepository.GetAllAsync();
        return users;
    }

    public async Task<IEnumerable<UserDto>> GetAllAsync()
    {
        var users = await _userRepository.GetAllAsync();
        return _mapper.Map<IEnumerable<UserDto>>(users);
    }

    public async Task<UserDto> GetByIdAsync(int id)
    {
        var user = await _userRepository.GetByIdAsync(id);
        return _mapper.Map<UserDto>(user);
    }

    public void Create(UserDto userDto)
    {
        var user = _mapper.Map<User>(userDto);
        _userRepository.Create(user);
        _userRepository.SaveChanges();
    }
 
    public IQueryable<UserDto> Reads()
    {
        var users = _userRepository.Reads();
        return _mapper.ProjectTo<UserDto>(users);
    }

    public void Update(UserDto userDto)
    {
        var user = _mapper.Map<User>(userDto);
        _userRepository.Update(user);
        _userRepository.SaveChanges();
    }

    public void Delete(UserDto userDto)
    {
        var user = _mapper.Map<User>(userDto);
        _userRepository.Delete(user);
        _userRepository.SaveChanges();
    }

    public void SaveChanges()
    {
        _userRepository.SaveChanges();
    }

    //public UserDto Read(Expression<Func<User, bool>> predicate)
    //{
    //    var user = _userRepository.Read(predicate);
    //    return _mapper.Map<UserDto>(user);
    //}

    //public UserDto Read(Expression<Func<UserDto, bool>> predicate)
    //{
    //    throw new NotImplementedException();
    //}

    public UserDto Authenticate(UserDto userDto)
    {
        //查找user 資料庫
        var user = _userRepository.GetAllAsync().Result
            .FirstOrDefault(u => u.UserName == userDto.UserName && u.PasswordHash == userDto.Password);

        if (user == null)
        {
            return null;
        }

        return new UserDto
        {
            UserId = user.UserId,
            UserName = user.UserName,
            Password = user.PasswordHash
        };
    }
}
