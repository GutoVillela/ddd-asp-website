using KadoshDomain.Entities;
using KadoshDomain.Queries.Base;
using KadoshDomain.Queries.ProductQueries.DTOs;
using KadoshDomain.Repositories;
using KadoshShared.Constants.ErrorCodes;

namespace KadoshDomain.Queries.ProductQueries.GetAllProducts
{
    public class GetAllProductsQueryHandler : QueryHandlerBase<GetAllProductsQuery, GetAllProductsQueryResult>
    {
        private readonly IProductRepository _productRepository;

        public GetAllProductsQueryHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public override async Task<GetAllProductsQueryResult> HandleAsync(GetAllProductsQuery command)
        {
            // Fail Fast Validations
            command.Validate();
            if (!command.IsValid)
            {
                AddNotifications(command);
                var errors = GetErrorsFromNotifications(ErrorCodes.ERROR_INVALID_GET_ALL_PRODUCTS_QUERY);
                return new GetAllProductsQueryResult(errors);
            }

            IEnumerable<Product> products;

            if (command.PageSize == 0 || command.CurrentPage == 0)
                products = await _productRepository.ReadAllAsync();
            else
                products = await _productRepository.ReadAllPagedAsync(command.CurrentPage, command.PageSize);

            HashSet<ProductDTO> productsDTO = new();

            foreach (var product in products)
            {
                productsDTO.Add(product);
            }

            GetAllProductsQueryResult result = new()
            {
                Products = productsDTO
            };
            result.ProductsCount = await _productRepository.CountAllAsync();
            return result;
        }
    }
}
