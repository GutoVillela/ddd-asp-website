using KadoshDomain.Entities;
using KadoshDomain.Queries.Base;
using KadoshDomain.Queries.SaleQueries.DTOs;
using KadoshDomain.Repositories;
using KadoshShared.Constants.ErrorCodes;

namespace KadoshDomain.Queries.SaleQueries.GetAllSalesByCustomerId
{
    public class GetAllSalesByCustomerIdQueryHandler : QueryHandlerBase<GetAllSalesByCustomerIdQuery, GetAllSalesByCustomerIdQueryResult>
    {
        private readonly ISaleRepository _saleRepository;
        private readonly IProductRepository _productRepository;

        public GetAllSalesByCustomerIdQueryHandler(ISaleRepository saleRepository, IProductRepository productRepository)
        {
            _saleRepository = saleRepository;
            _productRepository = productRepository;
        }

        public override async Task<GetAllSalesByCustomerIdQueryResult> HandleAsync(GetAllSalesByCustomerIdQuery command)
        {
            // Fail Fast Validations
            command.Validate();
            if (!command.IsValid)
            {
                AddNotifications(command);
                var errors = GetErrorsFromNotifications(ErrorCodes.ERROR_INVALID_GET_ALL_SALES_BY_CUSTOMER_ID_QUERY);
                return new GetAllSalesByCustomerIdQueryResult(errors);
            }

            IEnumerable<Sale> sales;

            bool queryIsNotPaginated = command.PageSize == 0 || command.CurrentPage == 0;

            if (queryIsNotPaginated)
                sales = await _saleRepository.ReadAllFromCustomerAsync(command.CustomerId!.Value);
            else
                sales = await _saleRepository.ReadAllFromCustomerPaginatedAsync(command.CustomerId!.Value, command.CurrentPage, command.PageSize);

            List<SaleBaseDTO> salesDTO = new();

            foreach (var sale in sales)
            {
                salesDTO.Add(sale);
            }

            // Get product info is requested
            if (command.IncludeProductsInfo)
            {
                for (int i = 0; i < salesDTO.Count; i++)
                {
                    for (int j = 0; j < salesDTO[i].SaleItems.Count; j++)
                    {
                        var productInfo = await _productRepository.ReadAsync(salesDTO[i].SaleItems[j].ProductId);
                        if (productInfo != null)
                            salesDTO[i].SaleItems[j].ProductName = productInfo.Name;
                    }
                }
            }

            GetAllSalesByCustomerIdQueryResult result = new()
            {
                Sales = salesDTO
            };

            if (queryIsNotPaginated)
                result.SalesCount = salesDTO.Count;
            else
                result.SalesCount = await _saleRepository.CountAllFromCustomerAsync(command.CustomerId!.Value);

            return result;
        }
    }
}
