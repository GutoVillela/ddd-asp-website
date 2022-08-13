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

        public override async Task<GetAllCustomersQueryResult> HandleAsync(GetAllCustomersQuery query)
        {
            // Fail Fast Validations
            query.Validate();
            if (!query.IsValid)
            {
                AddNotifications(query);
                var errors = GetErrorsFromNotifications(ErrorCodes.ERROR_INVALID_GET_ALL_CUSTOMERS_QUERY);
                return new GetAllCustomersQueryResult(errors);
            }

            IEnumerable<Customer> customers;

            if (query.PageSize == 0 || query.CurrentPage == 0)
                customers = await _customerRepository.ReadAllAsync(query.IncludeInactives);
            else
                customers = await _customerRepository.ReadAllPagedAsync(query.CurrentPage, query.PageSize, query.IncludeInactives);

            HashSet<CustomerDTO> customersDTO = new();

            foreach (var customer in customers)
            {
                customersDTO.Add(customer);
            }

            GetAllCustomersQueryResult result = new()
            {
                Customers = customersDTO
            };
            result.CustomersCount = await _customerRepository.CountAllAsync(query.IncludeInactives);

            return result;
        }
    }
}
