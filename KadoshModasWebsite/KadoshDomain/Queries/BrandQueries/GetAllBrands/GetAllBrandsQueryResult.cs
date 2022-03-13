using KadoshDomain.Queries.Base;
using KadoshDomain.Queries.BrandQueries.DTOs;

namespace KadoshDomain.Queries.BrandQueries.GetAllBrands
{
    public class GetAllBrandsQueryResult : QueryResultBase
    {
        public IEnumerable<BrandDTO> Brands { get; set; } = new List<BrandDTO>();
    }
}
