using Flunt.Notifications;
using KadoshDomain.Commands;
using KadoshDomain.Entities;
using KadoshDomain.Repositories;
using KadoshDomain.ValueObjects;
using KadoshShared.Commands;
using KadoshShared.Constants.CommandMessages;
using KadoshShared.Constants.ErrorCodes;
using KadoshShared.Handlers;
using KadoshShared.ValueObjects;

namespace KadoshDomain.Handlers
{
    public class UserHandler : Notifiable<Notification>, IHandler<CreateUserCommand>, IHandler<UpdateUserCommand>, IHandler<DeleteUserCommand>, IHandler<AuthenticateUserCommand>
    {
        private readonly IUserRepository _repository;

        public UserHandler(IUserRepository repository)
        {
            _repository = repository;
        }

        public async Task<ICommandResult> HandleAsync(CreateUserCommand command)
        {
            // Fail Fast Validations
            command.Validate();
            if(!command.IsValid)
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

            return new CommandResult(true, UserCommandMessages.SUCCESS_ON_CREATE_USER_COMMAND);
        }

        public async Task<ICommandResult> HandleAsync(UpdateUserCommand command)
        {
            // Fail Fast Validations
            command.Validate();
            if (!command.IsValid)
            {
                AddNotifications(command);
                var errors = GetErrorsFromNotifications(ErrorCodes.ERROR_INVALID_USER_UPDATE_COMMAND);
                return new CommandResult(false, UserCommandMessages.INVALID_USER_UPDATE_COMMAND, errors);

            }

            // Verify if username is already taken
            var usernameTaken = await _repository.VerifyIfUsernameIsTakenExceptForGivenOneAsync(command.NewUsername, command.OriginalUsername);

            if (usernameTaken)
            {
                AddNotification(nameof(command.NewUsername), UserCommandMessages.ERROR_USERNAME_ALREADY_TAKEN);
                var errors = GetErrorsFromNotifications(ErrorCodes.ERROR_USERNAME_ALREADY_TAKEN);
                return new CommandResult(false, UserCommandMessages.ERROR_USERNAME_ALREADY_TAKEN, errors);
            }

            // Get Entity to Update
            User? user = await _repository.ReadAsync(command.Id.Value);

            if (user is null)
            {
                AddNotification(nameof(command.OriginalUsername), UserCommandMessages.ERROR_USERNAME_NOT_FOUND);
                var errors = GetErrorsFromNotifications(ErrorCodes.ERROR_USERNAME_NOT_FOUND);
                return new CommandResult(false, UserCommandMessages.ERROR_USERNAME_NOT_FOUND, errors);
            }

            // Get password encrypted
            (string passwordHash, byte[] salt, int iterations) = _repository.GetPasswordHashed(command.Password);

            user.UpdateUserInfo(
                name: command.Name,
                username: command.NewUsername,
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
                var errors = GetErrorsFromNotifications(ErrorCodes.ERROR_INVALID_USER_UPDATE_COMMAND);
                return new CommandResult(false, UserCommandMessages.INVALID_USER_UPDATE_COMMAND, errors);
            }

            // Create user
            await _repository.UpdateAsync(user);

            return new CommandResult(true, UserCommandMessages.SUCCESS_ON_UPDATE_USER_COMMAND);
        }

        public async Task<ICommandResult> HandleAsync(DeleteUserCommand command)
        {
            // Fail Fast Validation
            command.Validate();
            if (!command.IsValid)
            {
                AddNotifications(command);
                var errors = GetErrorsFromNotifications(ErrorCodes.ERROR_INVALID_USER_DELETE_COMMAND);
                return new CommandResult(false, UserCommandMessages.INVALID_USER_DELETE_COMMAND, errors);
            }

            // Get Entity
            User user = await _repository.ReadAsync(command.Id ?? 0);

            // Entity validations
            if (user is null)
                AddNotification(nameof(user), UserCommandMessages.ERROR_USERNAME_NOT_FOUND);

            // Check validations
            if (!IsValid)
            {
                var errors = GetErrorsFromNotifications(ErrorCodes.ERROR_INVALID_USER_DELETE_COMMAND);
                return new CommandResult(false, UserCommandMessages.INVALID_USER_DELETE_COMMAND, errors);
            }

            // Persist data
            await _repository.DeleteAsync(user);

            return new CommandResult(true, UserCommandMessages.SUCCESS_ON_DELETE_USER_COMMAND);
        }

        public async Task<ICommandResult> HandleAsync(AuthenticateUserCommand command)
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
                AddNotification(nameof(user), UserCommandMessages.ERROR_USERNAME_NOT_FOUND);

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
            
            return new CommandResult(true, UserCommandMessages.SUCCESS_ON_DELETE_USER_COMMAND);
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
