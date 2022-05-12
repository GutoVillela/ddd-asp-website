using KadoshShared.Entities;

namespace KadoshShared.Repositories
{
    /// <summary>
    /// Repository for legacy database.
    /// </summary>
    /// <typeparam name="TEntity">Entity class.</typeparam>
    /// <typeparam name="TLegacyEntity">Legacy Entity class related to the domain Entity class.</typeparam>
    public interface ILegacyRepository<TEntity, TLegacyEntity> where TEntity : Entity where TLegacyEntity : LegacyEntity<TEntity>
    {
        Task<IEnumerable<TLegacyEntity>> ReadAllAsync(string connectionString);
    }
}
