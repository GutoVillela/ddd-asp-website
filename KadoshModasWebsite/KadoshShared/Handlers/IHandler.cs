using KadoshShared.Commands;

namespace KadoshShared.Handlers
{
    public interface IHandler<T> where T : ICommand
    {
        Task<ICommandResult> HandleAsync(T command);
    }
}
