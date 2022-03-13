using KadoshDomain.Entities;
using System.Linq.Expressions;

namespace KadoshDomain.Queriables
{
    public static class UserQueriable
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
