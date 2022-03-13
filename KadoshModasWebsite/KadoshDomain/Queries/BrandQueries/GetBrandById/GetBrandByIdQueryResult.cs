using KadoshDomain.Queries.Base;
using KadoshDomain.Queries.BrandQueries.DTOs;
using KadoshShared.ValueObjects;

namespace KadoshDomain.Queries.BrandQueries.GetBrandById
{
    public class GetBrandByIdQueryResult : QueryResultBase
    {
        public GetBrandByIdQueryResult() { }

        public GetBrandByIdQueryResult(IReadOnlyCollection<Error> errors) 
        { 
            Errors = errors;
        }

        public BrandDTO? Brand { get; set; }
    }
}
