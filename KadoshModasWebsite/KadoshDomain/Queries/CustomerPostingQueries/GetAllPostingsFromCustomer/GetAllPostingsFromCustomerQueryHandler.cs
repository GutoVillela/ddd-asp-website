﻿using KadoshDomain.Queries.Base;
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

        public override async Task<GetAllPostingsFromCustomerQueryResult> HandleAsync(GetAllPostingsFromCustomerQuery command)
        {
            // Fail Fast Validations
            command.Validate();
            if (!command.IsValid)
            {
                AddNotifications(command);
                var errors = GetErrorsFromNotifications(ErrorCodes.ERROR_INVALID_GET_ALL_POSTINGS_FROM_CUSTOMER_QUERY);
                return new GetAllPostingsFromCustomerQueryResult(errors);
            }

            var customerPostings = await _customerPostingRepository.ReadAllPostingsFromCustomerAsync(command.CustomerId!.Value);
            List<CustomerPostingDTO> customerPostingDTOs = new();  

            foreach(var posting in customerPostings!)
            {
                customerPostingDTOs.Add(posting);
            }

            GetAllPostingsFromCustomerQueryResult result = new()
            {
                CustomerPostings = customerPostingDTOs
            };

            return result;
        }
    }
}
