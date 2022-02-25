using Flunt.Notifications;
using KadoshDomain.Commands;
using KadoshDomain.Entities;
using KadoshDomain.Enums;
using KadoshDomain.Repositories;
using KadoshDomain.ValueObjects;
using KadoshShared.Commands;
using KadoshShared.Constants.CommandMessages;
using KadoshShared.Constants.ErrorCodes;
using KadoshShared.Handlers;
using KadoshShared.Repositories;
using KadoshShared.ValueObjects;

namespace KadoshDomain.Handlers
{
    public class CustomerHandler : Notifiable<Notification>, IHandler<CreateCustomerCommand>, IHandler<UpdateCustomerCommand>, IHandler<DeleteCustomerCommand>, IHandler<InformPaymentCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICustomerRepository _repository;
        private readonly ISaleRepository _saleRepository;
        private readonly ICustomerPostingRepository _customerPostingRepository;

        public CustomerHandler(IUnitOfWork unitOfWork, ICustomerRepository repository, ISaleRepository saleRepository, ICustomerPostingRepository customerPostingRepository)
        {
            _unitOfWork = unitOfWork;
            _repository = repository;
            _saleRepository = saleRepository;
            _customerPostingRepository = customerPostingRepository;
        }

        public async Task<ICommandResult> HandleAsync(CreateCustomerCommand command)
        {
            try
            {
                // Fail Fast Validations
                command.Validate();
                if (!command.IsValid)
                {
                    AddNotifications(command);
                    var errors = GetErrorsFromNotifications(ErrorCodes.ERROR_INVALID_CUSTOMER_CREATE_COMMAND);
                    return new CommandResult(false, CustomerCommandMessages.INVALID_CUSTOMER_CREATE_COMMAND, errors);

                }

                // Create Entity
                Email? email = null;
                Document? document = null;
                Address? address = null;

                if (!string.IsNullOrEmpty(command.EmailAddress))
                    email = new(command.EmailAddress);

                if (!string.IsNullOrEmpty(command.DocumentNumber))
                    document = new Document(command.DocumentNumber, command.DocumentType.Value);

                if (!string.IsNullOrEmpty(command.AddressStreet))
                    address = new(
                        command.AddressStreet,
                        command.AddressNumber ?? string.Empty,
                        command.AddressNeighborhood ?? string.Empty,
                        command.AddressCity ?? string.Empty,
                        command.AddressState ?? string.Empty,
                        command.AddressZipCode ?? string.Empty,
                        command.AddressComplement ?? string.Empty
                        );

                Customer customer = new(
                    name: command.Name,
                    email: email,
                    document: document,
                    gender: command.Gender,
                    address: address,
                    phones: command.Phones?.ToList()
                    );

                // Group validations
                AddNotifications(customer);

                if (email is not null)
                    AddNotifications(email);

                if (document is not null)
                    AddNotifications(document);

                if (address is not null)
                    AddNotifications(address);

                // Validate before register customer
                if (!IsValid)
                {
                    var errors = GetErrorsFromNotifications(ErrorCodes.ERROR_INVALID_CUSTOMER_CREATE_COMMAND);
                    return new CommandResult(false, CustomerCommandMessages.INVALID_CUSTOMER_CREATE_COMMAND, errors);
                }

                // Create register
                await _repository.CreateAsync(customer);

                // Commit changes
                await _unitOfWork.CommitAsync();

                return new CommandResult(true, CustomerCommandMessages.SUCCESS_ON_CREATE_CUSTOMER_COMMAND);

            }
            catch
            {
                var errors = GetErrorsFromNotifications(ErrorCodes.UNEXPECTED_EXCEPTION);
                return new CommandResult(false, SaleCommandMessages.UNEXPECTED_EXCEPTION, errors);
            }
        }

        public async Task<ICommandResult> HandleAsync(UpdateCustomerCommand command)
        {
            try
            {
                // Fail Fast Validations
                command.Validate();
                if (!command.IsValid)
                {
                    AddNotifications(command);
                    var errors = GetErrorsFromNotifications(ErrorCodes.ERROR_INVALID_CUSTOMER_UPDATE_COMMAND);
                    return new CommandResult(false, CustomerCommandMessages.INVALID_CUSTOMER_UPDATE_COMMAND, errors);

                }

                // Create Entity
                Email? email = null;
                Document? document = null;
                Address? address = null;

                if (!string.IsNullOrEmpty(command.EmailAddress))
                    email = new(command.EmailAddress);

                if (!string.IsNullOrEmpty(command.DocumentNumber))
                    document = new Document(command.DocumentNumber, command.DocumentType.Value);

                if (!string.IsNullOrEmpty(command.AddressStreet))
                    address = new(
                        command.AddressStreet,
                        command.AddressNumber ?? string.Empty,
                        command.AddressNeighborhood ?? string.Empty,
                        command.AddressCity ?? string.Empty,
                        command.AddressState ?? string.Empty,
                        command.AddressZipCode ?? string.Empty,
                        command.AddressComplement ?? string.Empty
                        );


                // Recover entity to update its values
                var customer = await _repository.ReadAsync(command.Id.Value);

                if (customer is null)
                {
                    AddNotification(nameof(command.Id), CustomerCommandMessages.ERROR_CUSTOMER_NOT_FOUND);
                    var errors = GetErrorsFromNotifications(ErrorCodes.ERROR_CUSTOMER_NOT_FOUND);
                    return new CommandResult(false, CustomerCommandMessages.ERROR_CUSTOMER_NOT_FOUND, errors);
                }

                customer.UpdateCustomerInfo(
                    name: command.Name,
                    email: email,
                    document: document,
                    gender: command.Gender,
                    address: address,
                    phones: command.Phones?.ToList()
                    );

                customer.SetLastUpdateDate(DateTime.UtcNow);

                // Group validations
                AddNotifications(customer);

                if (email is not null)
                    AddNotifications(email);

                if (document is not null)
                    AddNotifications(document);

                if (address is not null)
                    AddNotifications(address);

                // Validate before register customer
                if (!IsValid)
                {
                    var errors = GetErrorsFromNotifications(ErrorCodes.ERROR_INVALID_CUSTOMER_UPDATE_COMMAND);
                    return new CommandResult(false, CustomerCommandMessages.INVALID_CUSTOMER_UPDATE_COMMAND, errors);
                }

                // Update entity
                await _repository.UpdateAsync(customer);

                // Commit changes
                await _unitOfWork.CommitAsync();

                return new CommandResult(true, CustomerCommandMessages.SUCCESS_ON_CUSTOMER_UPDATE_COMMAND);

            }
            catch
            {
                var errors = GetErrorsFromNotifications(ErrorCodes.UNEXPECTED_EXCEPTION);
                return new CommandResult(false, SaleCommandMessages.UNEXPECTED_EXCEPTION, errors);
            }
        }

        public async Task<ICommandResult> HandleAsync(DeleteCustomerCommand command)
        {
            try
            {
                // Fail Fast Validation
                command.Validate();
                if (!command.IsValid)
                {
                    AddNotifications(command);
                    var errors = GetErrorsFromNotifications(ErrorCodes.ERROR_INVALID_CUSTOMER_DELETE_COMMAND);
                    return new CommandResult(false, CustomerCommandMessages.INVALID_CUSTOMER_DELETE_COMMAND, errors);
                }

                // Get Entity
                Customer customer = await _repository.ReadAsync(command.Id ?? 0);

                // Entity validations
                if (customer is null)
                    AddNotification(nameof(customer), CustomerCommandMessages.ERROR_CUSTOMER_NOT_FOUND);

                // Check validations
                if (!IsValid)
                {
                    var errors = GetErrorsFromNotifications(ErrorCodes.ERROR_INVALID_CUSTOMER_DELETE_COMMAND);
                    return new CommandResult(false, CustomerCommandMessages.INVALID_CUSTOMER_DELETE_COMMAND, errors);
                }

                // Persist data
                _repository.Delete(customer);

                // Commit changes
                await _unitOfWork.CommitAsync();

                return new CommandResult(true, CustomerCommandMessages.SUCCESS_ON_CUSTOMER_DELETE_COMMAND);

            }
            catch
            {
                var errors = GetErrorsFromNotifications(ErrorCodes.UNEXPECTED_EXCEPTION);
                return new CommandResult(false, SaleCommandMessages.UNEXPECTED_EXCEPTION, errors);
            }
        }

        public async Task<ICommandResult> HandleAsync(InformPaymentCommand command)
        {
            try
            {
                // Fail Fast Validations
                command.Validate();
                if (!command.IsValid)
                {
                    AddNotifications(command);
                    var errors = GetErrorsFromNotifications(ErrorCodes.ERROR_INVALID_CUSTOMER_INFORM_PAYMENT_COMMAND);
                    return new CommandResult(false, CustomerCommandMessages.INVALID_CUSTOMER_INFORM_PAYMENT_COMMAND, errors);

                }

                // Validate customer
                Customer? customer = await _repository.ReadAsNoTrackingAsync(command.CustomerId!.Value);

                if (customer is null)
                {
                    AddNotification(nameof(customer), CustomerCommandMessages.ERROR_CUSTOMER_NOT_FOUND);
                    var errors = GetErrorsFromNotifications(ErrorCodes.ERROR_CUSTOMER_NOT_FOUND);
                    return new CommandResult(false, CustomerCommandMessages.ERROR_CUSTOMER_NOT_FOUND, errors);
                }

                // Get all opened sales
                var customersOpenSales = await _saleRepository.ReadAllOpenFromCustomerAsync(customer.Id);

                if (customersOpenSales is null || !customersOpenSales.Any()) 
                {
                    AddNotification(nameof(customersOpenSales), CustomerCommandMessages.ERROR_NO_OPEN_SALES_FOUND_TO_CUSTOMER);
                    var errors = GetErrorsFromNotifications(ErrorCodes.ERROR_NO_OPEN_SALES_FOUND_TO_CUSTOMER);
                    return new CommandResult(false, CustomerCommandMessages.ERROR_NO_OPEN_SALES_FOUND_TO_CUSTOMER, errors);
                }

                decimal amountRemainingToInform = command.AmountToInform!.Value;

                foreach (var sale in customersOpenSales)
                {
                    if (amountRemainingToInform == 0)
                        break;

                    decimal valueToInform = 0;
                    bool payoffSale = false;

                    if(amountRemainingToInform >= sale.TotalToPay)
                    {
                        valueToInform = sale.TotalToPay;
                        amountRemainingToInform -= valueToInform;
                        payoffSale = true;
                    }
                    else
                    {
                        valueToInform = amountRemainingToInform;
                        amountRemainingToInform = 0;
                    }

                    //CustomerPosting customerPosting = new(
                    //    type: ECustomerPostingType.Payment,
                    //    value: valueToInform,
                    //    saleId: sale.Id,
                    //    postingDate: DateTime.UtcNow
                    //);
                    CustomerPosting customerPosting = new(
                        type: ECustomerPostingType.Payment,
                        value: valueToInform,
                        sale: sale,
                        postingDate: DateTime.UtcNow
                    );


                    // Entity validations
                    AddNotifications(customerPosting);

                    // Check validations
                    if (!IsValid)
                    {
                        var errors = GetErrorsFromNotifications(ErrorCodes.ERROR_ONE_OR_MORE_INVALID_POSTINGS_TO_INFORMING_PAYMENT);
                        return new CommandResult(false, CustomerCommandMessages.ERROR_ONE_OR_MORE_INVALID_POSTINGS_TO_INFORMING_PAYMENT, errors);
                    }

                    await _customerPostingRepository.CreateAsync(customerPosting);

                    if (payoffSale)
                    {
                        sale.SetSituation(ESaleSituation.Completed);
                        sale.SetLastUpdateDate(DateTime.UtcNow);
                        await _saleRepository.UpdateAsync(sale);
                    }
                }

                // Commit changes
                await _unitOfWork.CommitAsync();

                if(amountRemainingToInform > 0)
                {
                    string valueInformed = (command.AmountToInform!.Value - amountRemainingToInform).ToString("C");
                    return new CommandResult(true, string.Format(CustomerCommandMessages.SUCCESS_ON_INFORM_PAYMENT_COMMAND_WITH_RESERVATIONS, command.AmountToInform!.Value.ToString("C"), valueInformed));
                }
                else
                    return new CommandResult(true, CustomerCommandMessages.SUCCESS_ON_INFORM_PAYMENT_COMMAND);

            }
            catch (Exception ex)
            {
                var errors = GetErrorsFromNotifications(ErrorCodes.UNEXPECTED_EXCEPTION);
                return new CommandResult(false, SaleCommandMessages.UNEXPECTED_EXCEPTION + $" {ex.Message}", errors);
            }
        }

        private ICollection<Error> GetErrorsFromNotifications(int errorCode)
        {
            HashSet<Error> errors = new();
            foreach (var error in Notifications)
            {
                errors.Add(new Error(errorCode, error.Message));
            }

            return errors;
        }
    }
}
