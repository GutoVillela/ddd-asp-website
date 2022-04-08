﻿using KadoshShared.Entities;
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
            TEntity? entity = await _dbSet.SingleOrDefaultAsync(x => x.Id == id);
            return entity;
        }

        public virtual async Task<TEntity?> ReadAsNoTrackingAsync(int id)
        {
            TEntity? entity = await _dbSet.AsNoTracking().SingleOrDefaultAsync(x => x.Id == id);
            return entity;
        }

        public virtual async Task<IEnumerable<TEntity>> ReadAllAsync()
        {
            return await _dbSet.AsNoTracking().ToListAsync();
        }

        public virtual async Task<IEnumerable<TEntity>> ReadAllPagedAsync(int currentPage, int pageSize)
        {
            int amountToTake = (currentPage - 1) * pageSize;
            return await _dbSet
                .AsNoTracking()
                .Skip(amountToTake)
                .Take(pageSize)
                .ToListAsync();
        }

        public virtual async Task UpdateAsync(TEntity entity)
        {
            _context.Update(entity);
        }

        public virtual async Task<int> CountAllAsync()
        {
            return await _dbSet
                .CountAsync();
        }
    }
}
