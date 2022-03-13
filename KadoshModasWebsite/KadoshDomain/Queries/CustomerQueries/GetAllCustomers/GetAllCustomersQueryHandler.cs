using KadoshDomain.Queries.Base;
using KadoshDomain.Queries.CustomerQueries.DTOs;
using KadoshDomain.Repositories;

namespace KadoshDomain.Queries.CustomerQueries.GetAllCustomers
{
    public class GetAllCustomersQueryHandler : QueryHandlerBase<GetAllCustomersQuery, GetAllCustomersQueryResult>
    {
        private readonly ICustomerRepository _customerRepository;

        public GetAllCustomersQueryHandler(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public override async Task<GetAllCustomersQueryResult> HandleAsync(GetAllCustomersQuery command)
        {
            var customers = await _customerRepository.ReadAllAsync();
            HashSet<CustomerDTO> customersDTO = new();

            foreach (var customer in customers)
            {
                customersDTO.Add(customer);
            }

            GetAllCustomersQueryResult result = new()
            {
                Customers = customersDTO
            };
            return result;
        }
    }
}
