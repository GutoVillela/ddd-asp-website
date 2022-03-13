using KadoshDomain.Queries.Base;
using KadoshDomain.Queries.ProductQueries.DTOs;
using KadoshDomain.Repositories;

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
            var products = await _productRepository.ReadAllAsync();
            HashSet<ProductDTO> productsDTO = new();

            foreach (var product in products)
            {
                productsDTO.Add(product);
            }

            GetAllProductsQueryResult result = new()
            {
                Products = productsDTO
            };
            return result;
        }
    }
}
