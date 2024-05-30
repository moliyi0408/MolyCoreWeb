using AutoMapper;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using MolyCoreWeb.Models.DBEntitiy;
using MolyCoreWeb.Models.DTOs;
using MolyCoreWeb.Repositorys;
using System.Security.Claims;

namespace MolyCoreWeb.Services
{
    public class UserAuthenticationService : IUserAuthenticationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserAuthenticationService(IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task Create(UserDto userDto)
        {
            var user = _mapper.Map<User>(userDto);
            await _unitOfWork.Repository<User>().Create(user);
            await _unitOfWork.CompleteAsync();
        }

        public IQueryable<UserDto> Reads()
        {
            var users = _unitOfWork.Repository<User>().Reads();
            return _mapper.ProjectTo<UserDto>(users);
        }

        public async Task Update(UserDto userDto)
        {
            var user = _mapper.Map<User>(userDto);
            _unitOfWork.Repository<User>().Update(user);
            await _unitOfWork.CompleteAsync();
        }

        public void Delete(UserDto entity)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<UserDto>> GetAllAsync()
        {
            var users = await _unitOfWork.Repository<User>().GetAllAsync();
            return _mapper.Map<IEnumerable<UserDto>>(users);
        }

        public async Task<UserDto> GetByIdAsync(int id)
        {
            var user = await _unitOfWork.Repository<User>().GetByIdAsync(id);
            return _mapper.Map<UserDto>(user);
        }

        public AuthenticationResult Authenticate(UserDto userDto)
        {
            //使用者輸入查找user 資料庫對應資料
            var user = _unitOfWork.Repository<User>().GetAllAsync().Result
                .FirstOrDefault(u => u.UserName == userDto.UserName && u.PasswordHash == userDto.Password);

            if (user == null)
            {
                return new AuthenticationResult
                {
                    Success = false,
                    ErrorMessage = "Account or password is incorrect"
                };
            }

            return new AuthenticationResult
            {
                Success = true,
                User = new UserDto
                {
                    UserId = user.UserId,
                    UserName = user.UserName,
                    Password = user.PasswordHash
                }
            };
        }

        public async Task SignInAsync(UserDto userDto, bool isPersistent)
        {
            var user = await _unitOfWork.Repository<User>().GetUserByUsernameAndPassword(userDto.UserName, userDto.Password);

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
