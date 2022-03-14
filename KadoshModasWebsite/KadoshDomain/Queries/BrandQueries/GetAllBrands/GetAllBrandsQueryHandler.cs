using KadoshDomain.Entities;
using KadoshDomain.Queries.Base;
using KadoshDomain.Queries.BrandQueries.DTOs;
using KadoshDomain.Repositories;
using KadoshShared.Constants.ErrorCodes;

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
            // Fail Fast Validations
            command.Validate();
            if (!command.IsValid)
            {
                AddNotifications(command);
                var errors = GetErrorsFromNotifications(ErrorCodes.ERROR_INVALID_GET_ALL_BRANDS_QUERY);
                return new GetAllBrandsQueryResult(errors);
            }

            IEnumerable<Brand> brands;

            if (command.PageSize == 0 || command.CurrentPage == 0)
                brands = await _brandRepository.ReadAllAsync();
            else
                brands = await _brandRepository.ReadAllPagedAsync(command.CurrentPage, command.PageSize);
            
            
            HashSet<BrandDTO> brandsDTO = new();

            foreach (var brand in brands)
            {
                brandsDTO.Add(brand);
            }

            GetAllBrandsQueryResult result = new()
            {
                Brands = brandsDTO
            };
            result.BrandsCount = await _brandRepository.CountAllAsync();

            return result;
        }
    }
}
