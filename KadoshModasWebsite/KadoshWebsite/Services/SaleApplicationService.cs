using KadoshDomain.Commands;
using KadoshDomain.Entities;
using KadoshDomain.Enums;
using KadoshDomain.Handlers;
using KadoshDomain.Repositories;
using KadoshShared.Commands;
using KadoshShared.Repositories;
using KadoshWebsite.Infrastructure;
using KadoshWebsite.Models;
using KadoshWebsite.Models.Enums;
using KadoshWebsite.Services.Interfaces;
using System.Globalization;

namespace KadoshWebsite.Services
{
    public class SaleApplicationService : ISaleApplicationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ISaleInCashRepository _saleInCashRepository;
        private readonly ISaleInInstallmentsRepository _saleInInstallmentsRepository;
        private readonly ISaleOnCreditRepository _saleOnCreditRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly IProductRepository _productRepository;
        private readonly IUserRepository _userRepository;
        private readonly ISaleItemRepository _saleItemRepository;
        private readonly ICustomerPostingRepository _customerPostingRepository;
        private readonly IInstallmentRepository _installmentRepository;
        private readonly ISaleRepository _saleRepository;
        private readonly IStoreRepository _storeRepository;

        public SaleApplicationService(
            IUnitOfWork unitOfWork,
            ISaleInCashRepository saleInCashRepository,
            ISaleInInstallmentsRepository saleInInstallmentsRepository,
            ISaleOnCreditRepository saleOnCreditRepository,
            ICustomerRepository customerRepository,
            IProductRepository productRepository,
            IUserRepository userRepository,
            IStoreRepository storeRepository,
            ISaleItemRepository saleItemRepository,
            ICustomerPostingRepository customerPostingRepository,
            IInstallmentRepository installmentRepository,
            ISaleRepository saleRepository)
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
            _saleRepository = saleRepository;
            _storeRepository = storeRepository;
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

                SaleHandler handler = new(
                    _unitOfWork,
                    _saleRepository,
                    _saleInCashRepository,
                    _saleInInstallmentsRepository,
                    _saleOnCreditRepository,
                    _customerRepository,
                    _productRepository,
                    _userRepository,
                    _saleItemRepository,
                    _customerPostingRepository,
                    _installmentRepository,
                    _storeRepository
                );
                return await handler.HandleAsync(command);
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

                SaleHandler handler = new(
                    _unitOfWork,
                    _saleRepository,
                    _saleInCashRepository,
                    _saleInInstallmentsRepository,
                    _saleOnCreditRepository,
                    _customerRepository,
                    _productRepository,
                    _userRepository,
                    _saleItemRepository,
                    _customerPostingRepository,
                    _installmentRepository,
                    _storeRepository
                );
                return await handler.HandleAsync(command);
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

                SaleHandler handler = new(
                    _unitOfWork,
                    _saleRepository,
                    _saleInCashRepository,
                    _saleInInstallmentsRepository,
                    _saleOnCreditRepository,
                    _customerRepository,
                    _productRepository,
                    _userRepository,
                    _saleItemRepository,
                    _customerPostingRepository,
                    _installmentRepository,
                    _storeRepository
                );
                return await handler.HandleAsync(command);
            }

            throw new NotImplementedException();
        }

        public async Task<IEnumerable<SaleViewModel>> GetAllSalesAsync()
        {
            var sales = await _saleRepository.ReadAllIncludingCustomerAsync();
            List<SaleViewModel> salesViewModel = new();

            foreach (var sale in sales)
            {
                salesViewModel.Add(GetViewModelFromEntity(sale));
            }

            return salesViewModel;
        }

        public async Task<IEnumerable<SaleViewModel>> GetAllSalesByCustomerAsync(int customerId)
        {
            var sales = await _saleRepository.ReadAllFromCustomerAsync(customerId);
            List<SaleViewModel> salesViewModel = new();

            foreach (var sale in sales)
            {
                salesViewModel.Add(GetViewModelFromEntity(sale));
            }

            return salesViewModel;
        }

        public async Task<ICommandResult> PayOffSaleAsync(int saleId)
        {
            PayOffSaleCommand command = new();
            command.SaleId = saleId;

            SaleHandler handler = new(
                    _unitOfWork,
                    _saleRepository,
                    _saleInCashRepository,
                    _saleInInstallmentsRepository,
                    _saleOnCreditRepository,
                    _customerRepository,
                    _productRepository,
                    _userRepository,
                    _saleItemRepository,
                    _customerPostingRepository,
                    _installmentRepository,
                    _storeRepository
                );
            return await handler.HandleAsync(command);
        }

        #region Private methods
        private SaleViewModel GetViewModelFromEntity(Sale sale)
        {
            return new SaleViewModel
            {
                Id = sale.Id,
                CustomerId = sale.CustomerId,
                CustomerName = sale.Customer?.Name,
                SellerId = sale.SellerId,
                SaleItems = GetSaleItemsViewModelFromSaleItems(sale.SaleItems),
                PaymentType = GetPaymentTypeFromSale(sale),
                NumberOfInstallments = GetNumberOfInstallmentsFromSale(sale),
                DownPayment = sale.DownPayment,
                SaleTotalFormatted = sale.Total.ToString("C", FormatProviderManager.CultureInfo),
                SaleDate = sale.SaleDate,
                Status = sale.Situation
            };
        }

        private IEnumerable<SaleItemViewModel> GetSaleItemsViewModelFromSaleItems(IEnumerable<SaleItem> saleItems)
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

        private ESalePaymentType GetPaymentTypeFromSale(Sale sale)
        {
            ArgumentNullException.ThrowIfNull(sale);

            if (sale is SaleInCash)
                return ESalePaymentType.Cash;
            else if (sale is SaleInInstallments)
                return ESalePaymentType.InStallments;
            else
                return ESalePaymentType.OnCredit;
        }

        private int GetNumberOfInstallmentsFromSale(Sale sale)
        {
            ArgumentNullException.ThrowIfNull(sale);

            if (sale is not SaleInInstallments)
                return 0;

            return (sale as SaleInInstallments).NumberOfInstallments;
        }
        #endregion Private methods
    }
}
