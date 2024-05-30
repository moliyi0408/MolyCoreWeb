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

        public UserService(IRepository<User> userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task Create(User user)
        {
            // 检查用户名是否已经存在
            var existingUser = await _userRepository.GetByCondition(u => u.UserName == user.UserName);
            if (existingUser != null)
            {
                throw new Exception("Username already exists.");
            }
            await _userRepository.Create(user); // 保存新用户到数据库
        }

        IQueryable<User> IService<User>.Reads()
        {
            throw new NotImplementedException();
        }

        public async Task Update(User user)
        {
            _userRepository.Update(user);
            await _userRepository.SaveChanges();
        }

        public void Delete(User entity)
        {
            throw new NotImplementedException();
        }

        public async Task DeleteAsync(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user != null)
            {
                _userRepository.Delete(user);
                await _userRepository.SaveChanges();
            }
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            var users = await _userRepository.GetAllAsync();
            return users;
        }

        public async Task<User> GetByIdAsync(int id)
        {
            return await _userRepository.GetByIdAsync(id);
        }

     
    }

}