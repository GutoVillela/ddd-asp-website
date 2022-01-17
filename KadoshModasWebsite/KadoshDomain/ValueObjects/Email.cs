using Flunt.Notifications;
using Flunt.Validations;
using KadoshShared.ValueObjects;
using System.ComponentModel.DataAnnotations;

namespace KadoshDomain.ValueObjects
{
    public class Email : ValueObject
    {
        public Email(string emailAddress)
        {
            EmailAddress = emailAddress;

            AddNotifications(new Contract<Notification>()
                .Requires()
                .IsEmail(EmailAddress, nameof(EmailAddress), "E-mail inválido")
            );
        }

        [Required]
        [MaxLength(255)]
        public string EmailAddress { get; set; }
    }
}
