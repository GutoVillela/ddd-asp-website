using KadoshDomain.Commands.Base;
using KadoshDomain.Entities;
using KadoshDomain.Repositories;
using KadoshShared.Commands;
using KadoshShared.Constants.CommandMessages;
using KadoshShared.Constants.ErrorCodes;
using KadoshShared.Repositories;

namespace KadoshDomain.Commands.BrandCommands.UpdateBrand
{
    public class UpdateBrandHandler : CommandHandlerBase<UpdateBrandCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IBrandRepository _repository;

        public UpdateBrandHandler(IUnitOfWork unitOfWork, IBrandRepository repository)
        {
            _unitOfWork = unitOfWork;
            _repository = repository;
        }

        public override async Task<ICommandResult> HandleAsync(UpdateBrandCommand command)
        {
            try
            {
                // Fail Fast Validations
                command.Validate();
                if (!command.IsValid)
                {
                    AddNotifications(command);
                    var errors = GetErrorsFromNotifications(ErrorCodes.ERROR_INVALID_BRAND_UPDATE_COMMAND);
                    return new CommandResult(false, BrandCommandMessages.INVALID_BRAND_UPDATE_COMMAND, errors);

                }

                // Recover entity to update its values
                Brand? brand = await _repository.ReadAsync(command.Id.Value);

                if (brand is null)
                {
                    AddNotification(nameof(command.Id), BrandCommandMessages.ERROR_BRAND_NOT_FOUND);
                    var errors = GetErrorsFromNotifications(ErrorCodes.ERROR_BRAND_NOT_FOUND);
                    return new CommandResult(false, BrandCommandMessages.ERROR_BRAND_NOT_FOUND, errors);
                }

                brand.UpdateBrandInfo(
                    name: command.Name
                    );

                brand.SetLastUpdateDate(DateTime.UtcNow);

                // Group validations
                AddNotifications(brand);

                // Validate before register customer
                if (!IsValid)
                {
                    var errors = GetErrorsFromNotifications(ErrorCodes.ERROR_INVALID_BRAND_UPDATE_COMMAND);
                    return new CommandResult(false, BrandCommandMessages.INVALID_BRAND_UPDATE_COMMAND, errors);
                }

                // Update entity
                await _repository.UpdateAsync(brand);

                // Commit changes
                await _unitOfWork.CommitAsync();

                return new CommandResult(true, BrandCommandMessages.SUCCESS_ON_UPDATE_BRAND_COMMAND);
            }
            catch
            {
                var errors = GetErrorsFromNotifications(ErrorCodes.UNEXPECTED_EXCEPTION);
                return new CommandResult(false, SaleCommandMessages.UNEXPECTED_EXCEPTION, errors);
            }
        }
    }
}
