using KadoshDomain.Entities;
using System.Linq.Expressions;

namespace KadoshDomain.Queries
{
    public static class ProductQueries
    {
        public static Expression<Func<Product, bool>> GetProductById(int id)
        {
            return x => x.Id == id;
        }
    }
}
