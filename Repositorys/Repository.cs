using MolyCoreWeb.Datas;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using MolyCoreWeb.Models.DBEntitiy;

namespace MolyCoreWeb.Repositorys
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        private readonly WebDbContext _context;
        private readonly DbSet<TEntity> _dbSet;

        public Repository(WebDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<TEntity>();
        }

     
        public async Task Create(TEntity entity)
        {
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync(); // 确保保存更改
        }

        public TEntity Read(Expression<Func<TEntity, bool>> predicate)
        {
            return _dbSet.FirstOrDefault(predicate);
        }

        public IQueryable<TEntity> Reads()
        {
            return _dbSet.AsQueryable();
        }

        public void Update(TEntity entity)
        {
            _dbSet.Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
        }

        public void Delete(TEntity entity)
        {
            if (_context.Entry(entity).State == EntityState.Detached)
            {
                _dbSet.Attach(entity);
            }
            _dbSet.Remove(entity);
        }

        public async Task SaveChanges()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<User> GetUserByUsernameAndPassword(string username, string password)
        {
            return await _context.User.FirstOrDefaultAsync(u => u.UserName == username && u.PasswordHash == password);

        }

       

        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<TEntity> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task<TEntity> GetByCondition(Expression<Func<TEntity, bool>> predicate)
        {
            return await _dbSet.FirstOrDefaultAsync(predicate);

        }

        public async Task<IEnumerable<TEntity>> ExecuteSqlQueryAsync(string sql, params object[] parameters)
        {
            return await _dbSet.FromSqlRaw(sql, parameters).ToListAsync();
        }
    }
}
