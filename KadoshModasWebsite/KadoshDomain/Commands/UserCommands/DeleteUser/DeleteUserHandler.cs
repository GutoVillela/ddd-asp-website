using KadoshDomain.Commands.Base;
using KadoshDomain.Entities;
using KadoshDomain.Repositories;
using KadoshShared.Commands;
using KadoshShared.Constants.CommandMessages;
using KadoshShared.Constants.ErrorCodes;
using KadoshShared.Repositories;

namespace KadoshDomain.Commands.UserCommands.DeleteUser
{
    public class DeleteUserHandler : CommandHandlerBase<DeleteUserCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserRepository _repository;

        public DeleteUserHandler(IUnitOfWork unitOfWork, IUserRepository repository)
        {
            _unitOfWork = unitOfWork;
            _repository = repository;
        }

        public override async Task<ICommandResult> HandleAsync(DeleteUserCommand command)
        {
            try
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
                _repository.Delete(user);

                // Commit changes
                await _unitOfWork.CommitAsync();

                return new CommandResult(true, UserCommandMessages.SUCCESS_ON_DELETE_USER_COMMAND);
            }
            catch
            {
                var errors = GetErrorsFromNotifications(ErrorCodes.UNEXPECTED_EXCEPTION);
                return new CommandResult(false, SaleCommandMessages.UNEXPECTED_EXCEPTION, errors);
            }
        }
    }
}
