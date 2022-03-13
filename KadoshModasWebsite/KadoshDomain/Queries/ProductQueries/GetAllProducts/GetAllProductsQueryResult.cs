using KadoshDomain.Queries.Base;
using KadoshDomain.Queries.ProductQueries.DTOs;

namespace KadoshDomain.Queries.ProductQueries.GetAllProducts
{
    public class GetAllProductsQueryResult : QueryResultBase
    {
        public IEnumerable<ProductDTO> Products { get; set; } = new List<ProductDTO>();
    }
}
