using KadoshDomain.Commands.Base;
using KadoshDomain.Entities;
using KadoshDomain.Repositories;
using KadoshShared.Commands;
using KadoshShared.Constants.CommandMessages;
using KadoshShared.Constants.ErrorCodes;

namespace KadoshDomain.Commands.CustomerCommands.AuthenticateCustomerUser
{
    public class AuthenticateCustomerUserHandler : CommandHandlerBase<AuthenticateCustomerUserCommand>
    {
        private readonly ICustomerRepository _repository;

        public AuthenticateCustomerUserHandler(ICustomerRepository repository)
        {
            _repository = repository;
        }

        public override async Task<ICommandResult> HandleAsync(AuthenticateCustomerUserCommand command)
        {
            try
            {
                // Fail Fast Validations
                command.Validate();
                if (!command.IsValid)
                {
                    AddNotifications(command);
                    var errors = GetErrorsFromNotifications(ErrorCodes.ERROR_INVALID_CUSTOMER_AUTHENTICATE_COMMAND);
                    return new CommandResult(false, CustomerCommandMessages.INVALID_CUSTOMER_AUTHENTICATE_COMMAND, errors);
                }

                // Get customer by username
                Customer? customer = await _repository.GetCustomerByUsernameAsync(command.Username ?? string.Empty);

                // Entity validations
                if (customer is null)
                {
                    AddNotification(nameof(customer), CustomerCommandMessages.ERROR_CUSTOMER_USERNAME_NOT_FOUND);
                    var errors = GetErrorsFromNotifications(ErrorCodes.ERROR_CUSTOMER_USERNAME_NOT_FOUND);
                    return new CommandResult(false, CustomerCommandMessages.ERROR_CUSTOMER_USERNAME_NOT_FOUND, errors);
                }

                // Check validations
                if (!IsValid)
                {
                    var errors = GetErrorsFromNotifications(ErrorCodes.ERROR_INVALID_CUSTOMER_AUTHENTICATE_COMMAND);
                    return new CommandResult(false, CustomerCommandMessages.INVALID_CUSTOMER_AUTHENTICATE_COMMAND, errors);
                }

                // Check if customer password is correctly set
                if(customer.PasswordHash == null || customer.PasswordSalt == null || customer.PasswordSaltIterations == null)
                {
                    AddNotification(nameof(customer), CustomerCommandMessages.ERROR_CUSTOMER_USER_PASSWORD_NOT_SET);
                    var errors = GetErrorsFromNotifications(ErrorCodes.ERROR_INVALID_CUSTOMER_AUTHENTICATE_COMMAND);
                    return new CommandResult(false, CustomerCommandMessages.INVALID_CUSTOMER_AUTHENTICATE_COMMAND, errors);
                }

                // Hash password
                string passwordHash = _repository.GetPasswordHashed(command.Password!, customer.PasswordSalt, customer.PasswordSaltIterations!.Value);

                // Compare passwords
                if (passwordHash != customer.PasswordHash)
                {
                    AddNotification(nameof(command.Username), CustomerCommandMessages.ERROR_AUTHENTICATION_FAILED);
                    var errors = GetErrorsFromNotifications(ErrorCodes.ERROR_AUTHENTICATION_FAILED);
                    return new CommandResult(false, CustomerCommandMessages.ERROR_AUTHENTICATION_FAILED, errors);
                }

                return new CommandResult(true, CustomerCommandMessages.SUCCESS_ON_AUTHENTICATE_CUSTOMER_USER_COMMAND);

            }
            catch
            {
                var errors = GetErrorsFromNotifications(ErrorCodes.UNEXPECTED_EXCEPTION);
                return new CommandResult(false, SaleCommandMessages.UNEXPECTED_EXCEPTION, errors);
            }
        }
    }
}
