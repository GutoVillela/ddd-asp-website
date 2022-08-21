using KadoshDomain.Commands.Base;
using KadoshDomain.Entities;
using KadoshDomain.Enums;
using KadoshDomain.Repositories;
using KadoshShared.Commands;
using KadoshShared.Constants.CommandMessages;
using KadoshShared.Constants.ErrorCodes;
using KadoshShared.Repositories;

namespace KadoshDomain.Commands.CustomerCommands.MergeCustomer
{
    public class MergeCustomerHandler : CommandHandlerBase<MergeCustomerCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICustomerRepository _repository;
        private readonly ISaleRepository _saleRepository;

        public MergeCustomerHandler(IUnitOfWork unitOfWork, ICustomerRepository repository, ISaleRepository saleRepository)
        {
            _unitOfWork = unitOfWork;
            _repository = repository;
            _saleRepository = saleRepository;
        }

        public override async Task<ICommandResult> HandleAsync(MergeCustomerCommand command)
        {
            try
            {
                // Fail Fast Validations
                command.Validate();
                if (!command.IsValid)
                {
                    AddNotifications(command);
                    var errors = GetErrorsFromNotifications(ErrorCodes.ERROR_INVALID_MERGE_CUSTOMERS_COMMAND);
                    return new CommandResult(false, CustomerCommandMessages.INVALID_MERGE_CUSTOMERS_COMMAND, errors);

                }

                // Validate main customer
                Customer? mainCustomer = await _repository.ReadAsync(command.MainCustomerId);

                if (mainCustomer is null)
                {
                    AddNotification(nameof(mainCustomer), CustomerCommandMessages.ERROR_CUSTOMER_NOT_FOUND);
                    var errors = GetErrorsFromNotifications(ErrorCodes.ERROR_CUSTOMER_NOT_FOUND);
                    return new CommandResult(false, CustomerCommandMessages.ERROR_CUSTOMER_NOT_FOUND, errors);
                }

                // Validate customers to merge and apply merge
                foreach(var customerId in command.CustomersToMerge)
                {
                    Customer? customer = await _repository.ReadAsync(customerId);

                    if (customer is null)
                    {
                        AddNotification(nameof(mainCustomer), CustomerCommandMessages.ERROR_CUSTOMER_NOT_FOUND);
                        var errors = GetErrorsFromNotifications(ErrorCodes.ERROR_CUSTOMER_NOT_FOUND);
                        return new CommandResult(false, CustomerCommandMessages.ERROR_CUSTOMER_NOT_FOUND, errors);
                    }

                    // Validate if customer is already merged
                    if(customer.BoundedToCustomerId is not null)
                    {
                        AddNotification(nameof(customer), CustomerCommandMessages.ERROR_CUSTOMER_ALREADY_BOUNDED);
                        var errors = GetErrorsFromNotifications(ErrorCodes.ERROR_CUSTOMER_ALREADY_BOUNDED);
                        return new CommandResult(false, CustomerCommandMessages.ERROR_CUSTOMER_ALREADY_BOUNDED, errors);
                    }

                    // Validate if customer has merged customers
                    if(customer.BoundedCustomers is not null && customer.BoundedCustomers.Any()) 
                    {
                        AddNotification(nameof(customer), string.Format(CustomerCommandMessages.ERROR_CUSTOMER_HAS_BOUNDED_CUSTOMERS, customer.Name, customer.Id));
                        var errors = GetErrorsFromNotifications(ErrorCodes.ERROR_CUSTOMER_HAS_BOUNDED_CUSTOMERS);
                        return new CommandResult(false, string.Format(CustomerCommandMessages.ERROR_CUSTOMER_HAS_BOUNDED_CUSTOMERS, customer.Name, customer.Id), errors);
                    }

                    var customerSales = await _saleRepository.ReadAllFromCustomerAsync(customer.Id);
                    customer.MergeToCustomer(mainCustomer);
                    customer.SetLastUpdateDate(DateTime.UtcNow);
                    
                }
                // Commit changes
                await _unitOfWork.CommitAsync();

                return new CommandResult(true, CustomerCommandMessages.SUCCESS_ON_MERGE_CUSTOMER_COMMAND);

            }
            catch (Exception ex)
            {
                var errors = GetErrorsFromNotifications(ErrorCodes.UNEXPECTED_EXCEPTION);
                return new CommandResult(false, SaleCommandMessages.UNEXPECTED_EXCEPTION + $" {ex.Message}", errors);
            }
        }
    }
}

