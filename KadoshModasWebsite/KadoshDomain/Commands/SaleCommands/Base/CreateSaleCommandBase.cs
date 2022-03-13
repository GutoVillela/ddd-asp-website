using Flunt.Notifications;
using Flunt.Validations;
using KadoshDomain.Entities;
using KadoshDomain.Enums;
using KadoshShared.Commands;
using KadoshShared.Constants.ValidationErrors;

namespace KadoshDomain.Commands.SaleCommands.Base
{
    public abstract class CreateSaleCommandBase : Notifiable<Notification>, ICommand
    {
        public int? CustomerId { get; set; }
        public EFormOfPayment? FormOfPayment { get; set; }
        public decimal DiscountInPercentage { get; set; } = 0;
        public decimal DownPayment { get; set; } = 0;
        public DateTime? SaleDate { get; set; }
        public int? SellerId { get; set; }
        public int? StoreId { get; set; }
        public DateTime? SettlementDate { get; set; }
        public IEnumerable<SaleItem> SaleItems { get; set; } = new List<SaleItem>();// TODO check if it's possible not to use Entity here

        public virtual void Validate()
        {
            AddNotifications(new Contract<Notification>()
                .Requires()
                .IsNotNull(CustomerId, nameof(CustomerId), SaleValidationsErrors.INVALID_SALE_CUSTOMER)
                .IsNotNull(FormOfPayment, nameof(FormOfPayment), SaleValidationsErrors.INVALID_SALE_FORM_OF_PAYMENT)
                .IsBetween(DiscountInPercentage, 0, 100, nameof(DiscountInPercentage), SaleValidationsErrors.SALE_DISCOUNT_OUT_OF_RANGE)
                .IsGreaterOrEqualsThan(DownPayment, 0, nameof(DownPayment), SaleValidationsErrors.DOWN_PAYMENT_LOWER_THAN_ZERO)
                .IsNotNull(SaleDate, nameof(SaleDate), SaleValidationsErrors.NULL_SALE_DATE)
                .IsNotNull(SellerId, nameof(SellerId), SaleValidationsErrors.NULL_SELLER_ID)
                .IsNotNull(StoreId, nameof(StoreId), SaleValidationsErrors.NULL_STORE_ID)
                .IsNotEmpty(SaleItems, nameof(SaleItems), SaleValidationsErrors.EMPTY_SALE_LIST_ITEMS)
            );

            if (SaleDate.HasValue && SettlementDate.HasValue)
                if (SettlementDate.Value < SaleDate.Value)
                    AddNotification(nameof(SettlementDate), SaleValidationsErrors.SETTLEMENT_DATE_LOWER_THAN_SALE_DATE);
        }
    }
}
