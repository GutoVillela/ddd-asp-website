using KadoshDomain.Commands.Base;
using KadoshDomain.Entities;
using KadoshDomain.Repositories;
using KadoshShared.Commands;
using KadoshShared.Constants.CommandMessages;
using KadoshShared.Constants.ErrorCodes;
using KadoshShared.Repositories;

namespace KadoshDomain.Commands.BrandCommands.CreateBrand
{
    public class CreateBrandHandler : CommandHandlerBase<CreateBrandCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IBrandRepository _repository;

        public CreateBrandHandler(IUnitOfWork unitOfWork, IBrandRepository repository)
        {
            _unitOfWork = unitOfWork;
            _repository = repository;
        }

        public override async Task<ICommandResult> HandleAsync(CreateBrandCommand command)
        {
            try
            {
                // Fail Fast Validations
                command.Validate();
                if (!command.IsValid)
                {
                    AddNotifications(command);
                    var errors = GetErrorsFromNotifications(ErrorCodes.ERROR_INVALID_BRAND_CREATE_COMMAND);
                    return new CommandResult(false, BrandCommandMessages.INVALID_BRAND_CREATE_COMMAND, errors);

                }

                // Create Entity
                Brand brand = new(
                    name: command.Name
                    );

                // Group validations
                AddNotifications(brand);

                // Validate before register customer
                if (!IsValid)
                {
                    var errors = GetErrorsFromNotifications(ErrorCodes.ERROR_INVALID_BRAND_CREATE_COMMAND);
                    return new CommandResult(false, BrandCommandMessages.INVALID_BRAND_CREATE_COMMAND, errors);
                }

                // Create register
                await _repository.CreateAsync(brand);

                // Commit changes
                await _unitOfWork.CommitAsync();

                return new CommandResult(true, BrandCommandMessages.SUCCESS_ON_CREATE_BRAND_COMMAND);

            }
            catch
            {
                var errors = GetErrorsFromNotifications(ErrorCodes.UNEXPECTED_EXCEPTION);
                return new CommandResult(false, SaleCommandMessages.UNEXPECTED_EXCEPTION, errors);
            }

        }
    }
}
