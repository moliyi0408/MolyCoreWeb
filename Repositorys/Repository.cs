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

        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<TEntity> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public void Create(TEntity entity)
        {
            _dbSet.Add(entity);
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
    }
}
