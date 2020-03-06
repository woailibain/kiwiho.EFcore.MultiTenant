using System;
using kiwiho.EFcore.MultiTenant.Core;
using kiwiho.EFcore.MultiTenant.Core.Extension;
using kiwiho.EFcore.MultiTenant.Core.Interface;
using kiwiho.EFcore.MultiTenant.DAL.Interface;
using kiwiho.EFcore.MultiTenant.Model;
using kiwiho.EFcore.MultiTenant.Model.Enum;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.EntityFrameworkCore
{
    public static class MySqlTenantExtension
    {
        public static IServiceCollection AddMySqlPerConnection<TDbContext>(this IServiceCollection services,
            string key = "default",
            string connectionPrefix = "tenanted",
            Action<DbContextOptionsBuilder> optionAction = null,
            Action<IServiceProvider, string, DbContextOptionsBuilder> dbContextSetup = null)
            where TDbContext : DbContext, ITenantDbContext
        {
            return services.AddDbPerConnection<TDbContext>(DbIntegrationType.Mysql, key, connectionPrefix, 
                optionAction, dbContextSetup ?? SetUpMySql<TDbContext>);
        }

        public static IServiceCollection AddMySqlPerConnection<TDbContext>(this IServiceCollection services,
            Action<TenantSettings<TDbContext>> setupAction = null)
            where TDbContext : DbContext, ITenantDbContext
        {
            return services.AddDbPerConnection<TDbContext>(CombineSettings(setupAction));
        }

        public static IServiceCollection AddMySqlPerTable<TDbContext>(this IServiceCollection services,
            string key = "default",
            string connectionName = "tenanted",
            Action<DbContextOptionsBuilder> optionAction = null,
            Action<IServiceProvider, string, DbContextOptionsBuilder> dbContextSetup = null)
            where TDbContext : DbContext, ITenantDbContext
        {
            return services.AddDbPerTable<TDbContext>(DbIntegrationType.Mysql, key, connectionName, 
                optionAction, dbContextSetup ?? SetUpMySql<TDbContext>);
        }

        public static IServiceCollection AddMySqlPerTable<TDbContext>(this IServiceCollection services,
            Action<TenantSettings<TDbContext>> setupAction = null)
            where TDbContext : DbContext, ITenantDbContext
        {
            return services.AddDbPerTable<TDbContext>(CombineSettings(setupAction));
        }

        static Action<TenantSettings<TDbContext>> CombineSettings<TDbContext>(
            Action<TenantSettings<TDbContext>> setupAction = null)
            where TDbContext : DbContext, ITenantDbContext
        {
            return (settings) =>
            {
                settings.DbContextSetup = SetUpMySql<TDbContext>;
                setupAction?.Invoke(settings);
            };
        }

        internal static void SetUpMySql<TDbContext>(IServiceProvider serviceProvider, string connectionString,
            DbContextOptionsBuilder optionsBuilder)
            where TDbContext : DbContext, ITenantDbContext
        {
            var settings = serviceProvider.GetService<TenantSettings<TDbContext>>();
            var tenant = serviceProvider.GetService<TenantInfo>();
            optionsBuilder.UseMySql(connectionString, builder =>
            {
                builder.TenantBuilderSetup(serviceProvider, settings, tenant);
            });
        }

    }

}
