using Flunt.Notifications;
using Flunt.Validations;
using KadoshShared.Commands;
using KadoshShared.Constants.ValidationErrors;

namespace KadoshDomain.Commands.CustomerCommands.InformPayment
{
    public class InformPaymentCommand : Notifiable<Notification>, ICommand
    {
        public int? CustomerId { get; set; }
        public decimal? AmountToInform { get; set; }

        public void Validate()
        {
            AddNotifications(new Contract<Notification>()
                .Requires()
                .IsNotNull(CustomerId, nameof(CustomerId), CustomerValidationsErrors.INVALID_CUSTOMER_ID)
                .IsNotNull(AmountToInform, nameof(AmountToInform), CustomerValidationsErrors.NULL_AMOUNT_TO_INFORM)
            );

            if (AmountToInform.HasValue && AmountToInform.Value <= 0)
                AddNotification(nameof(AmountToInform), CustomerValidationsErrors.AMOUNT_TO_INFORM_LESS_OR_EQUALS_ZERO);

        }
    }
}
