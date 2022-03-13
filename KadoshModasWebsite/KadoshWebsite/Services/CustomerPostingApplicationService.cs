using KadoshDomain.Queries.CustomerPostingQueries.DTOs;
using KadoshDomain.Queries.CustomerPostingQueries.GetAllPostingsFromCustomer;
using KadoshShared.ExtensionMethods;
using KadoshShared.Handlers;
using KadoshWebsite.Models;
using KadoshWebsite.Services.Interfaces;

namespace KadoshWebsite.Services
{
    public class CustomerPostingApplicationService : ICustomerPostingApplicationService
    {
        private readonly IQueryHandler<GetAllPostingsFromCustomerQuery, GetAllPostingsFromCustomerQueryResult> _getAllPostingsFromCustomerQueryHandler;

        public CustomerPostingApplicationService(IQueryHandler<GetAllPostingsFromCustomerQuery, GetAllPostingsFromCustomerQueryResult> getAllPostingsFromCustomerQueryHandler)
        {
            _getAllPostingsFromCustomerQueryHandler = getAllPostingsFromCustomerQueryHandler;
        }

        public async Task<IEnumerable<CustomerPostingViewModel>> GetAllPostingsFromCustomerAsync(int customerId)
        {
            GetAllPostingsFromCustomerQuery query = new();
            query.CustomerId = customerId;

            var result = await _getAllPostingsFromCustomerQueryHandler.HandleAsync(query);

            if (!result.Success)
                throw new ApplicationException(result.Errors!.GetAsSingleMessage());

            List<CustomerPostingViewModel> customerPostingsViewModel = new();

            foreach (var posting in result.CustomerPostings)
            {
                customerPostingsViewModel.Add(GetViewModelFromDTO(posting));
            }

            return customerPostingsViewModel;

        }

        private CustomerPostingViewModel GetViewModelFromDTO(CustomerPostingDTO customerPostingDTO)
        {
            return new CustomerPostingViewModel
            {
                SaleId = customerPostingDTO.SaleId,
                PostingDate = customerPostingDTO.PostingDate,
                Value = customerPostingDTO.Value,
                PostingType = customerPostingDTO.PostingType
            };
        }
    }
}
