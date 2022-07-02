using KadoshDomain.Commands.Base;
using KadoshDomain.Entities;
using KadoshDomain.Repositories;
using KadoshShared.Commands;
using KadoshShared.Constants.CommandMessages;
using KadoshShared.Constants.ErrorCodes;
using KadoshShared.Repositories;

namespace KadoshDomain.Commands.UserCommands.AuthenticateUser
{
    public class AuthenticateUserHandler : CommandHandlerBase<AuthenticateUserCommand>
    {
        private readonly IUserRepository _repository;

        public AuthenticateUserHandler(IUserRepository repository)
        {
            _repository = repository;
        }

        public override async Task<ICommandResult> HandleAsync(AuthenticateUserCommand command)
        {
            try
            {
                // Fail Fast Validations
                command.Validate();
                if (!command.IsValid)
                {
                    AddNotifications(command);
                    var errors = GetErrorsFromNotifications(ErrorCodes.ERROR_INVALID_USER_AUTHENTICATE_COMMAND);
                    return new CommandResult(false, UserCommandMessages.INVALID_USER_AUTHENTICATE_COMMAND, errors);
                }

                // Get user by username
                User? user = await _repository.GetUserByUsername(command.Username ?? string.Empty);

                // Entity validations
                if (user is null)
                {
                    AddNotification(nameof(user), UserCommandMessages.ERROR_USERNAME_NOT_FOUND);
                    var errors = GetErrorsFromNotifications(ErrorCodes.ERROR_USERNAME_NOT_FOUND);
                    return new CommandResult(false, UserCommandMessages.ERROR_USERNAME_NOT_FOUND, errors);
                }

                // Check validations
                if (!IsValid)
                {
                    var errors = GetErrorsFromNotifications(ErrorCodes.ERROR_INVALID_USER_AUTHENTICATE_COMMAND);
                    return new CommandResult(false, UserCommandMessages.INVALID_USER_AUTHENTICATE_COMMAND, errors);
                }

                // Hash password
                string passwordHash = _repository.GetPasswordHashed(command.Password, user.PasswordSalt, user.PasswordSaltIterations);

                // Compare passwords
                if (passwordHash != user.PasswordHash)
                {
                    AddNotification(nameof(command.Username), UserCommandMessages.ERROR_AUTHENTICATION_FAILED);
                    var errors = GetErrorsFromNotifications(ErrorCodes.ERROR_AUTHENTICATION_FAILED);
                    return new CommandResult(false, UserCommandMessages.ERROR_AUTHENTICATION_FAILED, errors);
                }

                return new CommandResult(true, UserCommandMessages.SUCCESS_ON_AUTHENTICATE_USER_COMMAND);

            }
            catch
            {
                var errors = GetErrorsFromNotifications(ErrorCodes.UNEXPECTED_EXCEPTION);
                return new CommandResult(false, SaleCommandMessages.UNEXPECTED_EXCEPTION, errors);
            }
        }
    }
}
