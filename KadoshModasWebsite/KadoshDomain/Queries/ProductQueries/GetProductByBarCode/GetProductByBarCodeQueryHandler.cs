using KadoshDomain.Queries.Base;
using KadoshDomain.Repositories;
using KadoshShared.Constants.ErrorCodes;
using KadoshShared.Constants.QueriesMessages;

namespace KadoshDomain.Queries.ProductQueries.GetProductByBarCode
{
    public class GetProductByBarCodeQueryHandler : QueryHandlerBase<GetProductByBarCodeQuery, GetProductByBarCodeQueryResult>
    {
        private readonly IProductRepository _productRepository;

        public GetProductByBarCodeQueryHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public override async Task<GetProductByBarCodeQueryResult> HandleAsync(GetProductByBarCodeQuery query)
        {
            // Fail Fast Validations
            query.Validate();
            if (!query.IsValid)
            {
                AddNotifications(query);
                var errors = GetErrorsFromNotifications(ErrorCodes.ERROR_INVALID_GET_PRODUCT_BY_BARCODE_QUERY);
                return new GetProductByBarCodeQueryResult(errors);
            }

            var product = await _productRepository.ReadByBarCodeAsync(query.BarCode);

            if (product is null)
            {
                AddNotification(nameof(product), ProductQueriesMessages.ERROR_PRODUCT_BARCODE_NOT_FOUND);
                var errors = GetErrorsFromNotifications(ErrorCodes.ERROR_PRODUCT_NOT_FOUND);
                return new GetProductByBarCodeQueryResult(errors);
            }

            GetProductByBarCodeQueryResult result = new()
            {
                Product = product
            };

            return result;
        }
    }
}
