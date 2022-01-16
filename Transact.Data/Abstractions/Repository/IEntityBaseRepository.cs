using System;
using System.Linq;
using System.Linq.Expressions;

namespace Transact.Data.Abstractions.Repository
{
    public interface IEntityBaseRepository<TEntity> where TEntity : class
    {
        void Add(TEntity entity);

        void Edit(TEntity entity);

        void Delete(TEntity entity);

        TEntity Get(Guid id);
        TEntity Get();
        IQueryable<TEntity> GetAll();
        IQueryable<TEntity> AllIncluding(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includeProperties);

        IQueryable<TEntity> FindBy(Expression<Func<TEntity, bool>> predicate);

        IQueryable<TEntity> FromSQL(string sqlQuery);

    }
}
