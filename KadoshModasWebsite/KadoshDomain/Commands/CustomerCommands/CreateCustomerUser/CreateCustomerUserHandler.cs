using KadoshDomain.Commands.Base;
using KadoshDomain.Repositories;
using KadoshShared.Commands;
using KadoshShared.Constants.CommandMessages;
using KadoshShared.Constants.ErrorCodes;
using KadoshShared.Repositories;

namespace KadoshDomain.Commands.CustomerCommands.CreateCustomerUser
{
    public class CreateCustomerUserHandler : CommandHandlerBase<CreateCustomerUserCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICustomerRepository _repository;

        public CreateCustomerUserHandler(IUnitOfWork unitOfWork, ICustomerRepository repository)
        {
            _unitOfWork = unitOfWork;
            _repository = repository;
        }

        public async override Task<ICommandResult> HandleAsync(CreateCustomerUserCommand command)
        {
            try
            {
                // Fail Fast Validations
                command.Validate();
                if (!command.IsValid)
                {
                    AddNotifications(command);
                    var errors = GetErrorsFromNotifications(ErrorCodes.ERROR_INVALID_CUSTOMER_USER_CREATE_COMMAND);
                    return new CommandResult(false, CustomerCommandMessages.INVALID_CUSTOMER_USER_CREATE_COMMAND, errors);

                }

                // Recover entity to update its values
                var customer = await _repository.ReadAsync(command.CustomerId.Value);

                if (customer is null)
                {
                    AddNotification(nameof(command.CustomerId), CustomerCommandMessages.ERROR_CUSTOMER_NOT_FOUND);
                    var errors = GetErrorsFromNotifications(ErrorCodes.ERROR_CUSTOMER_NOT_FOUND);
                    return new CommandResult(false, CustomerCommandMessages.ERROR_CUSTOMER_NOT_FOUND, errors);
                }

                // Verify it there's already an user associated to the given customer
                if (!string.IsNullOrEmpty(customer.Username))
                {
                    AddNotification(nameof(command.CustomerId), CustomerCommandMessages.ERROR_CUSTOMER_USER_ALREADY_CREATED);
                    var errors = GetErrorsFromNotifications(ErrorCodes.ERROR_CUSTOMER_USER_ALREADY_CREATED);
                    return new CommandResult(false, CustomerCommandMessages.ERROR_CUSTOMER_USER_ALREADY_CREATED, errors);
                }

                // Verify if username is already taken
                var usernameTaken = await _repository.VerifyIfUsernameIsTakenAsync(command.Username);

                if (usernameTaken)
                {
                    AddNotification(nameof(command.Username), CustomerCommandMessages.ERROR_USERNAME_ALREADY_TAKEN);
                    var errors = GetErrorsFromNotifications(ErrorCodes.ERROR_USERNAME_ALREADY_TAKEN);
                    return new CommandResult(false, CustomerCommandMessages.ERROR_USERNAME_ALREADY_TAKEN, errors);
                }

                // Get password encrypted
                (string passwordHash, byte[] salt, int iterations) = _repository.GetPasswordHashed(command.Password);

                // Set user information
                customer.SetUsernameAndPassword(command.Username, passwordHash, salt, iterations);

                // Group validations
                AddNotifications(customer);

                // Validate before register customer
                if (!IsValid)
                {
                    var errors = GetErrorsFromNotifications(ErrorCodes.ERROR_INVALID_CUSTOMER_USER_CREATE_COMMAND);
                    return new CommandResult(false, CustomerCommandMessages.INVALID_CUSTOMER_USER_CREATE_COMMAND, errors);
                }

                // Create user
                await _repository.UpdateAsync(customer);

                // Commit changes
                await _unitOfWork.CommitAsync();

                return new CommandResult(true, CustomerCommandMessages.SUCCESS_ON_CREATE_CUSTOMER_USER_COMMAND);
            }
            catch
            {
                var errors = GetErrorsFromNotifications(ErrorCodes.UNEXPECTED_EXCEPTION);
                return new CommandResult(false, SaleCommandMessages.UNEXPECTED_EXCEPTION, errors);
            }
        }
    }
}
