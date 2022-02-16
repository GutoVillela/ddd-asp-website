using KadoshDomain.Commands;
using KadoshDomain.Entities;
using KadoshDomain.Enums;
using KadoshDomain.Repositories;
using KadoshDomain.Services.Interfaces;
using KadoshShared.Commands;
using KadoshWebsite.Models;
using KadoshWebsite.Models.Enums;
using KadoshWebsite.Services.Interfaces;
using System.Globalization;

namespace KadoshWebsite.Services
{
    public class SaleApplicationService : ISaleApplicationService
    {
        private readonly ISaleService _saleService;
        private readonly ISaleRepository _saleRepository;

        public SaleApplicationService(ISaleService saleService, ISaleRepository saleRepository)
        {
            _saleService = saleService;
            _saleRepository = saleRepository;
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
                command.Situation = ESaleSituation.Completed;

                return await _saleService.CreateSaleInCashAsync(command);
            }
            else if(sale.PaymentType == ESalePaymentType.InStallments)
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
                command.Situation = ESaleSituation.Completed;

                for (int i = 1; i <= sale.NumberOfInstallments; i++)
                {
                    command.Installments.Add(new Installment(number: i, value: 0, maturityDate: DateTime.UtcNow.AddMonths(i), situation: EInstallmentSituation.Open, saleId: 0));
                }

                return await _saleService.CreateSaleInInstallmentsAsync(command);
            }
            else if(sale.PaymentType == ESalePaymentType.OnCredit)
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
                command.Situation = ESaleSituation.Completed;

                return await _saleService.CreateSaleOnCreditAsync(command);
            }

            throw new NotImplementedException();
        }

        public async Task<IEnumerable<SaleViewModel>> GetAllSalesAsync()
        {
            var sales = await _saleService.GetAllSalesIncludingCustomerAsync();
            List<SaleViewModel> salesViewModel = new();

            foreach (var sale in sales)
            {
                salesViewModel.Add(GetViewModelFromEntity(sale));
            }

            return salesViewModel;
        }

        public async Task<IEnumerable<SaleViewModel>> GetAllSalesByCustomerAsync(int customerId)
        {
            var sales = await _saleRepository.ReadAllFromCustomer(customerId);
            List<SaleViewModel> salesViewModel = new();

            foreach (var sale in sales)
            {
                salesViewModel.Add(GetViewModelFromEntity(sale));
            }

            return salesViewModel;
        }

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
                SaleTotalFormatted = sale.Total.ToString("C", CultureInfo.GetCultureInfo("pt-br")),
                SaleDate = sale.SaleDate
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
    }
}
