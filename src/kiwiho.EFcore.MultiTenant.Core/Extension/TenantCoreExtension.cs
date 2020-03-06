using System;
using System.Collections.Generic;
using kiwiho.EFcore.MultiTenant.Core.Impl;
using kiwiho.EFcore.MultiTenant.Core.Interface;
using kiwiho.EFcore.MultiTenant.DAL;
using kiwiho.EFcore.MultiTenant.DAL.Impl;
using kiwiho.EFcore.MultiTenant.DAL.Interface;
using kiwiho.EFcore.MultiTenant.Model;
using kiwiho.EFcore.MultiTenant.Model.Enum;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace kiwiho.EFcore.MultiTenant.Core.Extension
{
    public static class TenantCoreExtension
    {
        public static IServiceCollection AddDbPerConnection<TDbContext>(this IServiceCollection services,
            DbIntegrationType dbType, string key = "default",
            string connectionPrefix = "tenanted",
            Action<DbContextOptionsBuilder> optionAction = null,
            Action<IServiceProvider, string, DbContextOptionsBuilder> dbContextSetup = null)
            where TDbContext : DbContext, ITenantDbContext
        {
            var settings = new TenantSettings<TDbContext>()
            {
                Key = key,
                DbType = dbType,
                ConnectionPrefix = connectionPrefix,
                ConnectionType = ConnectionResolverType.ByDatabase,
                DbContextOptionAction = optionAction,
                DbContextSetup = dbContextSetup
            };

            return services.AddTenantedDatabase<TDbContext>(settings);
        }

        public static IServiceCollection AddDbPerConnection<TDbContext>(this IServiceCollection services,
            Action<TenantSettings<TDbContext>> setupAction = null)
            where TDbContext : DbContext, ITenantDbContext
        {
            var settings = new TenantSettings<TDbContext>()
            {
                ConnectionType = ConnectionResolverType.ByDatabase
            };
            return services.AddTenantedDatabase(settings, setupAction);
        }

        public static IServiceCollection AddDbPerTable<TDbContext>(this IServiceCollection services,
            DbIntegrationType dbType, string key = "default",
            string connectionName = "tenantConnection",
            Action<DbContextOptionsBuilder> optionAction = null,
            Action<IServiceProvider, string, DbContextOptionsBuilder> dbContextSetup = null)
            where TDbContext : DbContext, ITenantDbContext
        {
            var settings = new TenantSettings<TDbContext>()
            {
                Key = key,
                DbType = dbType,
                ConnectionName = connectionName,
                ConnectionType = ConnectionResolverType.ByTable,
                DbContextOptionAction = optionAction,
                DbContextSetup = dbContextSetup
            };

            return services.AddTenantedDatabase<TDbContext>(settings);
        }

        public static IServiceCollection AddDbPerTable<TDbContext>(this IServiceCollection services,
            Action<TenantSettings<TDbContext>> setupAction = null)
            where TDbContext : DbContext, ITenantDbContext
        {
            var settings = new TenantSettings<TDbContext>()
            {
                ConnectionType = ConnectionResolverType.ByTable
            };
            return services.AddTenantedDatabase(settings, setupAction);
        }

        public static IServiceCollection AddDbPerSchema<TDbContext>(this IServiceCollection services,
            DbIntegrationType dbType, string key = "default",
            string connectionName = "tenantConnection",
            Action<DbContextOptionsBuilder> optionAction = null,
            Action<IServiceProvider, string, DbContextOptionsBuilder> dbContextSetup = null)
            where TDbContext : DbContext, ITenantDbContext
        {
            var settings = new TenantSettings<TDbContext>()
            {
                Key = key,
                DbType = dbType,
                ConnectionName = connectionName,
                ConnectionType = ConnectionResolverType.BySchema,
                DbContextOptionAction = optionAction,
                DbContextSetup = dbContextSetup
            };

            return services.AddTenantedDatabase<TDbContext>(settings);
        }

        public static IServiceCollection AddDbPerSchema<TDbContext>(this IServiceCollection services,
            Action<TenantSettings<TDbContext>> setupAction = null)
            where TDbContext : DbContext, ITenantDbContext
        {
            var settings = new TenantSettings<TDbContext>()
            {
                ConnectionType = ConnectionResolverType.BySchema
            };
            return services.AddTenantedDatabase(settings, setupAction);
        }

        public static IServiceCollection AddTenantedDatabase<TDbContext>(this IServiceCollection services,
            TenantSettings<TDbContext> settings, Action<TenantSettings<TDbContext>> setupAction = null)
            where TDbContext : DbContext, ITenantDbContext
        {
            services.AddScoped<TenantInfo>();
            services.AddScoped<TenantOption>();
            services.TryAddScoped<ITenantInfoGenerator, SimpleHeaderTenantInfoGenerator>();
            services.TryAddScoped<ITenantConnectionResolver<TDbContext>, TenantConnectionResolver<TDbContext>>();
            services.TryAddScoped<ITenantEntityBuilder<TDbContext>, TenantEntityBuilder<TDbContext>>();
            services.TryAddScoped<IEntityScaner<TDbContext>, SimpleEntityScaner<TDbContext>>();
            services.AddScoped<NamedConnectionGenerator>();
            services.InitSettings(settings, setupAction);

            services.AddTenantDbContext<TDbContext>();


            return services;
        }

        internal static IServiceCollection AddTenantDbContext<TDbContext>(this IServiceCollection services)
            where TDbContext : DbContext, ITenantDbContext
        {
            services.AddDbContext<TDbContext>((serviceProvider, options) =>
            {
                var settings = serviceProvider.GetService<TenantSettings<TDbContext>>();
                var connectionResolver = serviceProvider.GetService<ITenantConnectionResolver<TDbContext>>();

                var tenant = serviceProvider.GetService<TenantInfo>();
                settings.DbContextSetup?.Invoke(serviceProvider, connectionResolver.GetConnection(), options);
                options.ReplaceServiceTenanted(settings);
                settings.DbContextOptionAction?.Invoke(options);

            });
            return services;
        }

        internal static IServiceCollection InitSettings<TDbContext>(this IServiceCollection services,
            TenantSettings<TDbContext> settings, Action<TenantSettings<TDbContext>> setupAction)
            where TDbContext : DbContext, ITenantDbContext
        {
            services.AddSingleton((sp) =>
            {
                var rct = settings ?? new TenantSettings<TDbContext>();
                setupAction?.Invoke(rct);
                return rct;
            });
            return services;
        }

        public static void ReplaceServiceTenanted<TDbContext>(this DbContextOptionsBuilder dbOptions,
            TenantSettings<TDbContext> settings)
            where TDbContext : DbContext, ITenantDbContext
        {
            if (Constants.specialConnectionTypes.Contains(settings.ConnectionType))
            {
                dbOptions.ReplaceService<IModelCacheKeyFactory, TenantModelCacheKeyFactory<TDbContext>>();
            }
        }


        public static void TenantBuilderSetup<TDbContext, TBuilder, TExtension>(this RelationalDbContextOptionsBuilder<TBuilder, TExtension> builder,
            IServiceProvider serviceProvider, TenantSettings<TDbContext> settings, TenantInfo tenant)
            where TDbContext : DbContext, ITenantDbContext
            where TBuilder : RelationalDbContextOptionsBuilder<TBuilder, TExtension>
            where TExtension : RelationalOptionsExtension, new()
        {
            if (settings.ConnectionType == ConnectionResolverType.ByTable)
            {
                builder.MigrationsHistoryTable($"{tenant.Name}__EFMigrationsHistory");
            }
            if (settings.ConnectionType == ConnectionResolverType.BySchema)
            {
                builder.MigrationsHistoryTable("__EFMigrationHistory", $"{(settings.SchemaFunc?.Invoke(tenant) ?? tenant.Name)}");
            }
        }
    }


}
