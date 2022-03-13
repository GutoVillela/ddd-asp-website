using KadoshDomain.Entities;
using KadoshDomain.Enums;
using System.Linq.Expressions;

namespace KadoshDomain.Queriables
{
    public static class SaleQueriable
    {
        public static Expression<Func<Sale, Customer?>> IncludeCustomer()
        {
            return x => x.Customer;
        }

        public static Expression<Func<Sale, IReadOnlyCollection<SaleItem>?>> IncludeSaleItems()
        {
            return x => x.SaleItems;
        }

        public static Expression<Func<Sale, IReadOnlyCollection<CustomerPosting>?>> IncludePostings()
        {
            return x => x.Postings;
        }

        public static Expression<Func<Sale, bool>> GetSalesByCustomer(int customerId)
        {
            return x => x.CustomerId == customerId;
        }

        public static Expression<Func<Sale, bool>> GetSalesById(int saleId)
        {
            return x => x.Id == saleId;
        }

        //TODO Review method name
        public static Expression<Func<Sale, DateTime>> OrderBySaleDate()
        {
            return x => x.SaleDate;
        }

        public static Expression<Func<Sale, bool>> GetSalesBySituation(ESaleSituation saleSituation)
        {
            return x => x.Situation == saleSituation;
        }
    }
}
