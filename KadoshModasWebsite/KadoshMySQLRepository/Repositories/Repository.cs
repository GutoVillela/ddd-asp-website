using KadoshDomain.Queriables;
using KadoshShared.Entities;
using KadoshShared.Repositories;
using Microsoft.EntityFrameworkCore;

namespace KadoshRepository.Repositories
{
    public abstract class Repository<TEntity> : IRepository<TEntity> where TEntity : Entity
    {

        protected readonly DbContext _context;
        protected readonly DbSet<TEntity> _dbSet;

        protected Repository(DbContext dbContext)
        {
            _context = dbContext;
            _dbSet = dbContext.Set<TEntity>();
        }

        public virtual async Task CreateAsync(TEntity entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public virtual void Delete(TEntity entity) 
        { 
            _dbSet.Remove(entity);
        }

        public virtual async Task<TEntity?> ReadAsync(int id)
        {
            TEntity? entity = await _dbSet.SingleOrDefaultAsync(QueriableBase<TEntity>.GetById(id));
            return entity;
        }

        public virtual async Task<TEntity?> ReadAsNoTrackingAsync(int id)
        {
            TEntity? entity = await _dbSet.AsNoTracking().SingleOrDefaultAsync(QueriableBase<TEntity>.GetById(id));
            return entity;
        }

        public virtual async Task<IEnumerable<TEntity>> ReadAllAsync()
        {
            return await _dbSet
                .AsNoTracking()
                //.Where(QueriableBase<TEntity>.GetIfActive()) // TODO Think if that was really needed
                .ToListAsync();
        }

        public virtual async Task<IEnumerable<TEntity>> ReadAllPagedAsync(int currentPage, int pageSize)
        {
            int amountToTake = (currentPage - 1) * pageSize;
            return await _dbSet
                .AsNoTracking()
                .Where(QueriableBase<TEntity>.GetIfActive())
                .Skip(amountToTake)
                .Take(pageSize)
                .ToListAsync();
        }

        public virtual async Task UpdateAsync(TEntity entity)
        {
            //TODO remove Async from this method
            _context.Update(entity);
        }

        public virtual async Task<int> CountAllAsync()
        {
            return await _dbSet
                .Where(QueriableBase<TEntity>.GetIfActive())
                .CountAsync();
        }
    }
}
