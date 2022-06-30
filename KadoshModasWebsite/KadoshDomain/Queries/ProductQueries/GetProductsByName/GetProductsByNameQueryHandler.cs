using KadoshDomain.Entities;
using KadoshDomain.Queries.Base;
using KadoshDomain.Queries.ProductQueries.DTOs;
using KadoshDomain.Repositories;
using KadoshShared.Constants.ErrorCodes;

namespace KadoshDomain.Queries.ProductQueries.GetProductsByName
{
    public class GetProductsByNameQueryHandler : QueryHandlerBase<GetProductsByNameQuery, GetProductsByNameQueryResult>
    {
        private readonly IProductRepository _productRepository;

        public GetProductsByNameQueryHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public override async Task<GetProductsByNameQueryResult> HandleAsync(GetProductsByNameQuery query)
        {
            // Fail Fast Validations
            query.Validate();
            if (!query.IsValid)
            {
                AddNotifications(query);
                var errors = GetErrorsFromNotifications(ErrorCodes.ERROR_INVALID_GET_PRODUCTS_BY_NAME_QUERY);
                return new GetProductsByNameQueryResult(errors);
            }

            IEnumerable<Product> products;
            bool isQueryPaginated = query.PageSize != 0 && query.CurrentPage != 0;

            if (isQueryPaginated)
                products = await _productRepository.ReadAllByNamePagedAsync(query.ProductName, query.CurrentPage, query.PageSize);
            else
                products = await _productRepository.ReadAllByNameAsync(query.ProductName);

            HashSet<ProductDTO> productsDTO = new();

            foreach (var product in products)
            {
                productsDTO.Add(product);
            }

            GetProductsByNameQueryResult result = new()
            {
                Products = productsDTO
            };

            if (isQueryPaginated)
                result.ProductsCount = await _productRepository.CountAllByNameAsync(query.ProductName);
            else
                result.ProductsCount = productsDTO.Count;
            
            return result;
        }
    }
}
