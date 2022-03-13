using KadoshDomain.Queries.Base;
using KadoshDomain.Queries.ProductQueries.DTOs;
using KadoshShared.ValueObjects;

namespace KadoshDomain.Queries.ProductQueries.GetProductById
{
    public class GetProductByIdQueryResult : QueryResultBase
    {
        public GetProductByIdQueryResult() { }

        public GetProductByIdQueryResult(IEnumerable<Error> errors)
        {
            Errors = errors as IReadOnlyCollection<Error>;
        }

        public ProductDTO? Product { get; set; }
    }
}
