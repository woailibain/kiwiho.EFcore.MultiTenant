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
            Action<DbContextOptionsBuilder> optionAction = null)
            where TDbContext : DbContext, ITenantDbContext
        {
            var settings = new TenantSettings<TDbContext>()
            {
                Key = key,
                DbType = dbType,
                ConnectionPrefix = connectionPrefix,
                ConnectionType = ConnectionResolverType.ByDatabase,
                DbContextOptionAction = optionAction
            };

            return services.AddTenantedDatabase<TDbContext>(settings);
        }

        public static IServiceCollection AddDbPerConnection<TDbContext>(this IServiceCollection services)
            where TDbContext : DbContext, ITenantDbContext
        {
            var settings = new TenantSettings<TDbContext>()
            {
                ConnectionType = ConnectionResolverType.ByDatabase
            };
            return services.AddTenantedDatabase(settings);
        }

        public static IServiceCollection AddDbPerTable<TDbContext>(this IServiceCollection services,
            DbIntegrationType dbType, string key = "default",
            string connectionName = "tenantConnection",
            Action<DbContextOptionsBuilder> optionAction = null)
            where TDbContext : DbContext, ITenantDbContext
        {
            var settings = new TenantSettings<TDbContext>()
            {
                Key = key,
                DbType = dbType,
                ConnectionName = connectionName,
                ConnectionType = ConnectionResolverType.ByTable,
                DbContextOptionAction = optionAction
            };

            return services.AddTenantedDatabase<TDbContext>(settings);
        }

        public static IServiceCollection AddDbPerTable<TDbContext>(this IServiceCollection services)
            where TDbContext : DbContext, ITenantDbContext
        {
            var settings = new TenantSettings<TDbContext>()
            {
                ConnectionType = ConnectionResolverType.ByTable
            };
            return services.AddTenantedDatabase(settings);
        }

        public static IServiceCollection AddDbPerSchema<TDbContext>(this IServiceCollection services,
            DbIntegrationType dbType, string key = "default",
            string connectionName = "tenantConnection",
            Action<DbContextOptionsBuilder> optionAction = null)
            where TDbContext : DbContext, ITenantDbContext
        {
            var settings = new TenantSettings<TDbContext>()
            {
                Key = key,
                DbType = dbType,
                ConnectionName = connectionName,
                ConnectionType = ConnectionResolverType.BySchema,
                DbContextOptionAction = optionAction
            };

            return services.AddTenantedDatabase<TDbContext>(settings);
        }

        public static IServiceCollection AddDbPerSchema<TDbContext>(this IServiceCollection services)
            where TDbContext : DbContext, ITenantDbContext
        {
            var settings = new TenantSettings<TDbContext>()
            {
                ConnectionType = ConnectionResolverType.BySchema
            };
            return services.AddTenantedDatabase(settings);
        }

        public static IServiceCollection AddTenantedDatabase<TDbContext>(this IServiceCollection services,
            TenantSettings<TDbContext> settings)
            where TDbContext : DbContext, ITenantDbContext
        {
            services.AddSingleton(settings);
            services.AddScoped<TenantInfo>();
            services.AddScoped<TenantOption>();
            services.TryAddScoped<ITenantInfoGenerator, SimpleHeaderTenantInfoGenerator>();
            services.TryAddScoped<ITenantConnectionResolver<TDbContext>, TenantConnectionResolver<TDbContext>>();
            services.TryAddScoped<ITenantEntityBuilder<TDbContext>, TenantEntityBuilder<TDbContext>>();
            services.TryAddScoped<IEntityScaner<TDbContext>, SimpleEntityScaner<TDbContext>>();
            services.AddScoped<NamedConnectionGenerator>();



            return services;
        }

        public static TenantSettings<TDbContext> InitSettings<TDbContext>(this DbContextOptionsBuilder dbOptions,
            IServiceProvider serviceProvider, Action<TenantSettings<TDbContext>> setupAction)
            where TDbContext : DbContext, ITenantDbContext
        {
            var settings = serviceProvider.GetService<TenantSettings<TDbContext>>();
            setupAction?.Invoke(settings);
            if (settings.ConnectionType == ConnectionResolverType.ByTable)
            {
                settings.TableNameFunc = settings.TableNameFunc ?? ((tenantInfo, tableName) => $"{tenantInfo.Name}_{tableName}");

            }
            if (settings.ConnectionType == ConnectionResolverType.BySchema)
            {
                settings.TableNameFunc = settings.TableNameFunc ?? ((tenantInfo, tableName) => $"{tableName}");
                settings.SchemaFunc = settings.SchemaFunc ?? ((tenantInfo) => tenantInfo.Name);
            }
            return settings;
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
    }
}
