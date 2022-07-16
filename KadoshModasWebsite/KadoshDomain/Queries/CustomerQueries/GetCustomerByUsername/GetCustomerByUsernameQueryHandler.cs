using KadoshDomain.Queries.Base;
using KadoshDomain.Repositories;
using KadoshShared.Constants.ErrorCodes;
using KadoshShared.Constants.QueriesMessages;

namespace KadoshDomain.Queries.CustomerQueries.GetCustomerByUsername
{
    public class GetCustomerByUsernameQueryHandler : QueryHandlerBase<GetCustomerByUsernameQuery, GetCustomerByUsernameQueryResult>
    {
        private readonly ICustomerRepository _customerRepository;

        public GetCustomerByUsernameQueryHandler(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public override async Task<GetCustomerByUsernameQueryResult> HandleAsync(GetCustomerByUsernameQuery query)
        {
            // Fail Fast Validations
            query.Validate();
            if (!query.IsValid)
            {
                AddNotifications(query);
                var errors = GetErrorsFromNotifications(ErrorCodes.ERROR_INVALID_GET_CUSTOMER_BY_USERNAME_QUERY);
                return new GetCustomerByUsernameQueryResult(errors);
            }

            var customer = await _customerRepository.GetCustomerByUsernameAsync(query.Username!);

            if (customer is null)
            {
                AddNotification(nameof(customer), CustomerQueriesMessages.ERROR_CUSTOMER_USER_NOT_FOUND);
                var errors = GetErrorsFromNotifications(ErrorCodes.ERROR_CUSTOMER_USERNAME_NOT_FOUND);
                return new GetCustomerByUsernameQueryResult(errors);
            }

            GetCustomerByUsernameQueryResult result = new()
            {
                Customer = customer
            };

            return result;
        }
    }
}
