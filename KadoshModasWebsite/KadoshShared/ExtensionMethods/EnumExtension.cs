using System.ComponentModel;

namespace KadoshShared.ExtensionMethods
{
    public static class EnumExtension
    {
        public static string ToDescription<T>(this T enumValue) where T : struct, IConvertible
        {
            if (!typeof(T).IsEnum)
                return string.Empty;

            var description = enumValue.ToString();

            if (string.IsNullOrEmpty(description))
                return string.Empty;

            var fieldInfo = enumValue.GetType().GetField(description);

            if (fieldInfo != null)
            {
                var attrs = fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), true);
                if (attrs != null && attrs.Length > 0)
                {
                    description = ((DescriptionAttribute)attrs[0]).Description;
                }
            }

            return description;
        }
    }
}
