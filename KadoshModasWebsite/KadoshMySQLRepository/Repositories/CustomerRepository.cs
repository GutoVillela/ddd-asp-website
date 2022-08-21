using System.Linq;
using KadoshDomain.Entities;
using KadoshDomain.Queriables;
using KadoshDomain.Repositories;
using KadoshRepository.Persistence.DataContexts;
using KadoshRepository.Security;
using KadoshShared.Entities;
using Microsoft.EntityFrameworkCore;

namespace KadoshRepository.Repositories
{
    public class CustomerRepository : Repository<Customer>, ICustomerRepository
    {
        public CustomerRepository(StoreDataContext dbContext) : base(dbContext)
        {
        }

        public override async Task<Customer?> ReadAsync(int id)
        {
            Customer? entity = await _dbSet
                .Include(CustomerQueriable.IncludeBoundedCustomers())
                .SingleOrDefaultAsync(QueriableBase<Customer>.GetById(id));
            return entity;
        }

        public override async Task<IEnumerable<Customer>> ReadAllAsync(bool includeInactive = false)
        {
            if (includeInactive)
                return await _dbSet
                    .AsNoTracking()
                    .Where(CustomerQueriable.CustomerIsNotBounded())
                    .Include(CustomerQueriable.IncludeBoundedCustomers())
                    .ToListAsync();

            return await _dbSet
            .AsNoTracking()
                    .Where(QueriableBase<Customer>.GetIfActive()) // TODO Think if that was really needed
                    .Where(CustomerQueriable.CustomerIsNotBounded())
                    .Include(CustomerQueriable.IncludeBoundedCustomers())
            .ToListAsync();
        }

        public override async Task<IEnumerable<Customer>> ReadAllPagedAsync(int currentPage, int pageSize, bool includeInactive = false)
        {
            int amountToTake = (currentPage - 1) * pageSize;
            if (includeInactive)
                return await _dbSet
                    .Where(CustomerQueriable.CustomerIsNotBounded())
                    .Include(CustomerQueriable.IncludeBoundedCustomers())
                    .AsNoTracking()
                    .Skip(amountToTake)
                    .Take(pageSize)
                    .ToListAsync();
            else
                return await _dbSet
                .AsNoTracking()
                .Where(QueriableBase<Customer>.GetIfActive())
                .Where(CustomerQueriable.CustomerIsNotBounded())
                .Include(CustomerQueriable.IncludeBoundedCustomers())
                .Skip(amountToTake)
                .Take(pageSize)
                .ToListAsync();
        }

        public override async Task<int> CountAllAsync(bool includeInactive = false)
        {
            if (includeInactive)
                return await _dbSet
                    .Where(CustomerQueriable.CustomerIsNotBounded())
                    .CountAsync();

            return await _dbSet
                    .Where(QueriableBase<Customer>.GetIfActive())
                    .Where(CustomerQueriable.CustomerIsNotBounded())
                    .CountAsync();
        }

        public async Task<int> CountAllByNameAsync(string customerName, bool includeInactive = false)
        {
            if(includeInactive)
                return await _dbSet
                    .AsNoTracking()
                    .Where(CustomerQueriable.GetCustomerByName(customerName))
                    .Where(CustomerQueriable.CustomerIsNotBounded())
                    .CountAsync();

            return await _dbSet
                    .AsNoTracking()
                    .Where(QueriableBase<Customer>.GetIfActive())
                    .Where(CustomerQueriable.GetCustomerByName(customerName))
                    .Where(CustomerQueriable.CustomerIsNotBounded())
                    .CountAsync();
        }

        public async Task<IEnumerable<Customer>> ReadAllByNameAsync(string customerName, bool includeInactive = false)
        {
            if (includeInactive)
                return await _dbSet
                    .AsNoTracking()
                    .Where(CustomerQueriable.GetCustomerByName(customerName))
                    .Where(CustomerQueriable.CustomerIsNotBounded())
                    .Include(CustomerQueriable.IncludeBoundedCustomers())
                    .ToListAsync();

            return await _dbSet
                    .AsNoTracking()
                    .Where(QueriableBase<Customer>.GetIfActive())
                    .Where(CustomerQueriable.GetCustomerByName(customerName))
                    .Where(CustomerQueriable.CustomerIsNotBounded())
                    .Include(CustomerQueriable.IncludeBoundedCustomers())
                    .ToListAsync();
        }

        public async Task<IEnumerable<Customer>> ReadAllByNamePaginatedAsync(string customerName, int currentPage, int pageSize, bool includeInactive = false)
        {
            int amountToTake = (currentPage - 1) * pageSize;
            if (includeInactive)
                return await _dbSet
                    .AsNoTracking()
                    .Where(CustomerQueriable.GetCustomerByName(customerName))
                    .Where(CustomerQueriable.CustomerIsNotBounded())
                    .Include(CustomerQueriable.IncludeBoundedCustomers())
                    .Skip(amountToTake)
                    .Take(pageSize)
                    .ToListAsync();

            return await _dbSet
                    .AsNoTracking()
                    .Where(QueriableBase<Customer>.GetIfActive())
                    .Where(CustomerQueriable.GetCustomerByName(customerName))
                    .Where(CustomerQueriable.CustomerIsNotBounded())
                    .Include(CustomerQueriable.IncludeBoundedCustomers())
                    .Skip(amountToTake)
                    .Take(pageSize)
                    .ToListAsync();
        }

        public async Task<Customer?> GetCustomerByUsernameAsync(string username)
        {
            return await _dbSet
                .Include(CustomerQueriable.IncludeBoundedCustomers())
                .FirstOrDefaultAsync(CustomerQueriable.GetCustomerByUsername(username));
        }

        public (string hash, byte[] salt, int iterations) GetPasswordHashed(string password)
        {
            return PasswordEncodeUtil.GetPasswordHashed(password);
        }

        public string GetPasswordHashed(string password, byte[] salt, int iterations)
        {
            return PasswordEncodeUtil.GetPasswordHashed(password, salt, iterations);
        }

        public async Task<bool> VerifyIfUsernameIsTakenAsync(string username)
        {
            return await _dbSet.AnyAsync(CustomerQueriable.GetCustomerByUsername(username));
        }
    }
}
