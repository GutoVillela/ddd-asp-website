using KadoshDomain.Commands.Base;
using KadoshDomain.Entities;
using KadoshDomain.Repositories;
using KadoshDomain.ValueObjects;
using KadoshShared.Commands;
using KadoshShared.Constants.CommandMessages;
using KadoshShared.Constants.ErrorCodes;
using KadoshShared.Repositories;

namespace KadoshDomain.Commands.CustomerCommands.CreateCustomer
{
    public class CreateCustomerHandler : CommandHandlerBase<CreateCustomerCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICustomerRepository _repository;

        public CreateCustomerHandler(IUnitOfWork unitOfWork, ICustomerRepository repository)
        {
            _unitOfWork = unitOfWork;
            _repository = repository;
        }

        public override async Task<ICommandResult> HandleAsync(CreateCustomerCommand command)
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
    }
}
