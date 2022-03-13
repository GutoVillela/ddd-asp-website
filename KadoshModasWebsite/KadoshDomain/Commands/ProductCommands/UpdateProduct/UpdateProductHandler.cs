using KadoshDomain.Commands.Base;
using KadoshDomain.Entities;
using KadoshDomain.Repositories;
using KadoshShared.Commands;
using KadoshShared.Constants.CommandMessages;
using KadoshShared.Constants.ErrorCodes;
using KadoshShared.Repositories;

namespace KadoshDomain.Commands.ProductCommands.UpdateProduct
{
    public class UpdateProductHandler : CommandHandlerBase<UpdateProductCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IProductRepository _repository;

        public UpdateProductHandler(IUnitOfWork unitOfWork, IProductRepository repository)
        {
            _unitOfWork = unitOfWork;
            _repository = repository;
        }

        public override async Task<ICommandResult> HandleAsync(UpdateProductCommand command)
        {
            try
            {
                // Fail Fast Validations
                command.Validate();
                if (!command.IsValid)
                {
                    AddNotifications(command);
                    var errors = GetErrorsFromNotifications(ErrorCodes.ERROR_INVALID_PRODUCT_UPDATE_COMMAND);
                    return new CommandResult(false, ProductCommandMessages.INVALID_PRODUCT_UPDATE_COMMAND, errors);

                }

                // Recover entity to update its values
                Product product = await _repository.ReadAsync(command.Id.Value);

                if (product is null)
                {
                    AddNotification(nameof(command.Id), ProductCommandMessages.ERROR_PRODUCT_NOT_FOUND);
                    var errors = GetErrorsFromNotifications(ErrorCodes.ERROR_PRODUCT_NOT_FOUND);
                    return new CommandResult(false, ProductCommandMessages.ERROR_PRODUCT_NOT_FOUND, errors);
                }

                product.UpdateProductInfo(
                    name: command.Name,
                    barCode: command.BarCode,
                    price: command.Price.Value,
                    categoryId: command.CategoryId.Value,
                    brandId: command.BrandId.Value
                    );

                product.SetLastUpdateDate(DateTime.UtcNow);

                // Group validations
                AddNotifications(product);

                // Validate before register customer
                if (!IsValid)
                {
                    var errors = GetErrorsFromNotifications(ErrorCodes.ERROR_INVALID_PRODUCT_UPDATE_COMMAND);
                    return new CommandResult(false, ProductCommandMessages.INVALID_PRODUCT_UPDATE_COMMAND, errors);
                }

                // Update entity
                await _repository.UpdateAsync(product);

                // Commit changes
                await _unitOfWork.CommitAsync();

                return new CommandResult(true, ProductCommandMessages.SUCCESS_ON_UPDATE_PRODUCT_COMMAND);

            }
            catch
            {
                var errors = GetErrorsFromNotifications(ErrorCodes.UNEXPECTED_EXCEPTION);
                return new CommandResult(false, SaleCommandMessages.UNEXPECTED_EXCEPTION, errors);
            }
        }
    }
}
