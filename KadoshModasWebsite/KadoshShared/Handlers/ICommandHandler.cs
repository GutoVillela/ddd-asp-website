using KadoshShared.Commands;
using KadoshShared.ValueObjects;

namespace KadoshShared.Handlers
{
    public interface ICommandHandler<T> where T : ICommand
    {
        Task<ICommandResult> HandleAsync(T command);
    }
}