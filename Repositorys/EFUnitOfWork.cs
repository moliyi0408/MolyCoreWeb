using Microsoft.EntityFrameworkCore;
using System.Collections;

namespace MolyCoreWeb.Repositorys
{
    public class EFUnitOfWork : IUnitOfWork
    {
        private readonly DbContext _context;

        private bool _disposed;
        private Hashtable _repositories;

        /// 設定此Unit of work(UOF)的Context。
        public EFUnitOfWork(DbContext context)
        {
            _context = context;
            _repositories = new Hashtable();
        }

        /// 儲存所有異動。
        public void Save()
        {
            _context.SaveChanges();
        }

        /// 清除此Class的資源。
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// 清除此Class的資源。
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }

            _disposed = true;
        }

   
        /// 取得某一個Entity的Repository。
        /// 如果沒有取過，會initialise一個
        /// 如果有就取得之前initialise的那個。
        public IRepository<T> Repository<T>() where T : class
        {
            if (_repositories == null)
            {
                _repositories = [];
            }

            var type = typeof(T).Name;

            if (!_repositories.ContainsKey(type))
            {
                var repositoryType = typeof(IRepository<>);

                var repositoryInstance =
                    Activator.CreateInstance(repositoryType
                            .MakeGenericType(typeof(T)), _context);

                _repositories.Add(type, repositoryInstance);
            }

            return (IRepository<T>)_repositories[type];
        }
    }
}