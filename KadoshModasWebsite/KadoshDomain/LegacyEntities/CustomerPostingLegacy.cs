using KadoshDomain.Entities;
using KadoshDomain.Enums;
using KadoshShared.Entities;

namespace KadoshDomain.LegacyEntities
{
    public class CustomerPostingLegacy : LegacyEntity<CustomerPosting>
    {
        public CustomerPostingLegacy(ECustomerPostingLegacyType type, decimal value, int saleId, int customerId, DateTime postingDate)
        {
            Type = type;
            Value = value;
            SaleId = saleId;
            CustomerId = customerId;
            PostingDate = postingDate;
        }

        public ECustomerPostingLegacyType Type { get; private set; }
        public decimal Value { get; private set; }
        public int SaleId { get; private set; }
        public int CustomerId { get; set; }
        public DateTime PostingDate { get; private set; }

        public static implicit operator CustomerPosting(CustomerPostingLegacy customerPostingLegacy)
        {
            CustomerPosting customerPosting = new(
                type: GetPostingTypeFromLegacy(customerPostingLegacy.Type),
                value: customerPostingLegacy.Value,
                saleId: customerPostingLegacy.SaleId,
                postingDate: customerPostingLegacy.PostingDate
                );

            return customerPosting;
        }

        private static ECustomerPostingType GetPostingTypeFromLegacy(ECustomerPostingLegacyType legacyPostingType)
        {
            switch (legacyPostingType)
            {
                case ECustomerPostingLegacyType.CashPurchase:
                    return ECustomerPostingType.CashPurchase;

                case ECustomerPostingLegacyType.Payment:
                    return ECustomerPostingType.Payment;

                case ECustomerPostingLegacyType.DownPayment:
                    return ECustomerPostingType.DownPayment;

                case ECustomerPostingLegacyType.ReversalPayment:
                    return ECustomerPostingType.ReversalPayment;

                case ECustomerPostingLegacyType.SaleItemReturn:
                    return ECustomerPostingType.SaleItemReturn;

                default:
                    return ECustomerPostingType.Payment;
            }
        }
    }
}