using MolyCoreWeb.Models.DBEntitiy;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace MolyCoreWeb.Repositorys
{
    public interface IRepository<TEntity> 
    {
        Task<IEnumerable<TEntity>> GetAllAsync();
        Task<TEntity> GetByIdAsync(int id);
        // 新增一筆資料。
        Task Create(TEntity entity);
        // 取得第一筆符合條件的內容。如果符合條件有多筆，也只取得第一筆。
        TEntity Read(Expression<Func<TEntity, bool>> predicate);
        // <returns>Entity全部筆數的IQueryable。</returns>
        IQueryable<TEntity> Reads();
        // 更新一筆資料的內容。
        void Update(TEntity entity);
        // 刪除一筆資料內容。
        void Delete(TEntity entity);
        // 儲存異動。
        Task SaveChanges();

        Task<User> GetUserByUsernameAndPassword(string username, string password);

        Task<TEntity> GetByCondition(Expression<Func<TEntity, bool>> predicate);

    }
}
