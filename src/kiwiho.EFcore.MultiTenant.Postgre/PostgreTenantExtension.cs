using System;
using kiwiho.EFcore.MultiTenant.Core;
using kiwiho.EFcore.MultiTenant.Core.Extension;
using kiwiho.EFcore.MultiTenant.Core.Interface;
using kiwiho.EFcore.MultiTenant.DAL.Interface;
using kiwiho.EFcore.MultiTenant.Model;
using kiwiho.EFcore.MultiTenant.Model.Enum;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.EntityFrameworkCore
{
    public static class PostgreTenantExtension
    {
        public static IServiceCollection AddPostgrePerConnection<TDbContext>(this IServiceCollection services,
            string key = "default",
            string connectionPrefix = "tenanted",
            Action<DbContextOptionsBuilder> optionAction = null,
            Action<IServiceProvider, string, DbContextOptionsBuilder> dbContextSetup = null)
            where TDbContext : DbContext, ITenantDbContext
        {
            return services.AddDbPerConnection<TDbContext>(DbIntegrationType.Postgre, key, connectionPrefix,
                optionAction, dbContextSetup ?? SetUpPostgre<TDbContext>);
        }

        public static IServiceCollection AddPostgrePerConnection<TDbContext>(this IServiceCollection services,
            Action<TenantSettings<TDbContext>> setupAction = null)
            where TDbContext : DbContext, ITenantDbContext
        {
            return services.AddDbPerConnection<TDbContext>(CombineSettings(setupAction));
        }

        public static IServiceCollection AddPostgrePerTable<TDbContext>(this IServiceCollection services,
            string key = "default",
            string connectionName = "tenanted",
            Action<DbContextOptionsBuilder> optionAction = null,
            Action<IServiceProvider, string, DbContextOptionsBuilder> dbContextSetup = null)
            where TDbContext : DbContext, ITenantDbContext
        {
            return services.AddDbPerTable<TDbContext>(DbIntegrationType.Postgre, key, connectionName,
                optionAction, dbContextSetup ?? SetUpPostgre<TDbContext>);
        }

        public static IServiceCollection AddPostgrePerTable<TDbContext>(this IServiceCollection services,
            Action<TenantSettings<TDbContext>> setupAction = null)
            where TDbContext : DbContext, ITenantDbContext
        {
            return services.AddDbPerTable<TDbContext>(CombineSettings(setupAction));
        }

        public static IServiceCollection AddPostgrePerSchema<TDbContext>(this IServiceCollection services,
            string key = "default",
            string connectionName = "tenanted",
            Action<DbContextOptionsBuilder> optionAction = null,
            Action<IServiceProvider, string, DbContextOptionsBuilder> dbContextSetup = null)
            where TDbContext : DbContext, ITenantDbContext
        {
            return services.AddDbPerSchema<TDbContext>(DbIntegrationType.Postgre, key, connectionName,
                optionAction, dbContextSetup ?? SetUpPostgre<TDbContext>);
        }

        public static IServiceCollection AddPostgrePerSchema<TDbContext>(this IServiceCollection services,
            Action<TenantSettings<TDbContext>> setupAction = null)
            where TDbContext : DbContext, ITenantDbContext
        {
            return services.AddDbPerSchema<TDbContext>(CombineSettings(setupAction));
        }


        static Action<TenantSettings<TDbContext>> CombineSettings<TDbContext>(
            Action<TenantSettings<TDbContext>> setupAction = null)
            where TDbContext : DbContext, ITenantDbContext
        {
            return (settings) =>
            {
                settings.DbContextSetup = SetUpPostgre<TDbContext>;
                setupAction?.Invoke(settings);
            };
        }

        internal static void SetUpPostgre<TDbContext>(IServiceProvider serviceProvider, string connectionString,
            DbContextOptionsBuilder optionsBuilder)
            where TDbContext : DbContext, ITenantDbContext
        {
            var settings = serviceProvider.GetService<TenantSettings<TDbContext>>();
            var tenant = serviceProvider.GetService<TenantInfo>();
            optionsBuilder.UseNpgsql(connectionString, builder =>
            {
                builder.TenantBuilderSetup(serviceProvider, settings, tenant);
            });
        }
    }
}
