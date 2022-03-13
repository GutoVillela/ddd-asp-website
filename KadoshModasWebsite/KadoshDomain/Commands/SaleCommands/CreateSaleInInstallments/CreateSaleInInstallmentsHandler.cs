using KadoshDomain.Commands.Base;
using KadoshDomain.Entities;
using KadoshDomain.Enums;
using KadoshDomain.Repositories;
using KadoshShared.Commands;
using KadoshShared.Constants.CommandMessages;
using KadoshShared.Constants.ErrorCodes;
using KadoshShared.Repositories;

namespace KadoshDomain.Commands.SaleCommands.CreateSaleInInstallments
{
    public class CreateSaleInInstallmentsHandler : CommandHandlerBase<CreateSaleInInstallmentsCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ISaleInInstallmentsRepository _saleInInstallmentsRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly IProductRepository _productRepository;
        private readonly IUserRepository _userRepository;
        private readonly IStoreRepository _storeRepository;
        private readonly ICustomerPostingRepository _customerPostingRepository;

        public CreateSaleInInstallmentsHandler(
            IUnitOfWork unitOfWork,
            ISaleInInstallmentsRepository saleInInstallmentsRepository,
            ICustomerRepository customerRepository,
            IProductRepository productRepository,
            IUserRepository userRepository,
            ICustomerPostingRepository customerPostingRepository,
            IStoreRepository storeRepository)
        {
            _unitOfWork = unitOfWork;
            _saleInInstallmentsRepository = saleInInstallmentsRepository;
            _customerRepository = customerRepository;
            _productRepository = productRepository;
            _userRepository = userRepository;
            _customerPostingRepository = customerPostingRepository;
            _storeRepository = storeRepository;
        }

        public override async Task<ICommandResult> HandleAsync(CreateSaleInInstallmentsCommand command)
        {
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

                // Validate Customer
                Customer? customer = await _customerRepository.ReadAsync(command.CustomerId!.Value);
                if (customer is null)
                {
                    var errors = GetErrorsFromNotifications(ErrorCodes.ERROR_COULD_NOT_FIND_SALE_CUSTOMER);
                    return new CommandResult(false, SaleCommandMessages.ERROR_COULD_NOT_FIND_SALE_CUSTOMER, errors);
                }

                // Validate Seller
                User? seller = await _userRepository.ReadAsync(command.SellerId!.Value);
                if (seller is null)
                {
                    var errors = GetErrorsFromNotifications(ErrorCodes.ERROR_COULD_NOT_FIND_SALE_SELLER);
                    return new CommandResult(false, SaleCommandMessages.ERROR_COULD_NOT_FIND_SALE_SELLER, errors);
                }

                // Validate Store
                Store? store = await _storeRepository.ReadAsync(command.StoreId!.Value);
                if (store is null)
                {
                    var errors = GetErrorsFromNotifications(ErrorCodes.ERROR_COULD_NOT_FIND_SALE_STORE);
                    return new CommandResult(false, SaleCommandMessages.ERROR_COULD_NOT_FIND_SALE_STORE, errors);
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

                // Create sale items
                IList<SaleItem> saleItems = new List<SaleItem>();
                foreach (var commandSaleItem in command.SaleItems)
                {
                    var product = products.FirstOrDefault(p => p.Id == commandSaleItem.ProductId);
                    SaleItem item = new(
                        product: product,
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

                // Create sale installments
                int installmentNumber = 1;
                IList<Installment> installments = new List<Installment>();
                foreach (var commandInstallment in command.Installments)
                {
                    Installment installment = new(
                        number: installmentNumber,
                        value: Sale.CalculateTotal(saleItems, command.DiscountInPercentage) / command.Installments.Count,
                        maturityDate: commandInstallment.MaturityDate,
                        situation: EInstallmentSituation.Open);

                    AddNotifications(installment);

                    // Check validations
                    if (!IsValid)
                    {
                        var errors = GetErrorsFromNotifications(ErrorCodes.ERROR_INVALID_SALE_INSTALLMENT);
                        return new CommandResult(false, SaleCommandMessages.ERROR_INVALID_SALE_INSTALLMENT, errors);
                    }

                    installments.Add(installment);
                }

                //Create Sale
                SaleInInstallments saleInInstallments = new(
                    customer: customer,
                    formOfPayment: command.FormOfPayment!.Value,
                    discountInPercentage: command.DiscountInPercentage,
                    downPayment: command.DownPayment,
                    saleDate: command.SaleDate!.Value,
                    situation: ESaleSituation.Open,
                    seller: seller,
                    store: store,
                    saleItems: saleItems as IReadOnlyCollection<SaleItem>,
                    installments: installments as IReadOnlyCollection<Installment>,
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

                // Create customer posting if there's any Down Payment
                if (saleInInstallments.DownPayment > 0)
                {
                    CustomerPosting customerPosting = new(
                        type: ECustomerPostingType.DownPayment,
                        value: saleInInstallments.Total,
                        sale: saleInInstallments,
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
    }
}
