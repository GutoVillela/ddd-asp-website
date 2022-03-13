using KadoshDomain.Commands.Base;
using KadoshDomain.Entities;
using KadoshDomain.Repositories;
using KadoshShared.Commands;
using KadoshShared.Constants.CommandMessages;
using KadoshShared.Constants.ErrorCodes;
using KadoshShared.Repositories;

namespace KadoshDomain.Commands.UserCommands.CreateUser
{
    public class CreateUserHandler : CommandHandlerBase<CreateUserCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserRepository _repository;

        public CreateUserHandler(IUnitOfWork unitOfWork, IUserRepository repository)
        {
            _unitOfWork = unitOfWork;
            _repository = repository;
        }

        public override async Task<ICommandResult> HandleAsync(CreateUserCommand command)
        {
            try
            {
                // Fail Fast Validations
                command.Validate();
                if (!command.IsValid)
                {
                    AddNotifications(command);
                    var errors = GetErrorsFromNotifications(ErrorCodes.ERROR_INVALID_USER_CREATE_COMMAND);
                    return new CommandResult(false, UserCommandMessages.INVALID_USER_CREATE_COMMAND, errors);

                }

                // Verify if username is already taken
                var usernameTaken = await _repository.VerifyIfUsernameIsTakenAsync(command.Username);

                if (usernameTaken)
                {
                    AddNotification(nameof(command.Username), UserCommandMessages.ERROR_USERNAME_ALREADY_TAKEN);
                    var errors = GetErrorsFromNotifications(ErrorCodes.ERROR_USERNAME_ALREADY_TAKEN);
                    return new CommandResult(false, UserCommandMessages.ERROR_USERNAME_ALREADY_TAKEN, errors);
                }

                // Get password encrypted
                (string passwordHash, byte[] salt, int iterations) = _repository.GetPasswordHashed(command.Password);

                // Create Entity
                User user = new(
                    name: command.Name,
                    username: command.Username,
                    passwordHash: passwordHash,
                    passwordSalt: salt,
                    passwordSaltIterations: iterations,
                    role: command.Role.Value,
                    storeId: command.StoreId.Value
                    );

                // Group validations
                AddNotifications(user);

                // Validate before register customer
                if (!IsValid)
                {
                    var errors = GetErrorsFromNotifications(ErrorCodes.ERROR_INVALID_USER_CREATE_COMMAND);
                    return new CommandResult(false, UserCommandMessages.INVALID_USER_CREATE_COMMAND, errors);
                }

                // Create user
                await _repository.CreateAsync(user);

                // Commit changes
                await _unitOfWork.CommitAsync();

                return new CommandResult(true, UserCommandMessages.SUCCESS_ON_CREATE_USER_COMMAND);
            }
            catch
            {
                var errors = GetErrorsFromNotifications(ErrorCodes.UNEXPECTED_EXCEPTION);
                return new CommandResult(false, SaleCommandMessages.UNEXPECTED_EXCEPTION, errors);
            }
        }
    }
}
