using Flunt.Notifications;
using Flunt.Validations;
using KadoshShared.Constants.ValidationErrors;
using KadoshShared.Queries;

namespace KadoshDomain.Queries.CustomerPostingQueries.GetAllPostingsFromSale
{
    public class GetAllPostingsFromSaleQuery : Notifiable<Notification>, IQueryRequest
    {
        public int? SaleId { get; set; }

        /// <summary>
        /// If the value is zero the query will fetch all customer postings.
        /// </summary>
        public int CurrentPage { get; set; }

        /// <summary>
        /// If the value is zero the query will fetch all customer postings.
        /// </summary>
        public int PageSize { get; set; }

        public void Validate()
        {
            AddNotifications(new Contract<Notification>()
                .Requires()
                .IsNotNull(SaleId, nameof(SaleId), CustomerPostingValidationsErrors.NULL_SALE_ID)
                .IsGreaterOrEqualsThan(CurrentPage, 0, nameof(CurrentPage), CustomerPostingValidationsErrors.QUERY_CURRENT_PAGE_LOWER_THAN_ZERO)
                .IsGreaterOrEqualsThan(PageSize, 0, nameof(PageSize), CustomerPostingValidationsErrors.QUERY_PAGE_SIZE_LOWER_THAN_ZERO)
            );
        }
    }
}
