using KadoshDomain.Entities;
using KadoshShared.Repositories;

namespace KadoshDomain.Repositories
{
    public interface IUserRepository : IRepository<User>
    {
        string GetPasswordHashed(string password);

        Task<bool> VerifyIfUsernameIsTakenAsync(string username);

        Task<bool> VerifyIfUsernameIsTakenExceptForGivenOneAsync(string username, string usernameToIgnore);
    }
}
