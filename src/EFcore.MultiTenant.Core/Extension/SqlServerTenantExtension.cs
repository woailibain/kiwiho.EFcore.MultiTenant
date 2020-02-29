using System;
using kiwiho.EFcore.MultiTenant.Core.Interface;
using kiwiho.EFcore.MultiTenant.DAL.Interface;
using kiwiho.EFcore.MultiTenant.Model;
using kiwiho.EFcore.MultiTenant.Model.Enum;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace kiwiho.EFcore.MultiTenant.Core.Extension
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
            services.AddSqlServerTenanted<TDbContext>(setupAction);
            return services.AddDbPerConnection<TDbContext>();
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
            services.AddSqlServerTenanted<TDbContext>(setupAction);
            return services.AddDbPerTable<TDbContext>();
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
            services.AddSqlServerTenanted<TDbContext>(setupAction);
            return services.AddDbPerSchema<TDbContext>();
        }

        internal static IServiceCollection AddSqlServerTenanted<TDbContext>(this IServiceCollection services,
            Action<TenantSettings<TDbContext>> setupAction = null)
            where TDbContext : DbContext, ITenantDbContext
        {
            services.AddDbContext<TDbContext>((serviceProvider, options) =>
            {
                SetUpSqlServer<TDbContext>(serviceProvider, options, setupAction);
            });

            return services;
        }

        internal static void SetUpSqlServer<TDbContext>(IServiceProvider serviceProvider,
            DbContextOptionsBuilder optionsBuilder,
            Action<TenantSettings<TDbContext>> setupAction = null)
            where TDbContext : DbContext, ITenantDbContext
        {
            var settings = optionsBuilder.InitSettings<TDbContext>(serviceProvider, setupAction);
            settings.DbType = DbIntegrationType.SqlServer;

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
            });

            optionsBuilder.ReplaceServiceTenanted(settings);
            settings.DbContextOptionAction?.Invoke(optionsBuilder);
        }
    }
}
