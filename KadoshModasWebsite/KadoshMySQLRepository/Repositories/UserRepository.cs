using KadoshDomain.Entities;
using KadoshDomain.Queries;
using KadoshDomain.Repositories;
using KadoshRepository.Persistence.DataContexts;
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
            // Generate 128-bit salt
            byte[] salt = new byte[128 / 8];
            RandomNumberGenerator.Fill(salt);

            int iterations = RandomNumberGenerator.GetInt32(10000, 100000);

            return (GetPasswordHashed(password, salt, iterations), salt, iterations);
        }

        public string GetPasswordHashed(string password, byte[] salt, int iterations)
        {
            // Hash password
            string hashedPassword = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: iterations,
                numBytesRequested: 256 / 8)
            );

            return hashedPassword;
        }

        public async Task<User?> ReadAsync(string username)
        {
            return await _dbSet.SingleOrDefaultAsync(UserQueries.GetUserByUsername(username));
        }

        public async Task<bool> VerifyIfUsernameIsTakenAsync(string username)
        {
            return await _dbSet.AnyAsync(UserQueries.GetUserByUsername(username));
        }

        public async Task<bool> VerifyIfUsernameIsTakenExceptForGivenOneAsync(string username, string usernameToIgnore)
        {
            return await _dbSet.AnyAsync(UserQueries.GetUserByUsernameExceptForGivenOne(username, usernameToIgnore));
        }

        public async Task<User?> GetUserByUsername(string username)
        {
            return await _dbSet.FirstOrDefaultAsync(UserQueries.GetUserByUsername(username));
        }
        
    }
}
