using Flunt.Notifications;
using KadoshShared.Handlers;
using KadoshShared.Queries;
using KadoshShared.ValueObjects;

namespace KadoshDomain.Queries.Base
{
    public abstract class QueryHandlerBase <TRequest, TResult> : Notifiable<Notification>, IQueryHandler<TRequest, TResult> where TRequest : IQueryRequest where TResult : IQueryResult
    {
        public abstract Task<TResult> HandleAsync(TRequest command);

        protected virtual ICollection<Error> GetErrorsFromNotifications(int errorCode)
        {
            HashSet<Error> errors = new();
            foreach (var error in Notifications)
            {
                errors.Add(new Error(errorCode, error.Message));
            }

            return errors;
        }
    }
}
