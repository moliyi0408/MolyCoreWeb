namespace MolyCoreWeb.Repositorys
{
    public interface IUnitOfWork : IDisposable
    {
        Task CompleteAsync(); // Use async for save changes
        IRepository<T> Repository<T>() where T : class;
    }

}
