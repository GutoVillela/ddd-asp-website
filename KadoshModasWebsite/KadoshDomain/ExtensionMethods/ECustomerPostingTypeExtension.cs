using KadoshDomain.Enums;
using KadoshShared.Enums;
using KadoshShared.Enums.CustomAtributes;
using System.Linq.Expressions;
using System.Reflection;

namespace KadoshDomain.ExtensionMethods
{
    public static class ECustomerPostingTypeExtension
    {
        public static bool IsCreditType(this ECustomerPostingType customerPostingType)
        {
            return customerPostingType.GetBookEntryType() == EBookEntryType.Credit;
        }

        private static EBookEntryType GetBookEntryType(this ECustomerPostingType obj)
        {
            FieldInfo? fieldInfo = obj.GetType().GetField(obj.ToString());

            if (fieldInfo is null)
                throw new ApplicationException("Field info está nulo");

            BookEntryTypeAttribute? attr = (BookEntryTypeAttribute?)fieldInfo.GetCustomAttribute(typeof(BookEntryTypeAttribute));

            if (attr is null)
                throw new ApplicationException("Lançamento sem tipo de lançamento");

            return attr.BookEntryType;
        }
    }
}
