using KadoshShared.Entities;

namespace KadoshShared.Repositories
{
    public interface IRepository<TEntity> where TEntity : Entity
    {
        Task CreateAsync(TEntity entity);
        Task<TEntity?> ReadAsync(int id);
        Task<TEntity?> ReadAsNoTrackingAsync(int id);
        Task<IEnumerable<TEntity>> ReadAllAsync();
        Task<IEnumerable<TEntity>> ReadAllPagedAsync(int currentPage, int pageSize);
        Task<int> CountAllAsync();
        Task UpdateAsync(TEntity entity);
        void Delete(TEntity entity);
    }
}
