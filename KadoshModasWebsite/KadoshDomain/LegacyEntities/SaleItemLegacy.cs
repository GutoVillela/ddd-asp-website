using KadoshDomain.Entities;
using KadoshDomain.Enums;

namespace KadoshDomain.LegacyEntities
{
    public class SaleItemLegacy
    {
        public int ProductId { get; set; }
        public int Amount { get; set; }
        public decimal Price { get; set; }
        public decimal Discount { get; set; }
        public ESaleItemLegacySituation Situation { get; set; }
        public int SaleId { get; set; }

        public static implicit operator SaleItem(SaleItemLegacy legacySaleItem)
        {
            SaleItem saleItem = new(
                saleId: legacySaleItem.SaleId,
                productId: legacySaleItem.ProductId,
                amount: legacySaleItem.Amount,
                price: legacySaleItem.Price,
                discountInPercentage: legacySaleItem.Discount,
                situation: GetSaleItemSituationFromLegacy(legacySaleItem.Situation));

            return saleItem;
        }

        private static ESaleItemSituation GetSaleItemSituationFromLegacy(ESaleItemLegacySituation legacySituation)
        {
            switch(legacySituation)
            {
                case ESaleItemLegacySituation.AcquiredOnPurchase:
                    return ESaleItemSituation.AcquiredOnPurchase;

                case ESaleItemLegacySituation.AcquiredInExchange:
                    return ESaleItemSituation.AcquiredInExchange;

                case ESaleItemLegacySituation.Canceled:
                    return ESaleItemSituation.Canceled;

                case ESaleItemLegacySituation.Exchanged:
                    return ESaleItemSituation.Exchanged;

                default:
                    return ESaleItemSituation.AcquiredOnPurchase;
            }
        }
    }
}