using KadoshDomain.Entities;
using System.Linq.Expressions;

namespace KadoshDomain.Queries
{
    public static class CustomerQueries
    {
        public static Expression<Func<Customer, bool>> GetCustomerByName(string name)
        {
            return x => x.Name == name;
        }
    }
}
