using KadoshDomain.Queries.Base;
using KadoshDomain.Queries.ProductQueries.DTOs;
using KadoshShared.ValueObjects;

namespace KadoshDomain.Queries.ProductQueries.GetProductByBarCode
{
    public class GetProductByBarCodeQueryResult : QueryResultBase
    {
        public GetProductByBarCodeQueryResult() { }

        public GetProductByBarCodeQueryResult(IEnumerable<Error> errors)
        {
            Errors = errors as IReadOnlyCollection<Error>;
        }

        public ProductDTO? Product { get; set; }
    }
}
