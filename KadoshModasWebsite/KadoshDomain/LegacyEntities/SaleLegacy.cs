using KadoshDomain.Entities;
using KadoshDomain.Enums;
using KadoshShared.Entities;

namespace KadoshDomain.LegacyEntities
{
    public class SaleLegacy : LegacyEntity<Sale>
    {
        public SaleLegacy(
            int customerId,
            ESaleLegacyType saleType,
            decimal discount,
            decimal downPayment,
            ESaleLegacySituation situation, 
            ELegacyFormOfPayment formOfPayment, 
            DateTime saleDate)
        {
            CustomerId = customerId;
            SaleType = saleType;
            Situation = situation;
            FormOfPayment = formOfPayment;
            Discount = discount;
            DownPayment = downPayment;
            SaleDate = saleDate;
        }

        public int CustomerId { get; set; }
        public ESaleLegacyType SaleType { get; set; }
        public decimal Discount { get; set; }
        public decimal DownPayment { get; set; } = 0;
        public ELegacyFormOfPayment FormOfPayment { get; set; }
        public ESaleLegacySituation Situation { get; set; }
        public DateTime SaleDate { get; set; }
        public decimal Paid { get; set; } = 0;
        public DateTime? SettlementDate { get; set; }
        public IEnumerable<SaleItemLegacy> SaleItems { get; set; } = new List<SaleItemLegacy>();
        public IEnumerable<InstallmentLegacy>? SaleInstallments { get; set; }

        public static implicit operator Sale(SaleLegacy legacySale)
        {
            Sale sale;
            if (legacySale.SaleType == ESaleLegacyType.InCash)
                sale = new SaleInCash(
                    customerId: legacySale.CustomerId,
                    formOfPayment: GetFormOfPaymentFromLegacy(legacySale.FormOfPayment),
                    discountInPercentage: legacySale.Discount,
                    downPayment: legacySale.DownPayment,
                    saleDate: legacySale.SaleDate,
                    saleItems: GetSaleItemsFromLegacy(legacySale.SaleItems),
                    situation: GetSaleSituationFromLegacy(legacySale.Situation),
                    sellerId: 0, storeId: 0, // At this moment both seller and store IDs were not provided
                    settlementDate: legacySale.SettlementDate.HasValue ? legacySale.SettlementDate.Value : legacySale.SaleDate
                );
            else if (legacySale.SaleType == ESaleLegacyType.InInstallments)
                sale = new SaleInInstallments(
                    customerId: legacySale.CustomerId,
                    formOfPayment: GetFormOfPaymentFromLegacy(legacySale.FormOfPayment),
                    discountInPercentage: legacySale.Discount,
                    downPayment: legacySale.DownPayment,
                    saleDate: legacySale.SaleDate,
                    situation: GetSaleSituationFromLegacy(legacySale.Situation),
                    sellerId: 0, storeId: 0, // At this moment both seller and store IDs were not provided
                    saleItems: GetSaleItemsFromLegacy(legacySale.SaleItems),
                    installments: GetSaleInstallmentsFromLegacy(legacySale.SaleInstallments ?? new List<InstallmentLegacy>()),
                    interestOnTheTotalSaleInPercentage: 0);
            else
                sale = new SaleOnCredit(
                    customerId: legacySale.CustomerId,
                    formOfPayment: GetFormOfPaymentFromLegacy(legacySale.FormOfPayment),
                    discountInPercentage: legacySale.Discount,
                    downPayment: legacySale.DownPayment, 
                    saleDate: legacySale.SaleDate,
                    situation: GetSaleSituationFromLegacy(legacySale.Situation),
                    sellerId: 0, storeId: 0, // At this moment both seller and store IDs were not provided
                    saleItems: GetSaleItemsFromLegacy(legacySale.SaleItems)
                    );

            if (!legacySale.IsActive)
                sale.Inactivate();

            return sale;
        }

        private static EFormOfPayment GetFormOfPaymentFromLegacy(ELegacyFormOfPayment legacyFormOfPayment)
        {
            switch (legacyFormOfPayment)
            {
                case ELegacyFormOfPayment.Cash:
                    return EFormOfPayment.Cash;

                case ELegacyFormOfPayment.DebitCard:
                    return EFormOfPayment.DebitCard;
                
                case ELegacyFormOfPayment.CreditCard:
                    return EFormOfPayment.CreditCard;
                
                case ELegacyFormOfPayment.Check:
                    return EFormOfPayment.Check;

                default:
                    return EFormOfPayment.Cash;
            }
        }

        private static ESaleSituation GetSaleSituationFromLegacy(ESaleLegacySituation legacySituation)
        {
            switch (legacySituation)
            {
                case ESaleLegacySituation.Open:
                    return ESaleSituation.Open;
                
                case ESaleLegacySituation.Completed:
                    return ESaleSituation.Completed;

                case ESaleLegacySituation.Canceled:
                    return ESaleSituation.Canceled;
                
                default:
                    return ESaleSituation.Open;
            }
        }

        private static IReadOnlyCollection<SaleItem> GetSaleItemsFromLegacy(IEnumerable<SaleItemLegacy> legacySaleItems)
        {
            List<SaleItem> saleItems = new();
            foreach (var item in legacySaleItems)
                saleItems.Add(item);

            return saleItems;
        }

        private static IReadOnlyCollection<Installment> GetSaleInstallmentsFromLegacy(IEnumerable<InstallmentLegacy> legacyInstallments)
        {
            List<Installment> installments = new();
            foreach (var installment in legacyInstallments)
                installments.Add(installment);

            return installments;
        }
    }
}
