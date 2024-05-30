using Microsoft.EntityFrameworkCore;
using MolyCoreWeb.Datas;
using System.Collections;

namespace MolyCoreWeb.Repositorys
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly WebDbContext _context;
        private bool _disposed;
        private Hashtable _repositories;

        public UnitOfWork(WebDbContext context)
        {
            _context = context;
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
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

        public IRepository<T> Repository<T>() where T : class
        {
            _repositories ??= [];

            var type = typeof(T).Name;

            if (!_repositories.ContainsKey(type))
            {
                var repositoryType = typeof(Repository<>);

                var repositoryInstance =
                    Activator.CreateInstance(repositoryType
                            .MakeGenericType(typeof(T)), _context);

                _repositories.Add(type, repositoryInstance);
            }

            return (IRepository<T>)_repositories[type];
        }

        public async Task CompleteAsync()
        {
            await _context.SaveChangesAsync();
        }

    }
}
