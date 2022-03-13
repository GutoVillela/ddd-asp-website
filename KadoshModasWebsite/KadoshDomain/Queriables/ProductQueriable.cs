using KadoshDomain.Entities;
using System.Linq.Expressions;

namespace KadoshDomain.Queriables
{
    public static class ProductQueriable
    {
        public static Expression<Func<Product, bool>> GetProductById(int id)
        {
            return x => x.Id == id;
        }
    }
}
