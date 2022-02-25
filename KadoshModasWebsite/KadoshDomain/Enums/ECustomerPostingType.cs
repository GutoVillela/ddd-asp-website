using KadoshShared.Constants.Descriptions;
using KadoshShared.Enums;
using KadoshShared.Enums.CustomAtributes;
using System.ComponentModel;

namespace KadoshDomain.Enums
{
    public enum ECustomerPostingType
    {
        [Description(ECustomerPostingTypeDescriptions.CASH_PURCHASE_DESCRIPTION)]
        [BookEntryType(EBookEntryType.Credit)]
        CashPurchase = 0,

        [Description(ECustomerPostingTypeDescriptions.PAYMENT_DESCRIPTION)]
        [BookEntryType(EBookEntryType.Credit)]
        Payment = 1,

        [Description(ECustomerPostingTypeDescriptions.DOWN_PAYMENT_DESCRIPTION)]
        [BookEntryType(EBookEntryType.Credit)]
        DownPayment = 2,

        [Description(ECustomerPostingTypeDescriptions.REVERSAL_PAYMENT_DESCRIPTION)]
        [BookEntryType(EBookEntryType.Debit)]
        ReversalPayment = 3,

        [Description(ECustomerPostingTypeDescriptions.SALE_ITEM_RETURN_DESCRIPTION)]
        [BookEntryType(EBookEntryType.Debit)]
        SaleItemReturn = 4
    }
}
