using Flunt.Notifications;
using Flunt.Validations;
using KadoshDomain.Enums;
using KadoshShared.ValueObjects;

namespace KadoshDomain.ValueObjects
{
    public class Phone : ValueObject
    {
        #region Constructor
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
        #endregion Constructor

        #region Properties

        public string AreaCode { get; private set; }
        public string Number { get; private set; }
        public EPhoneType Type { get; private set; }
        public string TalkTo { get; private set; }

        #endregion Properties

        #region Methods

        private bool ValidateNumber()
        {
            return AreaCode.Length >= 2 && Number.Length >= 8;
        }

        #endregion Methods
    }
}
