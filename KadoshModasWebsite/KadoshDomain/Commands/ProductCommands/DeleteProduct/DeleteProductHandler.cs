using KadoshDomain.Commands.Base;
using KadoshDomain.Entities;
using KadoshDomain.Repositories;
using KadoshShared.Commands;
using KadoshShared.Constants.CommandMessages;
using KadoshShared.Constants.ErrorCodes;
using KadoshShared.Repositories;

namespace KadoshDomain.Commands.ProductCommands.DeleteProduct
{
    public class DeleteProductHandler : CommandHandlerBase<DeleteProductCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IProductRepository _repository;

        public DeleteProductHandler(IUnitOfWork unitOfWork, IProductRepository repository)
        {
            _unitOfWork = unitOfWork;
            _repository = repository;
        }

        public override async Task<ICommandResult> HandleAsync(DeleteProductCommand command)
        {
            try
            {
                // Fail Fast Validation
                command.Validate();
                if (!command.IsValid)
                {
                    AddNotifications(command);
                    var errors = GetErrorsFromNotifications(ErrorCodes.ERROR_INVALID_PRODUCT_DELETE_COMMAND);
                    return new CommandResult(false, ProductCommandMessages.INVALID_PRODUCT_DELETE_COMMAND, errors);
                }

                // Get Entity
                Product product = await _repository.ReadAsync(command.Id ?? 0);

                // Entity validations
                if (product is null)
                {
                    AddNotification(nameof(product), ProductCommandMessages.ERROR_PRODUCT_NOT_FOUND);
                    var errors = GetErrorsFromNotifications(ErrorCodes.ERROR_INVALID_PRODUCT_DELETE_COMMAND);
                    return new CommandResult(false, ProductCommandMessages.INVALID_PRODUCT_DELETE_COMMAND, errors);
                }

                // Inactivate data (instead of deleting)
                product.Inactivate();
                await _repository.UpdateAsync(product);

                // Commit changes
                await _unitOfWork.CommitAsync();

                return new CommandResult(true, ProductCommandMessages.SUCCESS_ON_DELETE_PRODUCT_COMMAND);
            }
            catch
            {
                var errors = GetErrorsFromNotifications(ErrorCodes.UNEXPECTED_EXCEPTION);
                return new CommandResult(false, SaleCommandMessages.UNEXPECTED_EXCEPTION, errors);
            }
        }
    }
}
