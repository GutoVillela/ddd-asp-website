using KadoshDomain.Queries.Base;
using KadoshDomain.Queries.BrandQueries.DTOs;
using KadoshShared.ValueObjects;

namespace KadoshDomain.Queries.BrandQueries.GetAllBrands
{
    public class GetAllBrandsQueryResult : QueryResultBase
    {
        public GetAllBrandsQueryResult() { }

        public GetAllBrandsQueryResult(IEnumerable<Error> errors) : base(errors) { }

        public IEnumerable<BrandDTO> Brands { get; set; } = new List<BrandDTO>();

        public int BrandsCount { get; set; }
    }
}
