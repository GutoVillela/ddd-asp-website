
using Flunt.Notifications;
using KadoshDomain.Commands;
using KadoshDomain.Entities;
using KadoshDomain.Repositories;
using KadoshShared.Commands;
using KadoshShared.Constants.CommandMessages;
using KadoshShared.Constants.ErrorCodes;
using KadoshShared.Handlers;
using KadoshShared.ValueObjects;

namespace KadoshDomain.Handlers
{
    public class CategoryHandler : Notifiable<Notification>, IHandler<CreateCategoryCommand>, IHandler<UpdateCategoryCommand>, IHandler<DeleteCategoryCommand>
    {
        private readonly ICategoryRepository _repository;

        public CategoryHandler(ICategoryRepository repository)
        {
            _repository = repository;
        }

        public async Task<ICommandResult> HandleAsync(CreateCategoryCommand command)
        {
            // Fail Fast Validations
            command.Validate();
            if (!command.IsValid)
            {
                AddNotifications(command);
                var errors = GetErrorsFromNotifications(ErrorCodes.ERROR_INVALID_CATEGORY_CREATE_COMMAND);
                return new CommandResult(false, CategoryCommandMessages.INVALID_CATEGORY_CREATE_COMMAND, errors);

            }

            // Create Entity
            Category category = new(
                name: command.Name
                );

            // Group validations
            AddNotifications(category);

            // Validate before register customer
            if (!IsValid)
            {
                var errors = GetErrorsFromNotifications(ErrorCodes.ERROR_INVALID_CATEGORY_CREATE_COMMAND);
                return new CommandResult(false, CategoryCommandMessages.INVALID_CATEGORY_CREATE_COMMAND, errors);
            }

            // Create register
            await _repository.CreateAsync(category);

            return new CommandResult(true, CategoryCommandMessages.SUCCESS_ON_CREATE_CATEGORY_COMMAND);
        }
        public async Task<ICommandResult> HandleAsync(UpdateCategoryCommand command)
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

            return new CommandResult(true, CategoryCommandMessages.SUCCESS_ON_UPDATE_CATEGORY_COMMAND);
        }

        public async Task<ICommandResult> HandleAsync(DeleteCategoryCommand command)
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
            Category category = await _repository.ReadAsync(command.Id ?? 0);

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

            // Delete data
            await _repository.DeleteAsync(category);

            return new CommandResult(true, CategoryCommandMessages.SUCCESS_ON_DELETE_CATEGORY_COMMAND);
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