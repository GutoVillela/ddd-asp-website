using KadoshShared.ValueObjects;

namespace KadoshShared.Commands
{
    public interface ICommandResult
    {
        bool Success { get; }
        string Message { get; }
        ICollection<Error> Errors { get; }
    }
}
