using KadoshDomain.Entities;
using KadoshDomain.Queriables;
using KadoshDomain.Repositories;
using KadoshRepository.Persistence.DataContexts;
using KadoshRepository.Security;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;

namespace KadoshRepository.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(StoreDataContext dbContext) : base(dbContext)
        {
            
        }

        public (string hash, byte[] salt, int iterations) GetPasswordHashed(string password)
        {
            return PasswordEncodeUtil.GetPasswordHashed(password);
        }

        public string GetPasswordHashed(string password, byte[] salt, int iterations)
        {
            return PasswordEncodeUtil.GetPasswordHashed(password, salt, iterations);
        }

        public async Task<User?> ReadAsync(string username)
        {
            return await _dbSet.SingleOrDefaultAsync(UserQueriable.GetUserByUsername(username));
        }

        public async Task<bool> VerifyIfUsernameIsTakenAsync(string username)
        {
            return await _dbSet.AnyAsync(UserQueriable.GetUserByUsername(username));
        }

        public async Task<bool> VerifyIfUsernameIsTakenExceptForGivenOneAsync(string username, string usernameToIgnore)
        {
            return await _dbSet.AnyAsync(UserQueriable.GetUserByUsernameExceptForGivenOne(username, usernameToIgnore));
        }

        public async Task<User?> GetUserByUsername(string username)
        {
            return await _dbSet.FirstOrDefaultAsync(UserQueriable.GetUserByUsername(username));
        }
        
    }
}
