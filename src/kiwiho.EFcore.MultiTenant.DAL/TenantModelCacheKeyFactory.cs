using System;
using kiwiho.EFcore.MultiTenant.DAL.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace kiwiho.EFcore.MultiTenant.DAL
{
    public sealed class TenantModelCacheKeyFactory<TDbContext> : ModelCacheKeyFactory
        where TDbContext : DbContext, ITenantDbContext
    {

        public override object Create(DbContext context)
        {
            var dbContext = context as TDbContext;
            return new TenantModelCacheKey<TDbContext>(dbContext, dbContext?.Tenant?.Name ?? "no_tenant_identifier");
        }

        public TenantModelCacheKeyFactory(ModelCacheKeyFactoryDependencies dependencies) : base(dependencies)
        {
        }
    }
}
