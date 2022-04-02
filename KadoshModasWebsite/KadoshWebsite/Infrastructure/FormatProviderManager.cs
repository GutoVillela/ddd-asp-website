using System.Globalization;

namespace KadoshWebsite.Infrastructure
{
    public static class FormatProviderManager
    {
        public static readonly CultureInfo CultureInfo = CultureInfo.GetCultureInfo("pt-BR");
        public static readonly string DateTimeFormat = "dd/MM/yyyy";

        // TODO Move this to AppSettings file
        public static readonly TimeZoneInfo TimeZone = TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time");
    }
}
