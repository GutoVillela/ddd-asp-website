using KadoshDomain.Queries.Base;
using KadoshDomain.Queries.BrandQueries.DTOs;
using KadoshDomain.Repositories;

namespace KadoshDomain.Queries.BrandQueries.GetAllBrands
{
    public class GetAllBrandsQueryHandler : QueryHandlerBase<GetAllBrandsQuery, GetAllBrandsQueryResult>
    {
        private readonly IBrandRepository _brandRepository;

        public GetAllBrandsQueryHandler(IBrandRepository brandRepository)
        {
            _brandRepository = brandRepository;
        }

        public override async Task<GetAllBrandsQueryResult> HandleAsync(GetAllBrandsQuery command)
        {
            var brands = await _brandRepository.ReadAllAsync();
            HashSet<BrandDTO> brandsDTO = new();

            foreach (var brand in brands)
            {
                brandsDTO.Add(brand);
            }

            GetAllBrandsQueryResult result = new()
            {
                Brands = brandsDTO
            };
            return result;
        }
    }
}
