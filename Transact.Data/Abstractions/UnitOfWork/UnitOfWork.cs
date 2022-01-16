using System;
using System.Collections.Generic;
using Transact.Data.Abstractions.DBFactory;
using Transact.Data.Abstractions.Repository;
using Transact.Data.Models;
using Transact.Data.Repository;

namespace Transact.Data.Abstractions.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IDbFactory dbFactory;
        private TransactDBContext dbContext;
        private Dictionary<Type, object> repositories;

        public UnitOfWork(IDbFactory dbFactory)
        {
            this.dbFactory = dbFactory;
        }

        public TransactDBContext DbContext
        {
            get { return dbContext ?? (dbContext = dbFactory.Init()); }
        }

        public void Commit()
        {
            DbContext.SaveChanges();
        }

        public IEntityBaseRepository<TEntity> GetRepository<TEntity>() where TEntity : class
        {
            if (repositories == null)
            {
                repositories = new Dictionary<Type, object>();
            }

            var type = typeof(TEntity);
            if (!repositories.ContainsKey(type))
            {
                repositories[type] = new EntityBaseRepository<TEntity>(this.dbFactory);
            }

            return (IEntityBaseRepository<TEntity>)repositories[type];
        }

        public TransactDBContext GetDBContext()
        {
            return DbContext;
        }
    }
}
