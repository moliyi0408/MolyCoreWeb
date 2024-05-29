using System.Linq.Expressions;
using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using MolyCoreWeb.Models.DBEntitiy;
using MolyCoreWeb.Models.DTOs;
using MolyCoreWeb.Repositorys;

namespace MolyCoreWeb.Services
{
    public class UserService : IUserService
    {
        private readonly IRepository<User> _userRepository;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserService(IRepository<User> userRepository, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
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
            //使用者輸入查找user 資料庫對應資料
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

        public async Task SignInAsync(UserDto userDto, bool isPersistent)
        {
            var user = await _userRepository.GetUserByUsernameAndPassword(userDto.UserName, userDto.Password);

            var claims = new List<Claim>
            {
                new(ClaimTypes.Name, userDto.UserName),
                new(ClaimTypes.NameIdentifier, userDto.UserId.ToString())
            };

            if (user.Permission == "admin")
            {
                claims.Add(new Claim("Permission", "admin"));
            }
            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = new AuthenticationProperties
            {
                IsPersistent = isPersistent
            };

            var httpContext = _httpContextAccessor.HttpContext;

            if (httpContext != null)
            {
                await httpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    authProperties);
            }
            else
            {
                throw new InvalidOperationException("HttpContext is null. Unable to sign in the user.");
            }
        }

        public async Task SignOutAsync()
        {
            var httpContext = _httpContextAccessor.HttpContext;

            if (httpContext != null)
            {
                await httpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            }
            else
            {
                throw new InvalidOperationException("HttpContext is null. Unable to sign out the user.");
            }
        }
    }
}