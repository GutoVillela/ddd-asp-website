using KadoshShared.Entities;

namespace KadoshShared.Repositories
{
    public interface IRepository<TEntity> where TEntity : Entity
    {
        Task CreateAsync(TEntity entity);
        Task<TEntity?> ReadAsync(int id);
        Task<TEntity?> ReadAsNoTrackingAsync(int id);
        Task<IEnumerable<TEntity>> ReadAllAsync(bool includeInactive = false);
        Task<IEnumerable<TEntity>> ReadAllPagedAsync(int currentPage, int pageSize, bool includeInactive = false);
        Task<int> CountAllAsync(bool includeInactive = false);
        Task UpdateAsync(TEntity entity);
        void Delete(TEntity entity);
    }
}
