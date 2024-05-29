using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using MolyCoreWeb.Models.DTOs;
using MolyCoreWeb.Models.ViewModels;
using MolyCoreWeb.Services;
using MolyCoreWeb.Models.DBEntitiy;

namespace MolyCoreWeb.Controllers
{
    public class UserController : BaseController
    {
        private readonly IUserService _userService;

        private readonly IUserAuthenticationService _userAuthenticationService;

        public UserController(IUserService userService, IUserAuthenticationService userAuthenticationService)
        {
            _userService = userService;
            _userAuthenticationService = userAuthenticationService;
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

        // 管理頁面
        public async Task<IActionResult> ManagementAsync()
        {
            var users = await _userService.GetAllAsync();
            return View(users);
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
        public async Task<IActionResult> UserProfile(int id)
        {
            var user = await _userService.GetByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

    }
}
