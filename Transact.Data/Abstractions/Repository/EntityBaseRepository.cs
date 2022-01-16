using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Linq;
using System.Linq.Expressions;
using Transact.Data.Abstractions.DBFactory;
using Transact.Data.Abstractions.Repository;
using Transact.Data.Models;

namespace Transact.Data.Repository
{
    public class EntityBaseRepository<TEntity> : IEntityBaseRepository<TEntity>
             where TEntity : class
    {

        private TransactDBContext dataContext;

        #region Properties
        protected IDbFactory DbFactory
        {
            get;
            private set;
        }

        protected TransactDBContext DbContext
        {
            get { return dataContext ?? (dataContext = DbFactory.Init()); }
        }
        public EntityBaseRepository(IDbFactory dbFactory)
        {
            DbFactory = dbFactory;
        }
        #endregion

        public virtual void Add(TEntity entity)
        {
            DbContext.Set<TEntity>().Add(entity);
        }

        public virtual void Edit(TEntity entity)
        {
            EntityEntry dbEntityEntry = DbContext.Entry<TEntity>(entity);
            dbEntityEntry.State = EntityState.Modified;
        }

        public virtual void Delete(TEntity entity)
        {
            EntityEntry dbEntityEntry = DbContext.Entry<TEntity>(entity);
            dbEntityEntry.State = EntityState.Deleted;
        }

        public TEntity Get(Guid id)
        {
            return DbContext.Set<TEntity>().Find(id);
        }

        public TEntity Get()
        {
            return DbContext.Set<TEntity>().FirstOrDefault();
        }

        public virtual IQueryable<TEntity> GetAll()
        {
            return DbContext.Set<TEntity>();
        }

        public virtual IQueryable<TEntity> AllIncluding(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            IQueryable<TEntity> query = DbContext.Set<TEntity>().Where(predicate);

            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }
            return query;
        }

        public virtual IQueryable<TEntity> FindBy(Expression<Func<TEntity, bool>> predicate)
        {
            return DbContext.Set<TEntity>().Where(predicate);
        }

        public virtual IQueryable<TEntity> FromSQL(string sql)
        {
            return DbContext.Set<TEntity>().FromSqlRaw(sql);
        }
    }
}
