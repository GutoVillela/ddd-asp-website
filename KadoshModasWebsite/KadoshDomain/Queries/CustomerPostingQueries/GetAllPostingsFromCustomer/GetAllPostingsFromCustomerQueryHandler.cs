using KadoshDomain.Entities;
using KadoshDomain.Queries.Base;
using KadoshDomain.Queries.CustomerPostingQueries.DTOs;
using KadoshDomain.Repositories;
using KadoshShared.Constants.ErrorCodes;

namespace KadoshDomain.Queries.CustomerPostingQueries.GetAllPostingsFromCustomer
{
    public class GetAllPostingsFromCustomerQueryHandler : QueryHandlerBase<GetAllPostingsFromCustomerQuery, GetAllPostingsFromCustomerQueryResult>
    {
        private readonly ICustomerPostingRepository _customerPostingRepository;

        public GetAllPostingsFromCustomerQueryHandler(ICustomerPostingRepository customerPostingRepository)
        {
            _customerPostingRepository = customerPostingRepository;
        }

        public override async Task<GetAllPostingsFromCustomerQueryResult> HandleAsync(GetAllPostingsFromCustomerQuery query)
        {
            // Fail Fast Validations
            query.Validate();
            if (!query.IsValid)
            {
                AddNotifications(query);
                var errors = GetErrorsFromNotifications(ErrorCodes.ERROR_INVALID_GET_ALL_POSTINGS_FROM_CUSTOMER_QUERY);
                return new GetAllPostingsFromCustomerQueryResult(errors);
            }

            IEnumerable<CustomerPosting> customerPostings;

            bool isQueryPaginated = query.PageSize != 0 && query.CurrentPage != 0;

            if (isQueryPaginated)
                customerPostings = await _customerPostingRepository.ReadAllPostingsFromCustomerPaginatedAsync(query.CustomerId!.Value, query.CurrentPage, query.PageSize);
            else
                customerPostings = await _customerPostingRepository.ReadAllPostingsFromCustomerAsync(query.CustomerId!.Value);

            List<CustomerPostingDTO> customerPostingDTOs = new();  

            foreach(var posting in customerPostings!)
            {
                customerPostingDTOs.Add(posting);
            }

            GetAllPostingsFromCustomerQueryResult result = new()
            {
                CustomerPostings = customerPostingDTOs
            };

            if (isQueryPaginated)
                result.CustomerPostingsCount = await _customerPostingRepository.CountAllFromCustomerAsync(query.CustomerId!.Value);
            else
                result.CustomerPostingsCount = customerPostingDTOs.Count;

            return result;
        }
    }
}
