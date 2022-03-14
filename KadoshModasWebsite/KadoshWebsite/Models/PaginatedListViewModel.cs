using KadoshWebsite.Infrastructure;

namespace KadoshWebsite.Models
{
    public class PaginatedListViewModel<T> where T : class, new()
    {
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int PageSize { get; set; }
        public int TotalItemsCount { get; set; }
        public int StartPage 
        { 
            get 
            {
                int halfOfMaxPagesPerView = (int) Math.Floor((double)PaginationManager.MAX_PAGES_PER_VIEW / 2);
                int startPage = CurrentPage - halfOfMaxPagesPerView;
                int endPageWithNoAdjustment = CurrentPage + halfOfMaxPagesPerView;

                if(endPageWithNoAdjustment > TotalPages)
                    startPage += TotalPages - endPageWithNoAdjustment;

                if (startPage <= 0)
                    return 1;

                return startPage; 
            } 
        }

        public int EndPage
        {
            get
            {
                int halfOfMaxPagesPerView = (int)Math.Floor((double)PaginationManager.MAX_PAGES_PER_VIEW / 2);
                int endPage = CurrentPage + halfOfMaxPagesPerView;
                int startPageWithNoAdjustment = CurrentPage - halfOfMaxPagesPerView;

                if (startPageWithNoAdjustment < 1)
                    endPage += Math.Abs(startPageWithNoAdjustment) + 1;

                if (endPage > TotalPages)
                    return TotalPages;

                return endPage;
            }
        }
        public IEnumerable<T> Items { get; set; } = new List<T>();
    }
}
