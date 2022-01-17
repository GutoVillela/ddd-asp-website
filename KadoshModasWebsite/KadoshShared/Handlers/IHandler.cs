using KadoshShared.Commands;

namespace KadoshShared.Handlers
{
    public interface IHandler<T> where T : ICommand
    {
        ICommandResult Handle(T command);
    }
}
