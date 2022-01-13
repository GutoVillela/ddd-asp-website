using KadoshDomain.Enums;
using System.Linq;

namespace KadoshDomain.ExtensionMethods
{
    public static class ECustomerPostingTypeExtension
    {
        public static bool IsEntryType(this ECustomerPostingType customerPostingType)
        {
            return customerPostingType == ECustomerPostingType.CashPurchase || customerPostingType == ECustomerPostingType.Payment || customerPostingType == ECustomerPostingType.DownPayment;
        }
    }
}
