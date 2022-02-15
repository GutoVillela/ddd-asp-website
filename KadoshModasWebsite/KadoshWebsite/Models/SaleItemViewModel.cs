namespace KadoshWebsite.Models
{
    public class SaleItemViewModel
    {
        public int ProductId { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public decimal? DiscountInPercentage { get; set; } = 0;

    }
}
