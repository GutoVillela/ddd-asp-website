namespace KadoshShared.Repositories
{
    public interface IUnitOfWork
    {
        Task CommitAsync();
    }
}
