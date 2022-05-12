using KadoshShared.Enums;
using KadoshShared.Enums.CustomAtributes;

namespace KadoshDomain.Enums
{
    public enum ECustomerPostingLegacyType
    {
        [BookEntryType(EBookEntryType.Credit)]
        CashPurchase = 0,

        [BookEntryType(EBookEntryType.Credit)]
        Payment = 1,

        [BookEntryType(EBookEntryType.Credit)]
        DownPayment = 2,

        [BookEntryType(EBookEntryType.Debit)]
        ReversalPayment = 3,

        [BookEntryType(EBookEntryType.Debit)]
        SaleItemReturn = 4
    }
}