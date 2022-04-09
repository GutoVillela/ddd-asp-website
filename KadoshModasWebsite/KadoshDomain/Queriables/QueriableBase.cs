using KadoshShared.Entities;
using System.Linq.Expressions;

namespace KadoshDomain.Queriables
{
    public class QueriableBase<TEntity> where TEntity : Entity
    {
        public static Expression<Func<TEntity, bool>> GetById(int id)
        {
            return x => x.Id == id;
        }

        public static Expression<Func<TEntity, bool>> GetIfActive()
        {
            return x => x.IsActive;
        }
    }
}
