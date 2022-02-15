using KadoshRepository.Persistence.DataContexts;
using KadoshShared.Repositories;

namespace KadoshRepository.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly StoreDataContext _applicationContext;

        public UnitOfWork(StoreDataContext applicationContext) => _applicationContext = applicationContext;

        public async Task CommitAsync() => await _applicationContext.SaveChangesAsync();
    }
}
