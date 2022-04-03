using KadoshDomain.Enums;
using KadoshDomain.ExtensionMethods;
using KadoshDomain.Queries.Base;
using KadoshDomain.Queries.CustomerQueries.DTOs;
using KadoshDomain.Repositories;
using KadoshShared.Constants.ErrorCodes;

namespace KadoshDomain.Queries.CustomerQueries.GetAllDelinquentCustomers
{
    public class GetAllDelinquentCustomersQueryHandler : QueryHandlerBase<GetAllDelinquentCustomersQuery, GetAllDelinquentCustomersQueryResult>
    {
        private readonly ISaleRepository _saleRepository;

        public GetAllDelinquentCustomersQueryHandler(ISaleRepository saleRepository)
        {
            _saleRepository = saleRepository;
        }

        public override async Task<GetAllDelinquentCustomersQueryResult> HandleAsync(GetAllDelinquentCustomersQuery query)
        {
            // Fail Fast Validations
            query.Validate();
            if (!query.IsValid)
            {
                AddNotifications(query);
                var errors = GetErrorsFromNotifications(ErrorCodes.ERROR_INVALID_GET_ALL_DELINQUENT_CUSTOMERS_QUERY);
                return new GetAllDelinquentCustomersQueryResult(errors);
            }

            // Get all open sales
            var openSales = await _saleRepository.ReadAllFromSituationAsync(ESaleSituation.Open);

            if(!openSales.Any())
                return new GetAllDelinquentCustomersQueryResult();

            HashSet<CustomerDTO> customersDTO = new();

            foreach (var sale in openSales)
            {
                DateTime lastCreditPostingDate;

                if (sale.Postings.Any(x => x.Type.IsCreditType()))
                    lastCreditPostingDate = sale.Postings.Where(x => x.Type.IsCreditType()).Max(x => x.PostingDate);
                else
                    lastCreditPostingDate = sale.SaleDate;

                TimeSpan differenceFromToday = DateTime.UtcNow.Subtract(lastCreditPostingDate);

                if (differenceFromToday.TotalDays >= query.IntervalSinceLastPaymentInDays)
                    customersDTO.Add(sale.Customer!);
            }

            GetAllDelinquentCustomersQueryResult result = new()
            {
                DelinquentCustomers = customersDTO,
                DelinquentCustomersCount = customersDTO.Count
            };

            return result;
        }
    }
}
