using KadoshDomain.Queries.Base;
using KadoshDomain.Queries.ProductQueries.DTOs;
using KadoshShared.ValueObjects;

namespace KadoshDomain.Queries.ProductQueries.GetProductsByName
{
    public class GetProductsByNameQueryResult : QueryResultBase
    {
        public GetProductsByNameQueryResult() { }

        public GetProductsByNameQueryResult(IEnumerable<Error> errors) : base(errors) { }

        public IEnumerable<ProductDTO> Products { get; set; } = new List<ProductDTO>();

        public int ProductsCount { get; set; }
    }
}