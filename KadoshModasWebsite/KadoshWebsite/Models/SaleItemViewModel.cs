using KadoshWebsite.Infrastructure;

namespace KadoshWebsite.Models
{
    public class SaleItemViewModel
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public decimal? DiscountInPercentage { get; set; } = 0;
        public string SubtotalFormated { get => (Price * Quantity).ToString("C", FormatProviderManager.CultureInfo); }

    }
}
