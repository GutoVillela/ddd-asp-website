using KadoshDomain.Commands.Base;
using KadoshDomain.Entities;
using KadoshDomain.Repositories;
using KadoshShared.Commands;
using KadoshShared.Constants.CommandMessages;
using KadoshShared.Constants.ErrorCodes;
using KadoshShared.Repositories;

namespace KadoshDomain.Commands.CategoryCommands.UpdateCategory
{
    public class UpdateCategoryHandler : HandlerBase<UpdateCategoryCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICategoryRepository _repository;

        public UpdateCategoryHandler(IUnitOfWork unitOfWork, ICategoryRepository repository)
        {
            _unitOfWork = unitOfWork;
            _repository = repository;
        }

        public override async Task<ICommandResult> HandleAsync(UpdateCategoryCommand command)
        {
            try
            {
                // Fail Fast Validations
                command.Validate();
                if (!command.IsValid)
                {
                    AddNotifications(command);
                    var errors = GetErrorsFromNotifications(ErrorCodes.ERROR_INVALID_CATEGORY_UPDATE_COMMAND);
                    return new CommandResult(false, CategoryCommandMessages.INVALID_CATEGORY_UPDATE_COMMAND, errors);

                }

                // Recover entity to update its values
                Category category = await _repository.ReadAsync(command.Id.Value);

                if (category is null)
                {
                    AddNotification(nameof(command.Id), CategoryCommandMessages.ERROR_CATEGORY_NOT_FOUND);
                    var errors = GetErrorsFromNotifications(ErrorCodes.ERROR_CATEGORY_NOT_FOUND);
                    return new CommandResult(false, CategoryCommandMessages.ERROR_CATEGORY_NOT_FOUND, errors);
                }

                category.UpdateCategoryInfo(
                    name: command.Name
                    );

                category.SetLastUpdateDate(DateTime.UtcNow);

                // Group validations
                AddNotifications(category);

                // Validate before register customer
                if (!IsValid)
                {
                    var errors = GetErrorsFromNotifications(ErrorCodes.ERROR_INVALID_CATEGORY_UPDATE_COMMAND);
                    return new CommandResult(false, CategoryCommandMessages.INVALID_CATEGORY_UPDATE_COMMAND, errors);
                }

                // Update entity
                await _repository.UpdateAsync(category);

                // Commit changes
                await _unitOfWork.CommitAsync();

                return new CommandResult(true, CategoryCommandMessages.SUCCESS_ON_UPDATE_CATEGORY_COMMAND);

            }
            catch
            {
                var errors = GetErrorsFromNotifications(ErrorCodes.UNEXPECTED_EXCEPTION);
                return new CommandResult(false, SaleCommandMessages.UNEXPECTED_EXCEPTION, errors);
            }
        }
    }
}
