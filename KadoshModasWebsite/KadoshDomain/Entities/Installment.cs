using Flunt.Notifications;
using Flunt.Validations;
using KadoshDomain.Enums;
using KadoshShared.Entities;

namespace KadoshDomain.Entities
{
    public class Installment : Entity
    {
        #region Constructor
        public Installment(int number, decimal value, DateTime maturityDate, EInstallmentSituation situation)
        {
            Number = number;
            Value = value;
            MaturityDate = maturityDate;
            Situation = situation;

            ValidateInstallment();
        }

        public Installment(int number, decimal value, DateTime maturityDate, EInstallmentSituation situation, DateTime settlementDate) : this(number, value, maturityDate, situation)
        {
            SettlementDate = settlementDate;
        }


        #endregion Constructor

        #region Properties
        public int Number { get; private set; }
        public decimal Value { get; private set; }
        public DateTime MaturityDate { get; private set; }
        public EInstallmentSituation Situation { get; private set; }
        public DateTime? SettlementDate { get; private set; }
        #endregion Properties

        #region Methods
        private void ValidateInstallment()
        {
            AddNotifications(new Contract<Notification>()
                .Requires()
                .IsGreaterThan(Number, 0, nameof(Number), "O número da parcela precisa ser maior do que 0!")
                .IsGreaterThan(Value, 0, nameof(Value), "O valor da parcela precisa ser maior do que 0!")
            );
        }
        #endregion Methods
    }
}