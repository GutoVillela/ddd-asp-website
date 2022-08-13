using KadoshDomain.Entities;
using KadoshDomain.Queries.Base;
using KadoshDomain.Queries.CustomerQueries.DTOs;
using KadoshDomain.Repositories;
using KadoshShared.Constants.ErrorCodes;

namespace KadoshDomain.Queries.CustomerQueries.GetCustomersByName
{
    public class GetCustomersByNameQueryHandler : QueryHandlerBase<GetCustomersByNameQuery, GetCustomersByNameQueryResult>
    {
        private readonly ICustomerRepository _customerRepository;

        public GetCustomersByNameQueryHandler(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public override async Task<GetCustomersByNameQueryResult> HandleAsync(GetCustomersByNameQuery query)
        {
            // Fail Fast Validations
            query.Validate();
            if (!query.IsValid)
            {
                AddNotifications(query);
                var errors = GetErrorsFromNotifications(ErrorCodes.ERROR_INVALID_GET_CUSTOMERS_BY_NAME_QUERY);
                return new GetCustomersByNameQueryResult(errors);
            }

            IEnumerable<Customer> customers;
            bool isQueryPaginated = query.PageSize != 0 && query.CurrentPage != 0;

            if (isQueryPaginated)
                customers = await _customerRepository.ReadAllByNamePaginatedAsync(query.CustomerName!, query.CurrentPage, query.PageSize, query.IncludeInactives);
            else
                customers = await _customerRepository.ReadAllByNameAsync(query.CustomerName!, query.IncludeInactives);

            HashSet<CustomerDTO> customersDTO = new();

            foreach (var customer in customers)
            {
                customersDTO.Add(customer);
            }

            GetCustomersByNameQueryResult result = new()
            {
                Customers = customersDTO
            };

            if (isQueryPaginated)
                result.CustomersCount = await _customerRepository.CountAllByNameAsync(query.CustomerName!, query.IncludeInactives);
            else
                result.CustomersCount = customersDTO.Count;

            return result;
        }
    }
}
