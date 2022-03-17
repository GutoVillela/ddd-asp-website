using KadoshDomain.Queries.Base;
using KadoshDomain.Queries.ProductQueries.DTOs;
using KadoshShared.ValueObjects;

namespace KadoshDomain.Queries.ProductQueries.GetAllProducts
{
    public class GetAllProductsQueryResult : QueryResultBase
    {
        public GetAllProductsQueryResult() { }

        public GetAllProductsQueryResult(IEnumerable<Error> errors) : base(errors) { }

        public IEnumerable<ProductDTO> Products { get; set; } = new List<ProductDTO>();

        public int ProductsCount { get; set; }
    }
}
