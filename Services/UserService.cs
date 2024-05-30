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
        private readonly IUnitOfWork _unitOfWork;

        //private readonly IRepository<User> _userRepository;

        public UserService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task Create(User user)
        {
            // 檢查用戶名是否已經存在
            var existingUser = await _unitOfWork.Repository<User>().GetByCondition(u => u.UserName == user.UserName);
            if (existingUser != null)
            {
                throw new Exception("Username already exists.");
            }
            await _unitOfWork.Repository<User>().Create(user);

            await _unitOfWork.CompleteAsync(); // 保存變更
        }
        IQueryable<User> IService<User>.Reads()
        {
            throw new NotImplementedException();
        }

        public async Task Update(User user)
        {
            _unitOfWork.Repository<User>().Update(user);
            await _unitOfWork.CompleteAsync(); // 保存變更
        }

        public void Delete(User entity)
        {
            throw new NotImplementedException();
        }

        public async Task DeleteAsync(int id)
        {
            var user = await _unitOfWork.Repository<User>().GetByIdAsync(id);
            if (user != null)
            {
                _unitOfWork.Repository<User>().Delete(user);
                await _unitOfWork.CompleteAsync(); // 保存變更
            }
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _unitOfWork.Repository<User>().GetAllAsync();
        }

        public async Task<User> GetByIdAsync(int id)
        {
            return await _unitOfWork.Repository<User>().GetByIdAsync(id);
        }

    }

}