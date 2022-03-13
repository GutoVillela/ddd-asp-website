using KadoshDomain.Commands.Base;
using KadoshDomain.Entities;
using KadoshDomain.Repositories;
using KadoshShared.Commands;
using KadoshShared.Constants.CommandMessages;
using KadoshShared.Constants.ErrorCodes;
using KadoshShared.Repositories;

namespace KadoshDomain.Commands.ProductCommands.CreateProduct
{
    public class CreateProductHandler : CommandHandlerBase<CreateProductCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IProductRepository _repository;

        public CreateProductHandler(IUnitOfWork unitOfWork, IProductRepository repository)
        {
            _unitOfWork = unitOfWork;
            _repository = repository;
        }

        public override async Task<ICommandResult> HandleAsync(CreateProductCommand command)
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
    }
}
