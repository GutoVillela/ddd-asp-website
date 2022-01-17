using Flunt.Notifications;
using Flunt.Validations;
using KadoshDomain.Enums;
using KadoshShared.Entities;
using System.ComponentModel.DataAnnotations;

namespace KadoshDomain.Entities
{
    public class Installment : Entity
    {
        public Installment(int number, decimal value, DateTime maturityDate, EInstallmentSituation situation, int saleId)
        {
            Number = number;
            Value = value;
            MaturityDate = maturityDate;
            Situation = situation;
            SaleId = saleId;

            ValidateInstallment();
        }

        public Installment(int number, decimal value, DateTime maturityDate, EInstallmentSituation situation, int saleId, DateTime? settlementDate) : this(number, value, maturityDate, situation, saleId)
        {
            SettlementDate = settlementDate;
        }

        public Installment(int number, decimal value, DateTime maturityDate, EInstallmentSituation situation, int saleId, DateTime? settlementDate, SaleInInstallments? sale) : this(number, value, maturityDate, situation, saleId, settlementDate)
        {
            Sale = sale;
        }

        [Required]
        public int Number { get; private set; }

        [Required]
        public decimal Value { get; private set; }

        [Required]
        public DateTime MaturityDate { get; private set; }

        [Required]
        public EInstallmentSituation Situation { get; private set; }
        public DateTime? SettlementDate { get; private set; }

        [Required]
        public int SaleId { get; private set; }

        public SaleInInstallments? Sale { get; private set; }

        private void ValidateInstallment()
        {
            AddNotifications(new Contract<Notification>()
                .Requires()
                .IsGreaterThan(Number, 0, nameof(Number), "O número da parcela precisa ser maior do que 0!")
                .IsGreaterThan(Value, 0, nameof(Value), "O valor da parcela precisa ser maior do que 0!")
            );
        }
    }
}