using KadoshShared.ValueObjects;

namespace KadoshShared.ExtensionMethods
{
    public static class ErrorExtension
    {
        public static string GetAsSingleMessage(this IEnumerable<Error> errors)
        {
            if(errors is null || !errors.Any())
                return string.Empty;

            return string.Join(". ", errors.Select(x => x.Message + ". "));
        }
    }
}
