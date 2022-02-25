using KadoshDomain.Entities;
using KadoshDomain.Repositories;
using KadoshWebsite.Models;
using KadoshWebsite.Services.Interfaces;

namespace KadoshWebsite.Services
{
    public class CustomerPostingApplicationService : ICustomerPostingApplicationService
    {
        private readonly ICustomerPostingRepository _customerPostingRepository;

        public CustomerPostingApplicationService(ICustomerPostingRepository customerPostingRepository)
        {
            _customerPostingRepository = customerPostingRepository;
        }

        public async Task<IEnumerable<CustomerPostingViewModel>> GetAllPostingsFromCustomerAsync(int customerId)
        {
            var customerPostings = await _customerPostingRepository.ReadAllPostingsFromCustomerAsync(customerId);

            List<CustomerPostingViewModel> customerPostingsViewModel = new();

            foreach (var posting in customerPostings)
            {
                customerPostingsViewModel.Add(GetViewModelFromEntity(posting));
            }

            return customerPostingsViewModel;

        }

        private CustomerPostingViewModel GetViewModelFromEntity(CustomerPosting customerPosting)
        {
            return new CustomerPostingViewModel
            {
                SaleId = customerPosting.SaleId,
                PostingDate = customerPosting.PostingDate,
                Value = customerPosting.Value,
                PostingType = customerPosting.Type
    };
        }
    }
}
