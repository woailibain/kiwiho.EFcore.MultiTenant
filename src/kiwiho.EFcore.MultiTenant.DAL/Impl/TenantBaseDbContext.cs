using System;
using kiwiho.EFcore.MultiTenant.DAL.Interface;
using kiwiho.EFcore.MultiTenant.Model;
using Microsoft.EntityFrameworkCore;

namespace kiwiho.EFcore.MultiTenant.DAL.Impl
{
    public abstract class TenantBaseDbContext : DbContext, ITenantDbContext
    {
        public TenantInfo Tenant { get; protected internal set; }
        private readonly IServiceProvider serviceProvider;
        public TenantBaseDbContext(DbContextOptions options, TenantInfo tenant, IServiceProvider serviceProvider)
            : base(options)
        {
            this.serviceProvider = serviceProvider;
            this.Tenant = tenant;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var builderType = typeof(ITenantEntityBuilder<>).MakeGenericType(this.GetType());
            
            ITenantEntityBuilder entityBuilder = (ITenantEntityBuilder)serviceProvider.GetService(builderType);

            entityBuilder.UpdateEntities(modelBuilder);

        }
    }
}
