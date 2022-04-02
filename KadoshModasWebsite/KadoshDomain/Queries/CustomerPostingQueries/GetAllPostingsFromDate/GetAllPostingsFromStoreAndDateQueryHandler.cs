using KadoshDomain.Entities;
using KadoshDomain.Queries.Base;
using KadoshDomain.Queries.CustomerPostingQueries.DTOs;
using KadoshDomain.Repositories;
using KadoshShared.Constants.ErrorCodes;

namespace KadoshDomain.Queries.CustomerPostingQueries.GetAllPostingsFromDate
{
    public class GetAllPostingsFromStoreAndDateQueryHandler : QueryHandlerBase<GetAllPostingsFromStoreAndDateQuery, GetAllPostingsFromStoreAndDateQueryResult>
    {

        private readonly ICustomerPostingRepository _customerPostingRepository;

        public GetAllPostingsFromStoreAndDateQueryHandler(ICustomerPostingRepository customerPostingRepository)
        {
            _customerPostingRepository = customerPostingRepository;
        }

        public override async Task<GetAllPostingsFromStoreAndDateQueryResult> HandleAsync(GetAllPostingsFromStoreAndDateQuery query)
        {
            // Fail Fast Validations
            query.Validate();
            if (!query.IsValid)
            {
                AddNotifications(query);
                var errors = GetErrorsFromNotifications(ErrorCodes.ERROR_INVALID_GET_ALL_POSTINGS_FROM_DATE_QUERY);
                return new GetAllPostingsFromStoreAndDateQueryResult(errors);
            }

            IEnumerable<CustomerPosting> customerPostings;

            bool isQueryPaginated = query.PageSize != 0 && query.CurrentPage != 0;
            DateTime localStartDate = new(query.LocalDate!.Value.Year, query.LocalDate!.Value.Month, query.LocalDate!.Value.Day, hour: 0, minute: 0, second: 0);
            DateTime localEndDate = new(query.LocalDate!.Value.Year, query.LocalDate!.Value.Month, query.LocalDate!.Value.Day, hour: 23, minute: 59, second: 59);
            DateTime utcStartDate = TimeZoneInfo.ConvertTimeToUtc(localStartDate, query.LocalTimeZone!);
            DateTime utcEndDate = TimeZoneInfo.ConvertTimeToUtc(localEndDate, query.LocalTimeZone!);

            if (isQueryPaginated)
                customerPostings = await _customerPostingRepository.ReadAllPostingsFromStoreAndDatePaginatedAsync(
                    utcStartDate,
                    utcEndDate,
                    query.StoreId!.Value,
                    query.CurrentPage, 
                    query.PageSize);
            else
                customerPostings = await _customerPostingRepository.ReadAllPostingsFromStoreAndDateAsync(
                    utcStartDate,
                    utcEndDate,
                    query.StoreId!.Value);

            List<CustomerPostingDTO> customerPostingDTOs = new();

            foreach (var posting in customerPostings!)
            {
                customerPostingDTOs.Add(posting);
            }

            GetAllPostingsFromStoreAndDateQueryResult result = new()
            {
                CustomerPostings = customerPostingDTOs
            };

            if (query.GetTotal)
            {
                if (isQueryPaginated)
                    customerPostings = await _customerPostingRepository.ReadAllPostingsFromStoreAndDateAsync(
                        utcStartDate,
                        utcEndDate,
                        query.StoreId!.Value);

                result.TotalCredit = CustomerPosting.CalculateTotalCredit(customerPostings);
                result.TotalDebit = CustomerPosting.CalculateTotalDebit(customerPostings);
            }

            result.CustomerPostingsCount = customerPostings.Count();
            
            return result;
        }
    }
}
