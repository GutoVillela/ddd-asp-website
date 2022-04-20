namespace KadoshWebsite.Models
{
    public class SaleReceiptViewModel
    {
        public SaleViewModel Sale { get; set; }

        public IEnumerable<CustomerPostingViewModel> Postings { get; set; }
    }
}
