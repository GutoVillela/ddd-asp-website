
using Flunt.Notifications;
using KadoshDomain.Commands;
using KadoshDomain.Entities;
using KadoshDomain.Repositories;
using KadoshShared.Commands;
using KadoshShared.Constants.CommandMessages;
using KadoshShared.Constants.ErrorCodes;
using KadoshShared.Handlers;
using KadoshShared.Repositories;
using KadoshShared.ValueObjects;

namespace KadoshDomain.Handlers
{
    public class ProductHandler : Notifiable<Notification>, IHandler<CreateProductCommand>, IHandler<UpdateProductCommand>, IHandler<DeleteProductCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IProductRepository _repository;

        public ProductHandler(IUnitOfWork unitOfWork, IProductRepository repository)
        {
            _unitOfWork = unitOfWork;
            _repository = repository;
        }

        public async Task<ICommandResult> HandleAsync(CreateProductCommand command)
        {
            try
            {
                // Fail Fast Validations
                command.Validate();
                if (!command.IsValid)
                {
                    AddNotifications(command);
                    var errors = GetErrorsFromNotifications(ErrorCodes.ERROR_INVALID_PRODUCT_CREATE_COMMAND);
                    return new CommandResult(false, ProductCommandMessages.INVALID_PRODUCT_CREATE_COMMAND, errors);

                }

                // Create Entity
                Product product = new(
                    name: command.Name,
                    barCode: command.BarCode,
                    price: command.Price.Value,
                    categoryId: command.CategoryId.Value,
                    brandId: command.BrandId.Value
                    );

                // Group validations
                AddNotifications(product);

                // Validate before register product
                if (!IsValid)
                {
                    var errors = GetErrorsFromNotifications(ErrorCodes.ERROR_INVALID_PRODUCT_CREATE_COMMAND);
                    return new CommandResult(false, ProductCommandMessages.INVALID_PRODUCT_CREATE_COMMAND, errors);
                }

                // Create register
                await _repository.CreateAsync(product);

                // Commit changes
                await _unitOfWork.CommitAsync();

                return new CommandResult(true, ProductCommandMessages.SUCCESS_ON_CREATE_PRODUCT_COMMAND);

            }
            catch
            {
                var errors = GetErrorsFromNotifications(ErrorCodes.UNEXPECTED_EXCEPTION);
                return new CommandResult(false, SaleCommandMessages.UNEXPECTED_EXCEPTION, errors);
            }
        }
        public async Task<ICommandResult> HandleAsync(UpdateProductCommand command)
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

        public async Task<ICommandResult> HandleAsync(DeleteProductCommand command)
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
                    AddNotification(nameof(product), ProductCommandMessages.ERROR_PRODUCT_NOT_FOUND);

                // Check validations
                if (!IsValid)
                {
                    var errors = GetErrorsFromNotifications(ErrorCodes.ERROR_INVALID_PRODUCT_DELETE_COMMAND);
                    return new CommandResult(false, ProductCommandMessages.INVALID_PRODUCT_DELETE_COMMAND, errors);
                }

                // Delete data
                _repository.Delete(product);

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