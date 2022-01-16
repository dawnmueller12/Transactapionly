using Transact.Data.Abstractions.Repository;
using Transact.Data.Models;

namespace Transact.Data.Abstractions.UnitOfWork
{
    public interface IUnitOfWork
    {
        void Commit();

        IEntityBaseRepository<TEntity> GetRepository<TEntity>() where TEntity : class;

        TransactDBContext GetDBContext();
    }
}
