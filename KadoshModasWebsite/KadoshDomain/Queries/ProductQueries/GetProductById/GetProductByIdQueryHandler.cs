using KadoshDomain.Queries.Base;
using KadoshDomain.Repositories;
using KadoshShared.Constants.ErrorCodes;
using KadoshShared.Constants.ServicesMessages;

namespace KadoshDomain.Queries.ProductQueries.GetProductById
{
    public class GetProductByIdQueryHandler : QueryHandlerBase<GetProductByIdQuery, GetProductByIdQueryResult>
    {
        private readonly IProductRepository _productRepository;

        public GetProductByIdQueryHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public override async Task<GetProductByIdQueryResult> HandleAsync(GetProductByIdQuery command)
        {
            // Fail Fast Validations
            command.Validate();
            if (!command.IsValid)
            {
                AddNotifications(command);
                var errors = GetErrorsFromNotifications(ErrorCodes.ERROR_INVALID_GET_PRODUCT_BY_ID_QUERY);
                return new GetProductByIdQueryResult(errors);
            }

            var product = await _productRepository.ReadAsync(command.ProductId!.Value);

            if (product is null)
            {
                AddNotification(nameof(product), ProductServiceMessages.ERROR_PRODUCT_ID_NOT_FOUND);
                var errors = GetErrorsFromNotifications(ErrorCodes.ERROR_PRODUCT_NOT_FOUND);
                return new GetProductByIdQueryResult(errors);
            }

            GetProductByIdQueryResult result = new()
            {
                Product = product
            };

            return result;
        }
    }
}
