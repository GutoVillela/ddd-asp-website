using Flunt.Notifications;
using Flunt.Validations;
using KadoshShared.Commands;
using KadoshShared.Constants.ValidationErrors;

namespace KadoshDomain.Commands.SaleCommands.InformPayment
{
    public class InformSalePaymentCommand : Notifiable<Notification>, ICommand
    {
        public int? SaleId { get; set; }
        public decimal? AmountToInform { get; set; }

        public void Validate()
        {
            AddNotifications(new Contract<Notification>()
                .Requires()
                .IsNotNull(SaleId, nameof(SaleId), SaleValidationsErrors.NULL_SALE_ID)
                .IsNotNull(AmountToInform, nameof(AmountToInform), SaleValidationsErrors.NULL_AMOUNT_TO_INFORM)
            );

            if (AmountToInform.HasValue && AmountToInform.Value <= 0)
                AddNotification(nameof(AmountToInform), SaleValidationsErrors.AMOUNT_TO_INFORM_LESS_OR_EQUALS_ZERO);

        }
    }
}
