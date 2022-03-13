using KadoshDomain.Entities;
using System.Linq.Expressions;

namespace KadoshDomain.Queriables
{
    public static class CustomerPostingsQueriable
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
