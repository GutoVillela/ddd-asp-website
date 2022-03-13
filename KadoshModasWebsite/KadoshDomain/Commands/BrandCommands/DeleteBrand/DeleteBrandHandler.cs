using KadoshDomain.Commands.Base;
using KadoshDomain.Entities;
using KadoshDomain.Repositories;
using KadoshShared.Commands;
using KadoshShared.Constants.CommandMessages;
using KadoshShared.Constants.ErrorCodes;
using KadoshShared.Repositories;

namespace KadoshDomain.Commands.BrandCommands.DeleteBrand
{
    public class DeleteBrandHandler : CommandHandlerBase<DeleteBrandCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IBrandRepository _repository;

        public DeleteBrandHandler(IUnitOfWork unitOfWork, IBrandRepository repository)
        {
            _unitOfWork = unitOfWork;
            _repository = repository;
        }

        public override async Task<ICommandResult> HandleAsync(DeleteBrandCommand command)
        {
            try
            {
                // Fail Fast Validation
                command.Validate();
                if (!command.IsValid)
                {
                    AddNotifications(command);
                    var errors = GetErrorsFromNotifications(ErrorCodes.ERROR_INVALID_BRAND_DELETE_COMMAND);
                    return new CommandResult(false, BrandCommandMessages.INVALID_BRAND_DELETE_COMMAND, errors);
                }

                // Get Entity
                Brand? brand = await _repository.ReadAsync(command.Id ?? 0);

                // Entity validations
                if (brand is null)
                    AddNotification(nameof(brand), BrandCommandMessages.ERROR_BRAND_NOT_FOUND);

                // Check validations
                if (!IsValid)
                {
                    var errors = GetErrorsFromNotifications(ErrorCodes.ERROR_INVALID_BRAND_DELETE_COMMAND);
                    return new CommandResult(false, BrandCommandMessages.INVALID_BRAND_DELETE_COMMAND, errors);
                }

                // Delete data
                _repository.Delete(brand!);

                // Commit changes
                await _unitOfWork.CommitAsync();

                return new CommandResult(true, BrandCommandMessages.SUCCESS_ON_DELETE_BRAND_COMMAND);
            }
            catch
            {
                var errors = GetErrorsFromNotifications(ErrorCodes.UNEXPECTED_EXCEPTION);
                return new CommandResult(false, SaleCommandMessages.UNEXPECTED_EXCEPTION, errors);
            }
        }
    }
}
