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
            services.AddPostgreTenanted<TDbContext>();
            return services.AddDbPerConnection<TDbContext>(setupAction);
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
            services.AddPostgreTenanted<TDbContext>();
            return services.AddDbPerTable<TDbContext>(setupAction);
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
            services.AddPostgreTenanted<TDbContext>();
            return services.AddDbPerSchema<TDbContext>(setupAction);
        }

        internal static IServiceCollection AddPostgreTenanted<TDbContext>(this IServiceCollection services)
            where TDbContext : DbContext, ITenantDbContext
        {
            services.AddDbContext<TDbContext>((serviceProvider, options) =>
            {
                SetUpPostgre<TDbContext>(serviceProvider, options);
            });

            return services;
        }

        internal static void SetUpPostgre<TDbContext>(IServiceProvider serviceProvider,
            DbContextOptionsBuilder optionsBuilder)
            where TDbContext : DbContext, ITenantDbContext
        {
            var settings = serviceProvider.GetService<TenantSettings<TDbContext>>();

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
