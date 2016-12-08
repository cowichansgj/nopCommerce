﻿using Nop.Data;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Core;
using Nop.Plugin.Commissions.ProductExtension.Data.Mapping;
using System.Data.Entity.Infrastructure;

namespace Nop.Plugin.Commissions.ProductExtension.Data
{
    public class CommissionsProductExtensionContext : DbContext, IDbContext
    {
        public CommissionsProductExtensionContext(string nameOrConnectionString) :base(nameOrConnectionString) { }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new ProductCommissionMap());
            modelBuilder.Configurations.Add(new OrderCommissionMap());
            modelBuilder.Configurations.Add(new OrderItemCommissionMap());

            base.OnModelCreating(modelBuilder);
        }

        public string CreateDatabaseInstallationScript()
        {
            return ((IObjectContextAdapter)this).ObjectContext.CreateDatabaseScript();
        }

        public void Install()
        {
            //It's required to set initializer to null (for SQL Server Compact).
            //otherwise, you'll get something like "The model backing the 'your context name' context has changed since the database was created. Consider using Code First Migrations to update the database"
            Database.SetInitializer<CommissionsProductExtensionContext>(null);

            Database.ExecuteSqlCommand(CreateDatabaseInstallationScript());
            SaveChanges();
        }

        public void Uninstall()
        {
            try
            {
                var dbScript = @"
                    DROP TABLE ProductCommission;
                    DROP TABLE OrderItemCommission;
                    DROP TABLE OrderCommission;";
                Database.ExecuteSqlCommand(dbScript);
                SaveChanges();
            }
            catch { }
        }

        public bool AutoDetectChangesEnabled { get; set; }

        public bool ProxyCreationEnabled { get; set; }
        
        public void Detach(object entity)
        {
            throw new NotImplementedException();
        }

        public int ExecuteSqlCommand(string sql, bool doNotEnsureTransaction = false, int? timeout = default(int?), params object[] parameters)
        {
            throw new NotImplementedException();
        }

        public IList<TEntity> ExecuteStoredProcedureList<TEntity>(string commandText, params object[] parameters) where TEntity : BaseEntity, new()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TElement> SqlQuery<TElement>(string sql, params object[] parameters)
        {
            throw new NotImplementedException();
        }

        IDbSet<TEntity> IDbContext.Set<TEntity>()
        {
            return base.Set<TEntity>();
        }
    }
}