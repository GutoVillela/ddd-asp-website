using KadoshDomain.Commands.Base;
using KadoshDomain.Entities;
using KadoshDomain.Enums;
using KadoshDomain.Repositories;
using KadoshShared.Commands;
using KadoshShared.Constants.CommandMessages;
using KadoshShared.Constants.ErrorCodes;
using KadoshShared.Repositories;

namespace KadoshDomain.Commands.SaleCommands.CreateSaleOnCredit
{
    public class CreateSaleOnCreditHandler : CommandHandlerBase<CreateSaleOnCreditCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ISaleOnCreditRepository _saleOnCreditRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly IProductRepository _productRepository;
        private readonly IUserRepository _userRepository;
        private readonly IStoreRepository _storeRepository;
        private readonly ISaleItemRepository _saleItemRepository;
        private readonly ICustomerPostingRepository _customerPostingRepository;

        public CreateSaleOnCreditHandler(
            IUnitOfWork unitOfWork,
            ISaleOnCreditRepository saleOnCreditRepository,
            ICustomerRepository customerRepository,
            IProductRepository productRepository,
            IUserRepository userRepository,
            ISaleItemRepository saleItemRepository,
            ICustomerPostingRepository customerPostingRepository,
            IStoreRepository storeRepository)
        {
            _unitOfWork = unitOfWork;
            _saleOnCreditRepository = saleOnCreditRepository;
            _customerRepository = customerRepository;
            _productRepository = productRepository;
            _userRepository = userRepository;
            _saleItemRepository = saleItemRepository;
            _customerPostingRepository = customerPostingRepository;
            _storeRepository = storeRepository;
        }

        public override async Task<ICommandResult> HandleAsync(CreateSaleOnCreditCommand command)
        {

            try
            {
                // Fail Fast Validation
                command.Validate();
                if (!command.IsValid)
                {
                    AddNotifications(command);
                    var errors = GetErrorsFromNotifications(ErrorCodes.ERROR_INVALID_SALE_ON_CREDIT_CREATE_COMMAND);
                    return new CommandResult(false, SaleCommandMessages.INVALID_SALE_ON_CREDIT_CREATE_COMMAND, errors);
                }

                (bool isCustomerSellerAndStoreValid, int errorCode, string errorMessage) = await ValidateSaleCustomerSellerAndStore(command.CustomerId, command.SellerId, command.StoreId);
                if (!isCustomerSellerAndStoreValid)
                {
                    AddNotifications(command);
                    var errors = GetErrorsFromNotifications(errorCode);
                    return new CommandResult(false, errorMessage, errors);
                }

                // Retrieve products info and validade products
                // TODO Create method to centralize this validations
                IList<Product> products = new List<Product>();
                foreach (var commandSaleItem in command.SaleItems)
                {
                    var productInfo = await _productRepository.ReadAsync(commandSaleItem.ProductId);

                    if (productInfo is null)
                    {
                        AddNotifications(command);
                        var errors = GetErrorsFromNotifications(ErrorCodes.ERROR_COULD_NOT_FIND_SALE_PRODUCT);
                        return new CommandResult(false, SaleCommandMessages.ERROR_COULD_NOT_FIND_SALE_PRODUCT, errors);
                    }
                    products.Add(productInfo);
                }

                // Create sale items
                IList<SaleItem> saleItems = new List<SaleItem>();
                foreach (var commandSaleItem in command.SaleItems)
                {
                    var product = products.FirstOrDefault(p => p.Id == commandSaleItem.ProductId);
                    SaleItem item = new(
                        saleId: 0,
                        productId: product!.Id,
                        amount: commandSaleItem.Amount,
                        price: product.Price,
                        discountInPercentage: commandSaleItem.DiscountInPercentage,
                        situation: ESaleItemSituation.AcquiredOnPurchase);

                    AddNotifications(item);

                    // Check validations
                    if (!IsValid)
                    {
                        var errors = GetErrorsFromNotifications(ErrorCodes.ERROR_INVALID_SALE_ITEM);
                        return new CommandResult(false, SaleCommandMessages.ERROR_INVALID_SALE_ITEM, errors);
                    }

                    saleItems.Add(item);
                }

                //Create Sale
                SaleOnCredit saleOnCredit = new(
                    customerId: command.CustomerId!.Value,
                    formOfPayment: command.FormOfPayment!.Value,
                    discountInPercentage: command.DiscountInPercentage,
                    downPayment: command.DownPayment,
                    saleDate: command.SaleDate!.Value,
                    situation: ESaleSituation.Open,
                    sellerId: command.SellerId!.Value,
                    storeId: command.StoreId!.Value,
                    saleItems: saleItems
                    );

                // Entity validations
                AddNotifications(saleOnCredit);

                // Check validations
                if (!IsValid)
                {
                    var errors = GetErrorsFromNotifications(ErrorCodes.ERROR_INVALID_SALE_ON_CREDIT_CREATE_COMMAND);
                    return new CommandResult(false, SaleCommandMessages.INVALID_SALE_ON_CREDIT_CREATE_COMMAND, errors);
                }

                // Save sale
                await _saleOnCreditRepository.CreateAsync(saleOnCredit);

                // Create customer posting if there's any Down Payment
                if (saleOnCredit.DownPayment > 0)
                {
                    CustomerPosting customerPosting = new(
                        type: ECustomerPostingType.DownPayment,
                        value: saleOnCredit.DownPayment,
                        sale: saleOnCredit,
                        postingDate: DateTime.UtcNow
                        );

                    // Entity validations
                    AddNotifications(customerPosting);

                    // Check validations
                    if (!IsValid)
                    {
                        var errors = GetErrorsFromNotifications(ErrorCodes.ERROR_COULD_NOT_CREATE_DOWN_PAYMENT_POSTING);
                        return new CommandResult(false, SaleCommandMessages.ERROR_COULD_NOT_CREATE_DOWN_PAYMENT_POSTING, errors);
                    }

                    // Register posting
                    await _customerPostingRepository.CreateAsync(customerPosting);
                }

                // Commit changes
                await _unitOfWork.CommitAsync();

                // Finish sale creation
                return new CommandResult(true, SaleCommandMessages.SUCCESS_ON_CREATE_SALE_COMMAND);
            }
            catch
            {
                var errors = GetErrorsFromNotifications(ErrorCodes.UNEXPECTED_EXCEPTION);
                return new CommandResult(false, SaleCommandMessages.UNEXPECTED_EXCEPTION, errors);
            }
        }

        // TODO Move this method to somewhere else where it makes more sense
        private async Task<(bool isValid, int errorCode, string errorMessage)> ValidateSaleCustomerSellerAndStore(int? customerId, int? sellerId, int? storeId)
        {
            if (customerId is null)
                return (false, ErrorCodes.ERROR_COULD_NOT_FIND_SALE_CUSTOMER, SaleCommandMessages.ERROR_COULD_NOT_FIND_SALE_CUSTOMER);

            if (sellerId is null)
                return (false, ErrorCodes.ERROR_COULD_NOT_FIND_SALE_SELLER, SaleCommandMessages.ERROR_COULD_NOT_FIND_SALE_SELLER);

            if (storeId is null)
                return (false, ErrorCodes.ERROR_COULD_NOT_FIND_SALE_STORE, SaleCommandMessages.ERROR_COULD_NOT_FIND_SALE_STORE);

            // Validate Customer
            Customer? customer = await _customerRepository.ReadAsync(customerId.Value);
            if (customer is null)
                return (false, ErrorCodes.ERROR_COULD_NOT_FIND_SALE_CUSTOMER, SaleCommandMessages.ERROR_COULD_NOT_FIND_SALE_CUSTOMER);

            // Validate Seller
            User? seller = await _userRepository.ReadAsync(sellerId.Value);
            if (seller is null)
                return (false, ErrorCodes.ERROR_COULD_NOT_FIND_SALE_SELLER, SaleCommandMessages.ERROR_COULD_NOT_FIND_SALE_SELLER);

            // Validate Store
            Store? store = await _storeRepository.ReadAsync(storeId.Value);
            if (store is null)
                return (false, ErrorCodes.ERROR_COULD_NOT_FIND_SALE_STORE, SaleCommandMessages.ERROR_COULD_NOT_FIND_SALE_STORE);

            // Validated
            return (true, 0, string.Empty);
        }
    }
}
