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
            Action<DbContextOptionsBuilder> optionAction = null)
            where TDbContext : DbContext, ITenantDbContext
        {
            services.AddMySqlTenanted<TDbContext>();
            return services.AddDbPerConnection<TDbContext>(DbIntegrationType.Mysql, key, connectionPrefix);
        }

        public static IServiceCollection AddMySqlPerConnection<TDbContext>(this IServiceCollection services,
            Action<TenantSettings<TDbContext>> setupAction = null)
            where TDbContext : DbContext, ITenantDbContext
        {
            services.AddMySqlTenanted<TDbContext>();
            return services.AddDbPerConnection<TDbContext>(setupAction);
        }

        public static IServiceCollection AddMySqlPerTable<TDbContext>(this IServiceCollection services,
            string key = "default",
            string connectionName = "tenanted",
            Action<DbContextOptionsBuilder> optionAction = null)
            where TDbContext : DbContext, ITenantDbContext
        {
            services.AddMySqlTenanted<TDbContext>();
            return services.AddDbPerTable<TDbContext>(DbIntegrationType.Mysql, key, connectionName);
        }

        public static IServiceCollection AddMySqlPerTable<TDbContext>(this IServiceCollection services,
            Action<TenantSettings<TDbContext>> setupAction = null)
            where TDbContext : DbContext, ITenantDbContext
        {
            services.AddMySqlTenanted<TDbContext>();
            return services.AddDbPerTable<TDbContext>(setupAction);
        }

        internal static IServiceCollection AddMySqlTenanted<TDbContext>(this IServiceCollection services)
            where TDbContext : DbContext, ITenantDbContext
        {
            services.AddDbContext<TDbContext>((serviceProvider, options) =>
            {
                SetUpMySql<TDbContext>(serviceProvider, options);
            });

            return services;
        }

        internal static void SetUpMySql<TDbContext>(IServiceProvider serviceProvider,
            DbContextOptionsBuilder optionsBuilder)
            where TDbContext : DbContext, ITenantDbContext
        {
            var settings = serviceProvider.GetService<TenantSettings<TDbContext>>();
            var connectionResolver = serviceProvider.GetService<ITenantConnectionResolver<TDbContext>>();

            var tenant = serviceProvider.GetService<TenantInfo>();
            optionsBuilder.UseMySql(connectionResolver.GetConnection(), builder =>
            {
                if (settings.ConnectionType == ConnectionResolverType.ByTable)
                {
                    builder.MigrationsHistoryTable($"{tenant.Name}__EFMigrationsHistory");
                }
                builder.ss();
            });

            optionsBuilder.ReplaceServiceTenanted(settings);
            settings.DbContextOptionAction?.Invoke(optionsBuilder);
        }
    }
}
