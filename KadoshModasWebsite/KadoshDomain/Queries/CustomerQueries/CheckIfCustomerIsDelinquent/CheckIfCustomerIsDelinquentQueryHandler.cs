using KadoshDomain.Enums;
using KadoshDomain.Queries.Base;
using KadoshDomain.Repositories;
using KadoshShared.Constants.ErrorCodes;

namespace KadoshDomain.Queries.CustomerQueries.CheckIfCustomerIsDelinquent
{
    public class CheckIfCustomerIsDelinquentQueryHandler : QueryHandlerBase<CheckIfCustomerIsDelinquentQuery, CheckIfCustomerIsDelinquentQueryResult>
    {
        private readonly ISaleRepository _saleRepository;

        public CheckIfCustomerIsDelinquentQueryHandler(ISaleRepository saleRepository)
        {
            _saleRepository = saleRepository;
        }

        public async override Task<CheckIfCustomerIsDelinquentQueryResult> HandleAsync(CheckIfCustomerIsDelinquentQuery query)
        {
            // Fail Fast Validations
            query.Validate();
            if (!query.IsValid)
            {
                AddNotifications(query);
                var errors = GetErrorsFromNotifications(ErrorCodes.ERROR_INVALID_CHECK_IF_CUSTOMER_IS_DELINQUENT_QUERY);
                return new CheckIfCustomerIsDelinquentQueryResult(errors);
            }

            // Get all open sales
            var openSales = await _saleRepository.ReadAllOpenFromCustomerAsync(query.CustomerId.Value);

            if (!openSales.Any())
                return new CheckIfCustomerIsDelinquentQueryResult();

            foreach (var sale in openSales)
            {
                if (sale.IsLatePaymentSale(query.IntervalSinceLastPaymentInDays))
                    return new CheckIfCustomerIsDelinquentQueryResult() { IsDelinquent = true };// Customer is Delinquent
            }

            return new CheckIfCustomerIsDelinquentQueryResult()
            {
                IsDelinquent = false
            };
        }
    }
}
