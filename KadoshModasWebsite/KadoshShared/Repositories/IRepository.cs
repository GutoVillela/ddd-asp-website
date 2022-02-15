using KadoshShared.Entities;

namespace KadoshShared.Repositories
{
    public interface IRepository<TEntity> where TEntity : Entity
    {
        Task CreateAsync(TEntity entity);
        Task<TEntity?> ReadAsync(int id);
        Task<IEnumerable<TEntity>> ReadAllAsync();
        Task UpdateAsync(TEntity entity);
        void Delete(TEntity entity);
    }
}
