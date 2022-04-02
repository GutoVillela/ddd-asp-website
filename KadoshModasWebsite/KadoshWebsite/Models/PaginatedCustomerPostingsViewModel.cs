namespace KadoshWebsite.Models
{
    public class PaginatedCustomerPostingsViewModel : PaginatedListViewModel<CustomerPostingViewModel>
    {
        public bool ShowTotal { get; set; }
        public decimal TotalCredit { get; set; }
        public decimal TotalDebit { get; set; }
    }
}
