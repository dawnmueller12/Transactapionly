using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Diagnostics;
using Transact.Data.Models;

namespace Transact.Data.Abstractions.DBFactory
{
    public class DbFactory : Disposable, IDbFactory
    {
        TransactDBContext dbContext;
        private IConfiguration _config;

        public DbFactory(IConfiguration config)
        {
            _config = config;
        }

        public TransactDBContext Init()
        {
            if (dbContext == null)
            {
                Debug.WriteLine("DBContext Initiated : " + DateTime.UtcNow.ToString());
                var builder = new DbContextOptionsBuilder<TransactDBContext>();

                builder.UseSqlServer(_config.GetConnectionString("TransactDBContext"));
                dbContext = new TransactDBContext(builder.Options);
            }
            return dbContext;
        }
        protected override void DisposeCore()
        {
            if (dbContext != null)
                dbContext.Dispose();
        }
    }
}
