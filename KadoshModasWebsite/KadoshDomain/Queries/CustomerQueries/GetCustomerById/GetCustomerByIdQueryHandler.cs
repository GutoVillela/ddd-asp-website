﻿using KadoshDomain.Queries.Base;
using KadoshDomain.Repositories;
using KadoshShared.Constants.ErrorCodes;
using KadoshShared.Constants.QueriesMessages;

namespace KadoshDomain.Queries.CustomerQueries.GetCustomerById
{
    public class GetCustomerByIdQueryHandler : QueryHandlerBase<GetCustomerByIdQuery, GetCustomerByIdQueryResult>
    {
        private readonly ICustomerRepository _customerRepository;

        public GetCustomerByIdQueryHandler(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public override async Task<GetCustomerByIdQueryResult> HandleAsync(GetCustomerByIdQuery command)
        {
            // Fail Fast Validations
            command.Validate();
            if (!command.IsValid)
            {
                AddNotifications(command);
                var errors = GetErrorsFromNotifications(ErrorCodes.ERROR_INVALID_GET_CUSTOMER_BY_ID_QUERY);
                return new GetCustomerByIdQueryResult(errors);
            }

            var customer = await _customerRepository.ReadAsync(command.CustomerId!.Value);

            if (customer is null)
            {
                AddNotification(nameof(customer), CustomerQueriesMessages.ERROR_CUSTOMER_ID_NOT_FOUND);
                var errors = GetErrorsFromNotifications(ErrorCodes.ERROR_CUSTOMER_NOT_FOUND);
                return new GetCustomerByIdQueryResult(errors);
            }

            GetCustomerByIdQueryResult result = new()
            {
                Customer = customer
            };

            return result;
        }
    }
}
