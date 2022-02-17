using Flunt.Notifications;
using KadoshDomain.Commands;
using KadoshDomain.Entities;
using KadoshDomain.Enums;
using KadoshDomain.Queries;
using KadoshDomain.Repositories;
using KadoshShared.Commands;
using KadoshShared.Constants.CommandMessages;
using KadoshShared.Constants.ErrorCodes;
using KadoshShared.Handlers;
using KadoshShared.Repositories;
using KadoshShared.ValueObjects;
using System.Linq;

namespace KadoshDomain.Handlers
{
    public class SaleHandler : Notifiable<Notification>, IHandler<CreateSaleInCashCommand>, IHandler<CreateSaleInInstallmentsCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ISaleInCashRepository _saleInCashRepository;
        private readonly ISaleInInstallmentsRepository _saleInInstallmentsRepository;
        private readonly ISaleOnCreditRepository _saleOnCreditRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly IProductRepository _productRepository;
        private readonly IUserRepository _userRepository;
        private readonly IStoreRepository _storeRepository;
        private readonly ISaleItemRepository _saleItemRepository;
        private readonly ICustomerPostingRepository _customerPostingRepository;
        private readonly IInstallmentRepository _installmentRepository;

        public SaleHandler(
            IUnitOfWork unitOfWork,
            ISaleInCashRepository saleInCashRepository,
            ISaleInInstallmentsRepository saleInInstallmentsRepository,
            ISaleOnCreditRepository saleOnCreditRepository,
            ICustomerRepository customerRepository,
            IProductRepository productRepository,
            IUserRepository userRepository,
            ISaleItemRepository saleItemRepository,
            ICustomerPostingRepository customerPostingRepository,
            IInstallmentRepository installmentRepository,
            IStoreRepository storeRepository)
        {
            _unitOfWork = unitOfWork;
            _saleInCashRepository = saleInCashRepository;
            _saleInInstallmentsRepository = saleInInstallmentsRepository;
            _saleOnCreditRepository = saleOnCreditRepository;
            _customerRepository = customerRepository;
            _productRepository = productRepository;
            _userRepository = userRepository;
            _saleItemRepository = saleItemRepository;
            _customerPostingRepository = customerPostingRepository;
            _installmentRepository = installmentRepository;
            _storeRepository = storeRepository;
        }

        private async Task<(bool isValid, int errorCode, string errorMessage)> ValidateSaleCustomerSellerAndStore(int? customerId, int? sellerId, int? storeId)
        {
            if(customerId is null)
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
        
        public async Task<ICommandResult> HandleAsync(CreateSaleInCashCommand command)
        {
            try
            {
                // Fail Fast Validation
                command.Validate();
                if (!command.IsValid)
                {
                    AddNotifications(command);
                    var errors = GetErrorsFromNotifications(ErrorCodes.ERROR_INVALID_SALE_IN_CASH_CREATE_COMMAND);
                    return new CommandResult(false, SaleCommandMessages.INVALID_SALE_IN_CASH_CREATE_COMMAND, errors);
                }

                (bool isCustomerSellerAndStoreValid, int errorCode, string errorMessage) = await ValidateSaleCustomerSellerAndStore(command.CustomerId, command.SellerId, command.StoreId);
                if (!isCustomerSellerAndStoreValid)
                {
                    AddNotifications(command);
                    var errors = GetErrorsFromNotifications(errorCode);
                    return new CommandResult(false, errorMessage, errors);
                }

                // Retrieve products info and validade products
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

                //Create Sale
                SaleInCash saleInCash = new(
                    customerId: command.CustomerId!.Value,
                    formOfPayment: command.FormOfPayment!.Value,
                    discountInPercentage: command.DiscountInPercentage,
                    downPayment: command.DownPayment,
                    saleDate: command.SaleDate!.Value,
                    situation: ESaleSituation.Completed,
                    sellerId: command.SellerId!.Value,
                    storeId: command.StoreId!.Value,
                    settlementDate: command.SettlementDate!.Value
                    );

                // Entity validations
                AddNotifications(saleInCash);

                // Check validations
                if (!IsValid)
                {
                    var errors = GetErrorsFromNotifications(ErrorCodes.ERROR_INVALID_SALE_IN_CASH_CREATE_COMMAND);
                    return new CommandResult(false, SaleCommandMessages.INVALID_SALE_IN_CASH_CREATE_COMMAND, errors);
                }

                // Save sale
                await _saleInCashRepository.CreateAsync(saleInCash);

                // Create sale items
                IList<SaleItem> saleItems = new List<SaleItem>();
                foreach (var commandSaleItem in command.SaleItems)
                {
                    var product = products.FirstOrDefault(p => p.Id == commandSaleItem.ProductId);
                    SaleItem item = new(
                        saleId: saleInCash.Id,
                        productId: product!.Id,
                        amount: commandSaleItem.Amount,
                        price: product.Price,
                        discountInPercentage: commandSaleItem.DiscountInPercentage,
                        situation: Enums.ESaleItemSituation.AcquiredOnPurchase);

                    AddNotifications(item);

                    // Check validations
                    if (!IsValid)
                    {
                        var errors = GetErrorsFromNotifications(ErrorCodes.ERROR_INVALID_SALE_ITEM);
                        return new CommandResult(false, SaleCommandMessages.ERROR_INVALID_SALE_ITEM, errors);
                    }

                    saleItems.Add(item);
                    await _saleItemRepository.CreateAsync(item);
                }

                saleInCash.SetSaleItems(saleItems);

                // Create customer posting
                CustomerPosting customerPosting = new(
                    customerId: command.CustomerId.Value,
                    type: ECustomerPostingType.CashPurchase,
                    value: saleInCash.Total,
                    saleId: saleInCash.Id,
                    postingDate: DateTime.UtcNow
                    );

                // Entity validations
                AddNotifications(customerPosting);

                // Check validations
                if (!IsValid)
                {
                    var errors = GetErrorsFromNotifications(ErrorCodes.ERROR_COULD_NOT_CREATE_SALE_IN_CASH_POSTING);
                    return new CommandResult(false, SaleCommandMessages.ERROR_COULD_NOT_CREATE_SALE_IN_CASH_POSTING, errors);
                }

                // Register posting
                await _customerPostingRepository.CreateAsync(customerPosting);

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

        public async Task<ICommandResult> HandleAsync(CreateSaleInInstallmentsCommand command)
        {
            //TODO implement transaction in sale creation

            try
            {
                // Fail Fast Validation
                command.Validate();
                if (!command.IsValid)
                {
                    AddNotifications(command);
                    var errors = GetErrorsFromNotifications(ErrorCodes.ERROR_INVALID_SALE_IN_INSTALLMENTS_CREATE_COMMAND);
                    return new CommandResult(false, SaleCommandMessages.INVALID_SALE_IN_INSTALLMENTS_CREATE_COMMAND, errors);
                }

                (bool isCustomerSellerAndStoreValid, int errorCode, string errorMessage) = await ValidateSaleCustomerSellerAndStore(command.CustomerId, command.SellerId, command.StoreId);
                if (!isCustomerSellerAndStoreValid)
                {
                    AddNotifications(command);
                    var errors = GetErrorsFromNotifications(errorCode);
                    return new CommandResult(false, errorMessage, errors);
                }

                // Retrieve products info and validade products
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

                //Create Sale
                SaleInInstallments saleInInstallments = new(
                    customerId: command.CustomerId!.Value,
                    formOfPayment: command.FormOfPayment!.Value,
                    discountInPercentage: command.DiscountInPercentage,
                    downPayment: command.DownPayment,
                    saleDate: command.SaleDate!.Value,
                    situation: ESaleSituation.Open,
                    sellerId: command.SellerId!.Value,
                    storeId: command.StoreId!.Value,
                    interestOnTheTotalSaleInPercentage: command.InterestOnTheTotalSaleInPercentage
                    );

                // Entity validations
                AddNotifications(saleInInstallments);

                // Check validations
                if (!IsValid)
                {
                    var errors = GetErrorsFromNotifications(ErrorCodes.ERROR_INVALID_SALE_IN_INSTALLMENTS_CREATE_COMMAND);
                    return new CommandResult(false, SaleCommandMessages.INVALID_SALE_IN_INSTALLMENTS_CREATE_COMMAND, errors);
                }

                // Save sale
                await _saleInInstallmentsRepository.CreateAsync(saleInInstallments);

                // Create sale items
                IList<SaleItem> saleItems = new List<SaleItem>();
                foreach (var commandSaleItem in command.SaleItems)
                {
                    var product = products.FirstOrDefault(p => p.Id == commandSaleItem.ProductId);
                    SaleItem item = new(
                        saleId: saleInInstallments.Id,
                        productId: product!.Id,
                        amount: commandSaleItem.Amount,
                        price: product.Price,
                        discountInPercentage: commandSaleItem.DiscountInPercentage,
                        situation: Enums.ESaleItemSituation.AcquiredOnPurchase);

                    AddNotifications(item);

                    // Check validations
                    if (!IsValid)
                    {
                        var errors = GetErrorsFromNotifications(ErrorCodes.ERROR_INVALID_SALE_ITEM);
                        return new CommandResult(false, SaleCommandMessages.ERROR_INVALID_SALE_ITEM, errors);
                    }

                    saleItems.Add(item);
                    await _saleItemRepository.CreateAsync(item);
                }

                saleInInstallments.SetSaleItems(saleItems);

                // Create sale installments
                int installmentNumber = 1;
                foreach (var commandInstallment in command.Installments)
                {
                    Installment installment = new(
                        number: installmentNumber,
                        value: saleInInstallments.Total / command.Installments.Count,
                        maturityDate: commandInstallment.MaturityDate,
                        situation: EInstallmentSituation.Open,
                        saleId: saleInInstallments.Id);

                    AddNotifications(installment);

                    // Check validations
                    if (!IsValid)
                    {
                        var errors = GetErrorsFromNotifications(ErrorCodes.ERROR_INVALID_SALE_INSTALLMENT);
                        return new CommandResult(false, SaleCommandMessages.ERROR_INVALID_SALE_INSTALLMENT, errors);
                    }

                    await _installmentRepository.CreateAsync(installment);
                }

                // Create customer posting if there's any Down Payment
                if (saleInInstallments.DownPayment > 0)
                {
                    CustomerPosting customerPosting = new(
                        customerId: command.CustomerId!.Value,
                        type: ECustomerPostingType.DownPayment,
                        value: saleInInstallments.Total,
                        saleId: saleInInstallments.Id,
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

        public async Task<ICommandResult> HandleAsync(CreateSaleOnCreditCommand command)
        {
            //TODO implement transaction in sale creation

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

                //Create Sale
                SaleOnCredit saleOnCredit = new(
                    customerId: command.CustomerId!.Value,
                    formOfPayment: command.FormOfPayment!.Value,
                    discountInPercentage: command.DiscountInPercentage,
                    downPayment: command.DownPayment,
                    saleDate: command.SaleDate!.Value,
                    situation: ESaleSituation.Open,
                    sellerId: command.SellerId!.Value,
                    storeId: command.StoreId!.Value
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

                // Create sale items
                IList<SaleItem> saleItems = new List<SaleItem>();
                foreach (var commandSaleItem in command.SaleItems)
                {
                    var product = products.FirstOrDefault(p => p.Id == commandSaleItem.ProductId);
                    SaleItem item = new(
                        saleId: saleOnCredit.Id,
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
                    await _saleItemRepository.CreateAsync(item);
                }

                saleOnCredit.SetSaleItems(saleItems);

                // Create customer posting if there's any Down Payment
                if (saleOnCredit.DownPayment > 0)
                {
                    CustomerPosting customerPosting = new(
                        customerId: command.CustomerId!.Value,
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
