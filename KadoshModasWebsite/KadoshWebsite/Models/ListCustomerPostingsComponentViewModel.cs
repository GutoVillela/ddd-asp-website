namespace KadoshWebsite.Models
{
    public class ListCustomerPostingsComponentViewModel
    {
        public int? FilterByCustumerId { get; set; }
        public int? FilterBySaleId { get; set; }
        public DateOnly? FilterByDate { get; set; }
        public int? FilterByStore { get; set; }
        public bool ShowTotal { get; set; }
    }
}
