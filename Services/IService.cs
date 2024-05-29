using MolyCoreWeb.Models.DTOs;
using System.Linq.Expressions;

namespace MolyCoreWeb.Services
{
    public interface IService<TEntity>
    {
        Task<IEnumerable<TEntity>> GetAllAsync();
        Task<TEntity> GetByIdAsync(int id);
        void Create(TEntity entity);
        IQueryable<TEntity> Reads();
        Task Update(TEntity entity);
        void Delete(TEntity entity);

    }

}
