using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using MolyCoreWeb.Models.DTOs;
using MolyCoreWeb.Models.ViewModels;
using MolyCoreWeb.Services;
using MolyCoreWeb.Models.DBEntitiy;
using System.Security.Claims;

namespace MolyCoreWeb.Controllers
{
    public class UserController : BaseController
    {
        private readonly IUserService _userService;

        private readonly IUserAuthenticationService _userAuthenticationService;

        private readonly IUserProfileService _userProfileService;

        public UserController(IUserService userService, IUserAuthenticationService userAuthenticationService, IUserProfileService userProfileService)
        {
            _userService = userService;
            _userAuthenticationService = userAuthenticationService;
            _userProfileService = userProfileService;
        }

        // 登入頁面
        public IActionResult Login()
        {
            return View();
        }

        //UserViewModel 來接收從視圖傳遞過來的用戶輸入
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var userDto = new UserDto
                {
                    UserName = model.UserName,
                    Password = model.Password
                };

                var authResult = _userAuthenticationService.Authenticate(userDto);
                if (authResult.Success)
                {
                    await _userAuthenticationService.SignInAsync(authResult.User, true);
                    // 登錄成功，重定向到首頁
                    return RedirectToAction("Index", "Home");
                }


                ModelState.AddModelError("", authResult.ErrorMessage);

            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await _userAuthenticationService.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        // 註冊頁面
        public IActionResult Create()
        {
            return View();
        }

        // 管理頁面
        public async Task<IActionResult> ManagementAsync()
        {
            var users = await _userService.GetAllAsync();
            return View(users);
        }

        [HttpPost]
        public async Task<IActionResult> Create(UserViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var newUser = new User
                    {
                        UserName = model.UserName,
                        PasswordHash = model.Password,
                        Permission = "user"
                    };

                    var newUserProfile = new UserProfile
                    {
                        Email = model.Email,
                        DateOfBirth = model.DateOfBirth
                    };

                    await _userService.CreateUserWithProfile(newUser, newUserProfile);


                    return RedirectToAction("Login");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }
         
            return View(model); // 如果模型验证失败，返回带有错误信息的视图
        }

        [HttpPost]
        public async Task<IActionResult> Edit(UserViewModel model)
        {
            var existingUser = await _userService.GetByIdAsync(model.UserId);
            if (existingUser == null)
            {
                return NotFound();
            }

            existingUser.UserName = model.UserName;
            existingUser.PasswordHash = model.Password;
          
            // 更新其他属性

            await _userService.Update(existingUser);
            return RedirectToAction("Management");

        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await _userService.DeleteAsync(id);
            return RedirectToAction("Management");
        }

        // 個人資料頁面
        public async Task<IActionResult> Profile()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId != null)
            {
                var userProfile = await _userProfileService.GetByIdAsync(int.Parse(userId));

                if (userProfile == null)
                {
                    return NotFound();
                }

                return View(userProfile);
            }
            else
            {
                return NotFound();
            }


        }

    }
}
