using Flunt.Notifications;
using Flunt.Validations;
using KadoshShared.Constants.ValidationErrors;
using KadoshShared.Queries;

namespace KadoshDomain.Queries.CategoryQueries.GetCategoryById
{
    public class GetCategoryByIdQuery : Notifiable<Notification>, IQueryRequest
    {
        public int? CategoryId { get; set; }

        public void Validate()
        {
            AddNotifications(new Contract<Notification>()
                .Requires()
                .IsNotNull(CategoryId, nameof(CategoryId), CategoryValidationsErrors.INVALID_CATEGORY_ID)
            );
        }
    }
}
