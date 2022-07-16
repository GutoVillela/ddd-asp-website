using KadoshDomain.Entities;
using KadoshDomain.Queriables;
using KadoshDomain.Repositories;
using KadoshRepository.Persistence.DataContexts;
using KadoshRepository.Security;
using Microsoft.EntityFrameworkCore;

namespace KadoshRepository.Repositories
{
    public class CustomerRepository : Repository<Customer>, ICustomerRepository
    {
        public CustomerRepository(StoreDataContext dbContext) : base(dbContext)
        {
            
        }

        public async Task<int> CountAllByNameAsync(string customerName)
        {
            return await _dbSet
                .AsNoTracking()
                .Where(CustomerQueriable.GetCustomerByName(customerName))
                .CountAsync();
        }

        public async Task<IEnumerable<Customer>> ReadAllByNameAsync(string customerName)
        {
            return await _dbSet
                .AsNoTracking()
                .Where(CustomerQueriable.GetCustomerByName(customerName))
                .ToListAsync();
        }

        public async Task<IEnumerable<Customer>> ReadAllByNamePaginatedAsync(string customerName, int currentPage, int pageSize)
        {
            int amountToTake = (currentPage - 1) * pageSize;
            return await _dbSet
                .AsNoTracking()
                .Where(CustomerQueriable.GetCustomerByName(customerName))
                .Skip(amountToTake)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<Customer?> GetCustomerByUsernameAsync(string username)
        {
            return await _dbSet.FirstOrDefaultAsync(CustomerQueriable.GetCustomerByUsername(username));
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
