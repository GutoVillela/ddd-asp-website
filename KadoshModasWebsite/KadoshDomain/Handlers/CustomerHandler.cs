using Flunt.Notifications;
using KadoshDomain.Commands;
using KadoshDomain.Entities;
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
    public class CustomerHandler : Notifiable<Notification>, IHandler<CreateCustomerCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICustomerRepository _repository;

        public CustomerHandler(IUnitOfWork unitOfWork, ICustomerRepository repository)
        {
            _unitOfWork = unitOfWork;
            _repository = repository;
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
