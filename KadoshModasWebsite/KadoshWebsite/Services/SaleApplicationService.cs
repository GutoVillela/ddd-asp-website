using KadoshDomain.Commands.SaleCommands.CreateSaleInCash;
using KadoshDomain.Commands.SaleCommands.CreateSaleInInstallments;
using KadoshDomain.Commands.SaleCommands.CreateSaleOnCredit;
using KadoshDomain.Commands.SaleCommands.InformPayment;
using KadoshDomain.Commands.SaleCommands.PayOffSale;
using KadoshDomain.Entities;
using KadoshDomain.Enums;
using KadoshDomain.Queries.SaleQueries.DTOs;
using KadoshDomain.Queries.SaleQueries.GetAllSales;
using KadoshDomain.Queries.SaleQueries.GetAllSalesByCustomerId;
using KadoshDomain.Queries.SaleQueries.GetSaleById;
using KadoshShared.Commands;
using KadoshShared.ExtensionMethods;
using KadoshShared.Handlers;
using KadoshWebsite.Infrastructure;
using KadoshWebsite.Models;
using KadoshWebsite.Models.Enums;
using KadoshWebsite.Services.Interfaces;

namespace KadoshWebsite.Services
{
    public class SaleApplicationService : ISaleApplicationService
    {

        private readonly ICommandHandler<CreateSaleInCashCommand> _createSaleInCashHandler;
        private readonly ICommandHandler<CreateSaleInInstallmentsCommand> _createSaleInInstallmentsHandler;
        private readonly ICommandHandler<CreateSaleOnCreditCommand> _createSaleOnCreditHandler;
        private readonly ICommandHandler<PayOffSaleCommand> _payOffSaleHandler;
        private readonly ICommandHandler<InformSalePaymentCommand> _informSalePaymentHandler;

        private readonly IQueryHandler<GetAllSalesQuery, GetAllSalesQueryResult> _getAllSalesQueryHandler;
        private readonly IQueryHandler<GetAllSalesByCustomerIdQuery, GetAllSalesByCustomerIdQueryResult> _getAllSalesByCustomerIdQueryHandler;
        private readonly IQueryHandler<GetSaleByIdQuery, GetSaleByIdQueryResult> _getSaleByIdQueryHandler;

        private readonly IProductApplicationService _productService;


        public SaleApplicationService(
            ICommandHandler<CreateSaleInCashCommand> createSaleInCashHandler,
            ICommandHandler<CreateSaleInInstallmentsCommand> createSaleInInstallmentsHandler,
            ICommandHandler<CreateSaleOnCreditCommand> createSaleOnCreditHandler,
            ICommandHandler<PayOffSaleCommand> payOffSaleHandler,
            ICommandHandler<InformSalePaymentCommand> informSalePaymentHandler,
            IQueryHandler<GetAllSalesQuery, GetAllSalesQueryResult> getAllSalesQueryHandler,
            IQueryHandler<GetAllSalesByCustomerIdQuery, GetAllSalesByCustomerIdQueryResult> getAllSalesByCustomerIdQueryHandler,
            IQueryHandler<GetSaleByIdQuery, GetSaleByIdQueryResult> getSaleByIdQueryHandler,
            IProductApplicationService productService)
        {
            _createSaleInCashHandler = createSaleInCashHandler;
            _createSaleInInstallmentsHandler = createSaleInInstallmentsHandler;
            _createSaleOnCreditHandler = createSaleOnCreditHandler;
            _payOffSaleHandler = payOffSaleHandler;
            _informSalePaymentHandler = informSalePaymentHandler;
            _getAllSalesQueryHandler = getAllSalesQueryHandler;
            _getAllSalesByCustomerIdQueryHandler = getAllSalesByCustomerIdQueryHandler;
            _getSaleByIdQueryHandler = getSaleByIdQueryHandler;
            _productService = productService;
        }

        public async Task<ICommandResult> CreateSaleAsync(SaleViewModel sale)
        {
            if (sale is null)
                throw new ArgumentNullException(nameof(sale));

            if (sale.PaymentType == ESalePaymentType.Cash)
            {
                CreateSaleInCashCommand command = new();
                command.CustomerId = sale.CustomerId;
                command.FormOfPayment = EFormOfPayment.Cash;
                command.DiscountInPercentage = 0; // TODO implement discount in total sale
                command.DownPayment = sale.DownPayment ?? 0;
                command.SaleDate = DateTime.UtcNow;
                command.SellerId = sale.SellerId;
                command.StoreId = sale.StoreId;
                command.SettlementDate = DateTime.UtcNow;
                command.SaleItems = sale.SaleItems.Select(x => new SaleItem(0, x.ProductId, x.Quantity, 0, x.DiscountInPercentage ?? 0, ESaleItemSituation.AcquiredOnPurchase));

                return await _createSaleInCashHandler.HandleAsync(command);
            }
            else if (sale.PaymentType == ESalePaymentType.InStallments)
            {
                CreateSaleInInstallmentsCommand command = new();
                command.CustomerId = sale.CustomerId;
                command.FormOfPayment = EFormOfPayment.Cash;
                command.DiscountInPercentage = 0; // TODO implement discount in total sale
                command.DownPayment = sale.DownPayment ?? 0;
                command.SaleDate = DateTime.UtcNow;
                command.SellerId = sale.SellerId;
                command.StoreId = sale.StoreId;
                command.SettlementDate = DateTime.UtcNow;
                command.SaleItems = sale.SaleItems.Select(x => new SaleItem(0, x.ProductId, x.Quantity, 0, x.DiscountInPercentage ?? 0, ESaleItemSituation.AcquiredOnPurchase));

                for (int i = 1; i <= sale.NumberOfInstallments; i++)
                {
                    command.Installments.Add(new Installment(number: i, value: 0, maturityDate: DateTime.UtcNow.AddMonths(i), situation: EInstallmentSituation.Open, saleId: 0));
                }

                return await _createSaleInInstallmentsHandler.HandleAsync(command);
            }
            else if (sale.PaymentType == ESalePaymentType.OnCredit)
            {
                CreateSaleOnCreditCommand command = new();
                command.CustomerId = sale.CustomerId;
                command.FormOfPayment = EFormOfPayment.Cash;
                command.DiscountInPercentage = 0; // TODO implement discount in total sale
                command.DownPayment = sale.DownPayment ?? 0;
                command.SaleDate = DateTime.UtcNow;
                command.SellerId = sale.SellerId;
                command.StoreId = sale.StoreId;
                command.SettlementDate = null;
                command.SaleItems = sale.SaleItems.Select(x => new SaleItem(0, x.ProductId, x.Quantity, 0, x.DiscountInPercentage ?? 0, ESaleItemSituation.AcquiredOnPurchase));

                return await _createSaleOnCreditHandler.HandleAsync(command);
            }

            throw new NotImplementedException();
        }

        public async Task<IEnumerable<SaleViewModel>> GetAllSalesAsync()
        {
            var result = await _getAllSalesQueryHandler.HandleAsync(new GetAllSalesQuery());

            if (!result.Success)
                throw new ApplicationException(result.Errors!.GetAsSingleMessage());

            List<SaleViewModel> salesViewModel = new();

            foreach (var sale in result.Sales)
            {
                salesViewModel.Add(GetViewModelFromDTO(sale));
            }

            return salesViewModel;
        }

        public async Task<PaginatedListViewModel<SaleViewModel>> GetAllSalesPaginatedAsync(int currentPage, int pageSize)
        {
            GetAllSalesQuery query = new();
            query.CurrentPage = currentPage;
            query.PageSize = pageSize;

            var result = await _getAllSalesQueryHandler.HandleAsync(query);

            if (!result.Success)
                throw new ApplicationException(result.Errors!.GetAsSingleMessage());

            List<SaleViewModel> salesViewModel = new();

            foreach (var sale in result.Sales)
            {
                salesViewModel.Add(GetViewModelFromDTO(sale));
            }

            PaginatedListViewModel<SaleViewModel> paginatedList = new();
            paginatedList.CurrentPage = currentPage;
            paginatedList.PageSize = pageSize;
            paginatedList.TotalItemsCount = result.SalesCount;
            paginatedList.TotalPages = PaginationManager.CalculateTotalPages(result.SalesCount, pageSize);
            paginatedList.Items = salesViewModel;

            return paginatedList;
        }

        public async Task<IEnumerable<SaleViewModel>> GetAllSalesByCustomerAsync(int customerId)
        {
            GetAllSalesByCustomerIdQuery query = new();
            query.CustomerId = customerId;

            var result = await _getAllSalesByCustomerIdQueryHandler.HandleAsync(query);

            if (!result.Success)
                throw new ApplicationException(result.Errors!.GetAsSingleMessage());

            List<SaleViewModel> salesViewModel = new();

            foreach (var sale in result.Sales)
            {
                salesViewModel.Add(GetViewModelFromDTO(sale));
            }

            return salesViewModel;
        }

        public async Task<PaginatedListViewModel<SaleViewModel>> GetAllSalesByCustomerPaginatedAsync(int customerId, int currentPage, int pageSize)
        {
            GetAllSalesByCustomerIdQuery query = new();
            query.CustomerId = customerId;
            query.CurrentPage = currentPage;
            query.PageSize = pageSize;

            var result = await _getAllSalesByCustomerIdQueryHandler.HandleAsync(query);

            if (!result.Success)
                throw new ApplicationException(result.Errors!.GetAsSingleMessage());

            List<SaleViewModel> salesViewModel = new();

            foreach (var sale in result.Sales)
            {
                salesViewModel.Add(GetViewModelFromDTO(sale));
            }

            PaginatedListViewModel<SaleViewModel> paginatedList = new();
            paginatedList.CurrentPage = currentPage;
            paginatedList.PageSize = pageSize;
            paginatedList.TotalItemsCount = result.SalesCount;
            paginatedList.TotalPages = PaginationManager.CalculateTotalPages(result.SalesCount, pageSize);
            paginatedList.Items = salesViewModel;

            return paginatedList;
        }

        public async Task<ICommandResult> PayOffSaleAsync(int saleId)
        {
            PayOffSaleCommand command = new();
            command.SaleId = saleId;

            return await _payOffSaleHandler.HandleAsync(command);
        }

        public async Task<SaleViewModel> GetSaleAsync(int saleId)
        {
            GetSaleByIdQuery query = new();
            query.SaleId = saleId;

            var result = await _getSaleByIdQueryHandler.HandleAsync(query);

            if (!result.Success)
                throw new ApplicationException(result.Errors!.GetAsSingleMessage());

            SaleViewModel saleViewModel = GetViewModelFromDTO(result.Sale!);

            // Get Items Product's Name
            List<SaleItemViewModel> saleItems = saleViewModel.SaleItems.ToList();
            foreach (var item in saleItems)
            {
                // TODO Double check if that's the only way to fetch the product name
                var product = await _productService.GetProductAsync(item.ProductId);
                item.ProductName = product.Name!;
            }
            saleViewModel.SaleItems = saleItems;

            return saleViewModel;
        }

        public async Task<ICommandResult> InformPaymentAsync(int saleId, decimal amountToInform)
        {
            InformSalePaymentCommand command = new();
            command.SaleId = saleId;
            command.AmountToInform = amountToInform;

            return await _informSalePaymentHandler.HandleAsync(command);
        }

        #region Private methods
        private SaleViewModel GetViewModelFromDTO(SaleBaseDTO sale)
        {
            return new SaleViewModel
            {
                Id = sale.Id,
                CustomerId = sale.CustomerId,
                CustomerName = sale.CustomerName,
                StoreName = sale.StoreName,
                SellerId = sale.SellerId,
                SaleItems = GetSaleItemsViewModelFromSaleItems(sale.SaleItems),
                PaymentType = GetPaymentTypeFromSaleDTO(sale),
                NumberOfInstallments = GetNumberOfInstallmentsFromSale(sale),
                DownPayment = sale.DownPayment,
                SaleTotalFormatted = sale.Total.ToString("C", FormatProviderManager.CultureInfo),
                SaleDate = sale.SaleDate,
                Status = sale.Situation,
                TotalPaid = sale.TotalPaid,
                TotalToPay = sale.TotalToPay
            };
        }

        private IEnumerable<SaleItemViewModel> GetSaleItemsViewModelFromSaleItems(IEnumerable<SaleItemDTO> saleItems)
        {
            ArgumentNullException.ThrowIfNull(saleItems);
            return saleItems.Select(x => new SaleItemViewModel()
            {
                ProductId = x.ProductId,
                Price = x.Price,
                Quantity = x.Amount,
                DiscountInPercentage = x.DiscountInPercentage
            });
        }

        private ESalePaymentType GetPaymentTypeFromSaleDTO(SaleBaseDTO sale)
        {
            ArgumentNullException.ThrowIfNull(sale);

            if (sale is SaleInCashDTO)
                return ESalePaymentType.Cash;
            else if (sale is SaleInInstallmentsDTO)
                return ESalePaymentType.InStallments;
            else
                return ESalePaymentType.OnCredit;
        }

        private int GetNumberOfInstallmentsFromSale(SaleBaseDTO sale)
        {
            ArgumentNullException.ThrowIfNull(sale);

            if (sale is not SaleInInstallmentsDTO)
                return 0;

            return (sale as SaleInInstallmentsDTO)!.NumberOfInstallments;
        }
        #endregion Private methods
    }
}
