using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using MolyCoreWeb.Models.DTOs;
using MolyCoreWeb.Models.ViewModels;
using MolyCoreWeb.Services;

namespace MolyCoreWeb.Controllers
{
    public class UserController : BaseController
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        // 登入頁面
        public IActionResult Login()
        {
            return View();
        }

        //UserViewModel 來接收從視圖傳遞過來的用戶輸入
        [HttpPost]
        public async Task<IActionResult> Login(UserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var userDto = new UserDto
                {
                    UserName = model.UserName,
                    Password = model.Password
                };

                var user = _userService.Authenticate(userDto);
                if (user != null)
                {
                    await _userService.SignInAsync(user, true);
                    // 登錄成功，重定向到首頁
                    return RedirectToAction("Index", "Home");
                }


                ModelState.AddModelError("", "Invalid login attempt.");
            }
       
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await _userService.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        // 管理頁面
        public async Task<IActionResult> ManagementAsync()
        {
            var users = await _userService.GetAllUserAsync();
            return View(users);
        }

        // 個人資料頁面
        public IActionResult UserProfile()
        {
            return View();
        }
        

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetAllUsers()
        {
            var users = await _userService.GetAllAsync();
            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserDto>> GetUserById(int id)
        {
            var user = await _userService.GetByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        [HttpPost]
        public ActionResult CreateUser([FromBody] UserDto userDto)
        {
            if (userDto == null)
            {
                return BadRequest();
            }
            _userService.Create(userDto);
            return CreatedAtAction(nameof(GetUserById), new { id = userDto.UserId }, userDto);
        }

        [HttpPut("{id}")]
        public ActionResult UpdateUser(int id, [FromBody] UserDto userDto)
        {
            if (userDto == null || userDto.UserId != id)
            {
                return BadRequest();
            }
            _userService.Update(userDto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public ActionResult DeleteUser(int id)
        {
            var user = _userService.GetByIdAsync(id).Result;
            if (user == null)
            {
                return NotFound();
            }
            _userService.Delete(user);
            return NoContent();
        }
    }
}
