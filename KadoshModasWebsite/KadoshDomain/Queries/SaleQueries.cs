using KadoshDomain.Entities;
using KadoshDomain.ValueObjects;
using System.Linq.Expressions;

namespace KadoshDomain.Queries
{
    public static class SaleQueries
    {
        public static Expression<Func<Sale, Customer?>> IncludeCustomer()
        {
            return x => x.Customer;
        }

        public static Expression<Func<Sale, IReadOnlyCollection<SaleItem>?>> IncludeSaleItems()
        {
            return x => x.SaleItems;
        }
    }
}
