using System;
using kiwiho.EFcore.MultiTenant.Core;
using kiwiho.EFcore.MultiTenant.Model;
using kiwiho.EFcore.MultiTenant.Model.Enum;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.AspNetCore.Builder
{
    public static class TenantApplicationBuilderExtension
    {
        public static IApplicationBuilder UseTenantDb(this IApplicationBuilder app)
        {
            // var settings = serviceProvider.GetService<TenantSettings<TDbContext>>();
            // setupAction?.Invoke(settings);
            // if (settings.ConnectionType == ConnectionResolverType.ByTable)
            // {
            //     settings.TableNameFunc = settings.TableNameFunc ?? ((tenantInfo, tableName) => $"{tenantInfo.Name}_{tableName}");

            // }
            // if (settings.ConnectionType == ConnectionResolverType.BySchema)
            // {
            //     settings.TableNameFunc = settings.TableNameFunc ?? ((tenantInfo, tableName) => $"{tableName}");
            //     settings.SchemaFunc = settings.SchemaFunc ?? ((tenantInfo) => tenantInfo.Name);
            // }
            // return settings;

            return app;
        }
    }
}
