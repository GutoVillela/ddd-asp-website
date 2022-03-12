using KadoshDomain.Commands.Base;
using KadoshDomain.Entities;
using KadoshDomain.Repositories;
using KadoshShared.Commands;
using KadoshShared.Constants.CommandMessages;
using KadoshShared.Constants.ErrorCodes;
using KadoshShared.Repositories;

namespace KadoshDomain.Commands.CategoryCommands.CreateCategory
{
    public class CreateCategoryHandler : HandlerBase<CreateCategoryCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICategoryRepository _repository;

        public CreateCategoryHandler(IUnitOfWork unitOfWork, ICategoryRepository repository)
        {
            _unitOfWork = unitOfWork;
            _repository = repository;
        }

        public override async Task<ICommandResult> HandleAsync(CreateCategoryCommand command)
        {
            try
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

                // Commit changes
                await _unitOfWork.CommitAsync();

                return new CommandResult(true, CategoryCommandMessages.SUCCESS_ON_CREATE_CATEGORY_COMMAND);

            }
            catch
            {
                var errors = GetErrorsFromNotifications(ErrorCodes.UNEXPECTED_EXCEPTION);
                return new CommandResult(false, SaleCommandMessages.UNEXPECTED_EXCEPTION, errors);
            }
        }
    }
}
