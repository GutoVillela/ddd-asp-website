using Flunt.Notifications;
using Flunt.Validations;
using KadoshShared.Commands;
using KadoshShared.Constants.ValidationErrors;

namespace KadoshDomain.Commands.SaleCommands.PayOffInstallment
{
    public class PayOffInstallmentCommand : Notifiable<Notification>, ICommand
    {
        public int? SaleId { get; set; }
        public int? InstallmentId { get; set; }

        public void Validate()
        {
            AddNotifications(new Contract<Notification>()
                .Requires()
                .IsNotNull(SaleId, nameof(SaleId), SaleValidationsErrors.NULL_SALE_ID)
                .IsNotNull(InstallmentId, nameof(InstallmentId), SaleValidationsErrors.NULL_INSTALLMENT_ID)
            );
        }
    }
}
