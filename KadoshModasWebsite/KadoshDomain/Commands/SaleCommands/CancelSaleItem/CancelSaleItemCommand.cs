using Flunt.Notifications;
using Flunt.Validations;
using KadoshShared.Commands;
using KadoshShared.Constants.ValidationErrors;

namespace KadoshDomain.Commands.SaleCommands.CancelSaleItem
{
    public class CancelSaleItemCommand : Notifiable<Notification>, ICommand
    {
        public int? SaleId { get; set; }
        public int? ProductId { get; set; }
        public int? AmountToCancel { get; set; }

        public void Validate()
        {
            AddNotifications(new Contract<Notification>()
                .Requires()
                .IsNotNull(SaleId, nameof(SaleId), SaleValidationsErrors.NULL_SALE_ID)
                .IsNotNull(ProductId, nameof(ProductId), SaleValidationsErrors.NULL_PRODUCT_ID)
                .IsNotNull(AmountToCancel, nameof(AmountToCancel), SaleValidationsErrors.NULL_AMOUNT_TO_CANCEL)
            );

            if(AmountToCancel != null && AmountToCancel < 1)
                AddNotification(nameof(AmountToCancel), SaleValidationsErrors.AMOUNT_TO_CANCEL_LOWER_THAN_ONE);
            
        }
    }
}
