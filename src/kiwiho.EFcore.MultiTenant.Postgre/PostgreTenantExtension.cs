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
            Action<DbContextOptionsBuilder> optionAction = null)
            where TDbContext : DbContext, ITenantDbContext
        {
            services.AddPostgreTenanted<TDbContext>();
            return services.AddDbPerConnection<TDbContext>(DbIntegrationType.Postgre, key, connectionPrefix);
        }

        public static IServiceCollection AddPostgrePerConnection<TDbContext>(this IServiceCollection services,
            Action<TenantSettings<TDbContext>> setupAction = null)
            where TDbContext : DbContext, ITenantDbContext
        {
            services.AddPostgreTenanted<TDbContext>(setupAction);
            return services.AddDbPerConnection<TDbContext>();
        }

        public static IServiceCollection AddPostgrePerTable<TDbContext>(this IServiceCollection services,
            string key = "default",
            string connectionName = "tenanted",
            Action<DbContextOptionsBuilder> optionAction = null)
            where TDbContext : DbContext, ITenantDbContext
        {
            services.AddPostgreTenanted<TDbContext>();
            return services.AddDbPerTable<TDbContext>(DbIntegrationType.Postgre, key, connectionName);
        }

        public static IServiceCollection AddPostgrePerTable<TDbContext>(this IServiceCollection services,
            Action<TenantSettings<TDbContext>> setupAction = null)
            where TDbContext : DbContext, ITenantDbContext
        {
            services.AddPostgreTenanted<TDbContext>(setupAction);
            return services.AddDbPerTable<TDbContext>();
        }

        public static IServiceCollection AddPostgrePerSchema<TDbContext>(this IServiceCollection services,
            string key = "default",
            string connectionName = "tenanted",
            Action<DbContextOptionsBuilder> optionAction = null)
            where TDbContext : DbContext, ITenantDbContext
        {
            services.AddPostgreTenanted<TDbContext>();
            return services.AddDbPerSchema<TDbContext>(DbIntegrationType.Postgre, key, connectionName);
        }

        public static IServiceCollection AddPostgrePerSchema<TDbContext>(this IServiceCollection services,
            Action<TenantSettings<TDbContext>> setupAction = null)
            where TDbContext : DbContext, ITenantDbContext
        {
            services.AddPostgreTenanted<TDbContext>(setupAction);
            return services.AddDbPerSchema<TDbContext>();
        }

        internal static IServiceCollection AddPostgreTenanted<TDbContext>(this IServiceCollection services,
            Action<TenantSettings<TDbContext>> setupAction = null)
            where TDbContext : DbContext, ITenantDbContext
        {
            services.AddDbContext<TDbContext>((serviceProvider, options) =>
            {
                SetUpPostgre<TDbContext>(serviceProvider, options, setupAction);
            });

            return services;
        }

        internal static void SetUpPostgre<TDbContext>(IServiceProvider serviceProvider,
            DbContextOptionsBuilder optionsBuilder,
            Action<TenantSettings<TDbContext>> setupAction = null)
            where TDbContext : DbContext, ITenantDbContext
        {
            var settings = optionsBuilder.InitSettings<TDbContext>(serviceProvider, setupAction);
            settings.DbType = DbIntegrationType.Postgre;

            var connectionResolver = serviceProvider.GetService<ITenantConnectionResolver<TDbContext>>();

            var tenant = serviceProvider.GetService<TenantInfo>();
            optionsBuilder.UseNpgsql(connectionResolver.GetConnection(), builder =>
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
