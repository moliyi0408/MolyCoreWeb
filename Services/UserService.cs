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

        public void Create(User entity)
        {
            throw new NotImplementedException();
        }

        IQueryable<User> IService<User>.Reads()
        {
            throw new NotImplementedException();
        }

        public void Update(User entity)
        {
            throw new NotImplementedException();
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
                _userRepository.SaveChanges();
            }
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            var users = await _userRepository.GetAllAsync();
            return users;
        }

        public Task<User> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

     
    }
}