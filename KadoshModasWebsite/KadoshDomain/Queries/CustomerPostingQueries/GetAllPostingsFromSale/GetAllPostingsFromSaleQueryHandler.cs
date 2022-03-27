using KadoshDomain.Entities;
using KadoshDomain.Queries.Base;
using KadoshDomain.Queries.CustomerPostingQueries.DTOs;
using KadoshDomain.Repositories;
using KadoshShared.Constants.ErrorCodes;

namespace KadoshDomain.Queries.CustomerPostingQueries.GetAllPostingsFromSale
{
    public class GetAllPostingsFromSaleQueryHandler : QueryHandlerBase<GetAllPostingsFromSaleQuery, GetAllPostingsFromSaleQueryResult>
    {
        private readonly ICustomerPostingRepository _customerPostingRepository;

        public GetAllPostingsFromSaleQueryHandler(ICustomerPostingRepository customerPostingRepository)
        {
            _customerPostingRepository = customerPostingRepository;
        }

        public override async Task<GetAllPostingsFromSaleQueryResult> HandleAsync(GetAllPostingsFromSaleQuery query)
        {
            // Fail Fast Validations
            query.Validate();
            if (!query.IsValid)
            {
                AddNotifications(query);
                var errors = GetErrorsFromNotifications(ErrorCodes.ERROR_INVALID_GET_ALL_POSTINGS_FROM_SALE_QUERY);
                return new GetAllPostingsFromSaleQueryResult(errors);
            }

            IEnumerable<CustomerPosting> customerPostings;

            bool isQueryPaginated = query.PageSize != 0 && query.CurrentPage != 0;

            if (isQueryPaginated)
                customerPostings = await _customerPostingRepository.ReadAllPostingsFromSalePaginatedAsync(query.SaleId!.Value, query.CurrentPage, query.PageSize);
            else
                customerPostings = await _customerPostingRepository.ReadAllPostingsFromSaleAsync(query.SaleId!.Value);

            List<CustomerPostingDTO> customerPostingDTOs = new();

            foreach (var posting in customerPostings!)
            {
                customerPostingDTOs.Add(posting);
            }

            GetAllPostingsFromSaleQueryResult result = new()
            {
                CustomerPostings = customerPostingDTOs
            };

            if (isQueryPaginated)
                result.CustomerPostingsCount = await _customerPostingRepository.CountAllFromSaleAsync(query.SaleId!.Value);
            else
                result.CustomerPostingsCount = customerPostingDTOs.Count;

            return result;
        }
    }
}
