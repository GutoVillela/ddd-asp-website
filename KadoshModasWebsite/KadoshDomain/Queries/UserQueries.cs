using KadoshDomain.Entities;
using System.Linq.Expressions;

namespace KadoshDomain.Queries
{
    public static class UserQueries
    {
        public static Expression<Func<User, bool>> GetUserByUsername(string username)
        {
            return x => x.Username == username;
        }

        public static Expression<Func<User, bool>> GetUserByUsernameExceptForGivenOne(string username, string usernameToIgnore)
        {
            return x => x.Username == username && x.Username != usernameToIgnore;
        }
    }
}
