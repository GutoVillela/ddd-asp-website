using Flunt.Notifications;
using Flunt.Validations;
using KadoshShared.Constants.ValidationErrors;
using KadoshShared.Queries;

namespace KadoshDomain.Queries.UserQueries.GetUserById
{
    public class GetUserByIdQuery : Notifiable<Notification>, IQueryRequest
    {
        public int? UserId { get; set; }

        public void Validate()
        {
            AddNotifications(new Contract<Notification>()
                .Requires()
                .IsNotNull(UserId, nameof(UserId), UserValidationsErrors.INVALID_USER_ID)
            );
        }
    }
}
