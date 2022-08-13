using Flunt.Notifications;
using Flunt.Validations;
using KadoshShared.Constants.ValidationErrors;
using KadoshShared.Queries;

namespace KadoshDomain.Queries.CustomerQueries.GetCustomersByName
{
    public class GetCustomersByNameQuery : Notifiable<Notification>, IQueryRequest
    {
        public string? CustomerName { get; set; }

        /// <summary>
        /// If the value is zero the query will fetch all customers.
        /// </summary>
        public int CurrentPage { get; set; }

        /// <summary>
        /// If the value is zero the query will fetch all customers.
        /// </summary>
        public int PageSize { get; set; }

        public bool IncludeInactives { get; set; }

        public void Validate()
        {
            AddNotifications(new Contract<Notification>()
                .Requires()
                .IsNotNullOrEmpty(CustomerName, nameof(CustomerName), CustomerValidationsErrors.INVALID_CUSTOMER_NAME)
                .IsGreaterOrEqualsThan(CurrentPage, 0, nameof(CurrentPage), CustomerValidationsErrors.QUERY_CURRENT_PAGE_LOWER_THAN_ZERO)
                .IsGreaterOrEqualsThan(PageSize, 0, nameof(PageSize), CustomerValidationsErrors.QUERY_PAGE_SIZE_LOWER_THAN_ZERO)
            );
        }
    }
}
