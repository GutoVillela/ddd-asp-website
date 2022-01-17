using Flunt.Notifications;
using Flunt.Validations;
using KadoshDomain.Enums;
using KadoshShared.ValueObjects;
using System.ComponentModel.DataAnnotations;

namespace KadoshDomain.ValueObjects
{
    public class Phone : ValueObject
    {
        public Phone(string areaCode, string number, EPhoneType type, string talkTo)
        {
            AreaCode = areaCode;
            Number = number;
            Type = type;
            TalkTo = talkTo;

            AddNotifications(new Contract<Notification>()
                .Requires()
                .IsTrue(ValidateNumber(), nameof(Number), $"Telefone inválido.")
            );
        }

        [Required]
        [MaxLength(3)]
        public string AreaCode { get; private set; }

        [Required]
        [MaxLength(10)]
        public string Number { get; private set; }

        [Required]
        public EPhoneType Type { get; private set; }

        public string? TalkTo { get; private set; }

        private bool ValidateNumber()
        {
            return AreaCode.Length >= 2 && Number.Length >= 8;
        }
    }
}
