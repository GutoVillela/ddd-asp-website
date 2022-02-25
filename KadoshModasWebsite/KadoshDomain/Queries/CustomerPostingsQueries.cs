using KadoshDomain.Entities;
using System.Linq.Expressions;

namespace KadoshDomain.Queries
{
    public static class CustomerPostingsQueries
    {
        public static Expression<Func<CustomerPosting, bool>> GetCustomerPostingsByCustomerId(int customerId)
        {
            // TODO figure out some way to garantee that x.Sale is not null.
            return x => x.Sale.CustomerId == customerId;
        }

        public static Expression<Func<CustomerPosting, Sale?>> IncludeSale()
        {
            return x => x.Sale;
        }

    }
}
