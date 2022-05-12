using KadoshDomain.Entities;
using KadoshDomain.Enums;

namespace KadoshDomain.LegacyEntities
{
    public class InstallmentLegacy
    {
        public int Number { get; set; }
        public decimal Value { get; set; }
        public decimal Discount { get; set; }
        public EInstallmentLegacySituation Situation { get; set; }
        public DateTime MaturityDate { get; set; }
        public DateTime? SettlementDate { get; set; }
        public int SaleId { get; set; }

        public static implicit operator Installment(InstallmentLegacy legacyInstallment)
        {
            Installment installment = new(
                number: legacyInstallment.Number, 
                value: legacyInstallment.Value, 
                maturityDate: legacyInstallment.MaturityDate, 
                situation: GetInstallmentSituationFromLegacy(legacyInstallment.Situation), 
                saleId: legacyInstallment.SaleId, 
                settlementDate: legacyInstallment.SettlementDate
                );

            return installment;
        }

        private static EInstallmentSituation GetInstallmentSituationFromLegacy(EInstallmentLegacySituation legacySituation)
        {
            switch (legacySituation)
            {
                case EInstallmentLegacySituation.Open:
                    return EInstallmentSituation.Open;

                case EInstallmentLegacySituation.PaidWithoutDelay:
                    return EInstallmentSituation.PaidWithoutDelay;

                case EInstallmentLegacySituation.PaidWithDelay:
                    return EInstallmentSituation.PaidWithDelay;

                case EInstallmentLegacySituation.Canceled:
                    return EInstallmentSituation.Canceled;

                default:
                    return EInstallmentSituation.Open;
            }
        }
    }
}