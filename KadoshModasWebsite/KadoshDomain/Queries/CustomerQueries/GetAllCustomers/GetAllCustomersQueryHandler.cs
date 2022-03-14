using KadoshDomain.Entities;
using KadoshDomain.Queries.Base;
using KadoshDomain.Queries.CustomerQueries.DTOs;
using KadoshDomain.Repositories;
using KadoshShared.Constants.ErrorCodes;

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
            // Fail Fast Validations
            command.Validate();
            if (!command.IsValid)
            {
                AddNotifications(command);
                var errors = GetErrorsFromNotifications(ErrorCodes.ERROR_INVALID_GET_ALL_CUSTOMERS_QUERY);
                return new GetAllCustomersQueryResult(errors);
            }

            IEnumerable<Customer> customers;

            if (command.PageSize == 0 || command.CurrentPage == 0)
                customers = await _customerRepository.ReadAllAsync();
            else
                customers = await _customerRepository.ReadAllPagedAsync(command.CurrentPage, command.PageSize);

            HashSet<CustomerDTO> customersDTO = new();

            foreach (var customer in customers)
            {
                customersDTO.Add(customer);
            }

            GetAllCustomersQueryResult result = new()
            {
                Customers = customersDTO
            };
            result.CustomersCount = await _customerRepository.CountAllAsync();

            return result;
        }
    }
}
