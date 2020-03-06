using System;
using kiwiho.EFcore.MultiTenant.Core.Interface;
using kiwiho.EFcore.MultiTenant.Model;
using kiwiho.EFcore.MultiTenant.Model.Enum;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace kiwiho.EFcore.MultiTenant.Core
{
    public abstract class TenantSettings
    {
        public string Key { get; set; }

        public ConnectionResolverType ConnectionType { get; set; }

        public DbIntegrationType DbType { get; set; }

        public Func<IConnectionGenerator> ConnectionGenerator { get; set; }

        public string ConnectionName { get; set; }

        public string ConnectionPrefix { get; set; }

        public Action<DbContextOptionsBuilder> DbContextOptionAction { get; set; }

        public Func<TenantInfo, string, string> TableNameFunc { get; set; }

        public Func<TenantInfo, string> SchemaFunc { get; set; }

        public Action<IServiceProvider, string, DbContextOptionsBuilder, TenantSettings> DbContextSetup { get; set; }
    }
    public class TenantSettings<TDbContext>
        where TDbContext : DbContext
    {
        public string Key { get; set; }

        public ConnectionResolverType ConnectionType { get; set; }

        public DbIntegrationType DbType { get; set; }

        public Func<IConnectionGenerator> ConnectionGenerator { get; set; }

        public string ConnectionName { get; set; }

        public string ConnectionPrefix { get; set; }

        public Action<DbContextOptionsBuilder> DbContextOptionAction { get; set; }

        public Func<TenantInfo, string, string> TableNameFunc { get; set; }

        public Func<TenantInfo, string> SchemaFunc { get; set; }

        public Action<IServiceProvider, string, DbContextOptionsBuilder> DbContextSetup { get; set; }
    }
}
