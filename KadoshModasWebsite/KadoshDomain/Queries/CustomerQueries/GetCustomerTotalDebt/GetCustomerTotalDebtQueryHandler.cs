using KadoshDomain.Queries.Base;
using KadoshDomain.Repositories;
using KadoshShared.Constants.ErrorCodes;

namespace KadoshDomain.Queries.CustomerQueries.GetCustomerTotalDebt
{
    public class GetCustomerTotalDebtQueryHandler : QueryHandlerBase<GetCustomerTotalDebtQuery, GetCustomerTotalDebtQueryResult>
    {
        private readonly ISaleRepository _saleRepository;

        public GetCustomerTotalDebtQueryHandler(ISaleRepository saleRepository)
        {
            _saleRepository = saleRepository;
        }

        public override async Task<GetCustomerTotalDebtQueryResult> HandleAsync(GetCustomerTotalDebtQuery command)
        {
            // Fail Fast Validations
            command.Validate();
            if (!command.IsValid)
            {
                AddNotifications(command);
                var errors = GetErrorsFromNotifications(ErrorCodes.ERROR_INVALID_GET_CUSTOMER_TOTAL_DEBT_QUERY);
                return new GetCustomerTotalDebtQueryResult(errors);
            }

            decimal totalDebt = 0;
            var customerSales = await _saleRepository.ReadAllOpenFromCustomerAsync(command.CustomerId!.Value);

            foreach (var sale in customerSales)
            {
                totalDebt += sale.TotalToPay;
            }

            GetCustomerTotalDebtQueryResult result = new()
            {
                CustomerTotalDebt = totalDebt
            };

            return result;
        }
    }
}
