namespace KadoshWebsite.Infrastructure
{
    public static class PaginationManager
    {
        public const int PAGE_SIZE = 10;

        public const int MAX_PAGES_PER_VIEW = 4;

        public static int CalculateTotalPages(int totalItems, int pageSize)
        {
            return (int) Math.Ceiling((double) totalItems / pageSize);
        }
    }
}
