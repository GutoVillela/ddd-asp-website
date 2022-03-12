using Flunt.Notifications;
using KadoshShared.Commands;
using KadoshShared.Handlers;
using KadoshShared.ValueObjects;

namespace KadoshDomain.Commands.Base
{
    public abstract class HandlerBase<TCommand> : Notifiable<Notification>, IHandler<TCommand> where TCommand : ICommand
    {
        public abstract Task<ICommandResult> HandleAsync(TCommand command);

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
