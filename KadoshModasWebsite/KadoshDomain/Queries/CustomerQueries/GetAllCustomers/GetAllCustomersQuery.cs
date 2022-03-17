using Flunt.Notifications;
using Flunt.Validations;
using KadoshShared.Constants.ValidationErrors;
using KadoshShared.Queries;

namespace KadoshDomain.Queries.CustomerQueries.GetAllCustomers
{
    public class GetAllCustomersQuery : Notifiable<Notification>, IQueryRequest
    {
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }

        public void Validate()
        {
            AddNotifications(new Contract<Notification>()
                .Requires()
                .IsGreaterOrEqualsThan(CurrentPage, 0, nameof(CurrentPage), CustomerValidationsErrors.QUERY_CURRENT_PAGE_LOWER_THAN_ZERO)
                .IsGreaterOrEqualsThan(PageSize, 0, nameof(PageSize), CustomerValidationsErrors.QUERY_PAGE_SIZE_LOWER_THAN_ZERO)
            );
        }
    }
}
