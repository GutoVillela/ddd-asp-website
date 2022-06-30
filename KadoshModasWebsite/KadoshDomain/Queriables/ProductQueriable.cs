using KadoshDomain.Entities;
using System.Linq.Expressions;

namespace KadoshDomain.Queriables
{
    public class ProductQueriable : QueriableBase<Product>
    {
        public static Expression<Func<Product, bool>> GetProductByName(string productName)
        {
            return x => x.Name.Contains(productName);
        }

        public static Expression<Func<Product, bool>> GetByBarCode(string barCode)
        {
            return x => x.BarCode == barCode;
        }
    }
}
