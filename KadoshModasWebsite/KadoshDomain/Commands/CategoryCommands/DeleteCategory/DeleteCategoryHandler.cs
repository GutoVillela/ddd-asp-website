using KadoshDomain.Commands.Base;
using KadoshDomain.Entities;
using KadoshDomain.Repositories;
using KadoshShared.Commands;
using KadoshShared.Constants.CommandMessages;
using KadoshShared.Constants.ErrorCodes;
using KadoshShared.Repositories;

namespace KadoshDomain.Commands.CategoryCommands.DeleteCategory
{
    public class DeleteCategoryHandler : CommandHandlerBase<DeleteCategoryCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICategoryRepository _repository;

        public DeleteCategoryHandler(IUnitOfWork unitOfWork, ICategoryRepository repository)
        {
            _unitOfWork = unitOfWork;
            _repository = repository;
        }

        public override async Task<ICommandResult> HandleAsync(DeleteCategoryCommand command)
        {
            try
            {
                // Fail Fast Validation
                command.Validate();
                if (!command.IsValid)
                {
                    AddNotifications(command);
                    var errors = GetErrorsFromNotifications(ErrorCodes.ERROR_INVALID_CATEGORY_DELETE_COMMAND);
                    return new CommandResult(false, CategoryCommandMessages.INVALID_CATEGORY_DELETE_COMMAND, errors);
                }

                // Get Entity
                Category? category = await _repository.ReadAsync(command.Id ?? 0);

                // Entity validations
                if (category is null)
                {
                    AddNotification(nameof(command.Id), CategoryCommandMessages.ERROR_CATEGORY_NOT_FOUND);
                    var errors = GetErrorsFromNotifications(ErrorCodes.ERROR_CATEGORY_NOT_FOUND);
                    return new CommandResult(false, CategoryCommandMessages.ERROR_CATEGORY_NOT_FOUND, errors);
                }

                // Check validations
                if (!IsValid)
                {
                    var errors = GetErrorsFromNotifications(ErrorCodes.ERROR_INVALID_CATEGORY_DELETE_COMMAND);
                    return new CommandResult(false, CategoryCommandMessages.INVALID_CATEGORY_DELETE_COMMAND, errors);
                }

                // Inactivate data (instead of deleting)
                category.Inactivate();
                await _repository.UpdateAsync(category);

                // Commit changes
                await _unitOfWork.CommitAsync();

                return new CommandResult(true, CategoryCommandMessages.SUCCESS_ON_DELETE_CATEGORY_COMMAND);

            }
            catch
            {
                var errors = GetErrorsFromNotifications(ErrorCodes.UNEXPECTED_EXCEPTION);
                return new CommandResult(false, SaleCommandMessages.UNEXPECTED_EXCEPTION, errors);
            }
        }
    }
}
