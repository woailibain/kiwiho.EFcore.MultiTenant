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
    public static class SqlServerTenantExtension
    {
        public static IServiceCollection AddSqlServerPerConnection<TDbContext>(this IServiceCollection services,
            string key = "default",
            string connectionPrefix = "tenanted",
            Action<DbContextOptionsBuilder> optionAction = null)
            where TDbContext : DbContext, ITenantDbContext
        {
            services.AddSqlServerTenanted<TDbContext>();
            return services.AddDbPerConnection<TDbContext>(DbIntegrationType.SqlServer, key, connectionPrefix);
        }

        public static IServiceCollection AddSqlServerPerConnection<TDbContext>(this IServiceCollection services,
            Action<TenantSettings<TDbContext>> setupAction = null)
            where TDbContext : DbContext, ITenantDbContext
        {
            services.AddSqlServerTenanted<TDbContext>();
            return services.AddDbPerConnection<TDbContext>(setupAction);
        }

        public static IServiceCollection AddSqlServerPerTable<TDbContext>(this IServiceCollection services,
            string key = "default",
            string connectionName = "tenanted",
            Action<DbContextOptionsBuilder> optionAction = null)
            where TDbContext : DbContext, ITenantDbContext
        {
            services.AddSqlServerTenanted<TDbContext>();
            return services.AddDbPerTable<TDbContext>(DbIntegrationType.SqlServer, key, connectionName);
        }

        public static IServiceCollection AddSqlServerPerTable<TDbContext>(this IServiceCollection services,
            Action<TenantSettings<TDbContext>> setupAction = null)
            where TDbContext : DbContext, ITenantDbContext
        {
            services.AddSqlServerTenanted<TDbContext>();
            return services.AddDbPerTable<TDbContext>(setupAction);
        }

        public static IServiceCollection AddSqlServerPerSchema<TDbContext>(this IServiceCollection services,
            string key = "default",
            string connectionName = "tenanted",
            Action<DbContextOptionsBuilder> optionAction = null)
            where TDbContext : DbContext, ITenantDbContext
        {
            services.AddSqlServerTenanted<TDbContext>();
            return services.AddDbPerSchema<TDbContext>(DbIntegrationType.SqlServer, key, connectionName);
        }

        public static IServiceCollection AddSqlServerPerSchema<TDbContext>(this IServiceCollection services,
            Action<TenantSettings<TDbContext>> setupAction = null)
            where TDbContext : DbContext, ITenantDbContext
        {
            services.AddSqlServerTenanted<TDbContext>();
            return services.AddDbPerSchema<TDbContext>(setupAction);
        }

        internal static IServiceCollection AddSqlServerTenanted<TDbContext>(this IServiceCollection services)
            where TDbContext : DbContext, ITenantDbContext
        {
            services.AddDbContext<TDbContext>((serviceProvider, options) =>
            {
                SetUpSqlServer<TDbContext>(serviceProvider, options);
            });

            return services;
        }

        internal static void SetUpSqlServer<TDbContext>(IServiceProvider serviceProvider,
            DbContextOptionsBuilder optionsBuilder)
            where TDbContext : DbContext, ITenantDbContext
        {
            var settings = serviceProvider.GetService<TenantSettings<TDbContext>>();

            var connectionResolver = serviceProvider.GetService<ITenantConnectionResolver<TDbContext>>();

            var tenant = serviceProvider.GetService<TenantInfo>();
            optionsBuilder.UseSqlServer(connectionResolver.GetConnection(), builder =>
            {
                
                if (settings.ConnectionType == ConnectionResolverType.ByTable)
                {
                    builder.MigrationsHistoryTable($"{tenant.Name}__EFMigrationsHistory");
                }
                if (settings.ConnectionType == ConnectionResolverType.BySchema)
                {
                    builder.MigrationsHistoryTable("__EFMigrationHistory", $"{(settings.SchemaFunc?.Invoke(tenant) ?? tenant.Name)}");
                }
                builder.ss();
            });

            optionsBuilder.ReplaceServiceTenanted(settings);
            settings.DbContextOptionAction?.Invoke(optionsBuilder);
        }

    }
}
