using KadoshDomain.Queries.Base;
using KadoshDomain.Queries.BrandQueries.DTOs;
using KadoshShared.ValueObjects;

namespace KadoshDomain.Queries.BrandQueries.GetBrandById
{
    public class GetBrandByIdQueryResult : QueryResultBase
    {
        public GetBrandByIdQueryResult() { }

        public GetBrandByIdQueryResult(IEnumerable<Error> errors) 
        { 
            Errors = errors as IReadOnlyCollection<Error>;
        }

        public BrandDTO? Brand { get; set; }
    }
}
