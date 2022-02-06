using KadoshDomain.Entities;
using KadoshShared.Repositories;

namespace KadoshDomain.Repositories
{
    public interface IUserRepository : IRepository<User>
    {
        (string hash, byte[] salt, int iterations) GetPasswordHashed(string password);

        string GetPasswordHashed(string password, byte[] salt, int iterations);

        Task<bool> VerifyIfUsernameIsTakenAsync(string username);

        Task<bool> VerifyIfUsernameIsTakenExceptForGivenOneAsync(string username, string usernameToIgnore);

        Task<User?> GetUserByUsername(string username);
    }
}
