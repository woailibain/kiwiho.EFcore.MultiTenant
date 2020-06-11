using System;
using kiwiho.EFcore.MultiTenant.DAL.Impl;
using kiwiho.EFcore.MultiTenant.MixMode.Entity;
using kiwiho.EFcore.MultiTenant.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace kiwiho.EFcore.MultiTenant.MixMode.DbContext
{
    public class StoreDbContext : TenantBaseDbContext
    {
        public DbSet<Product> Products => this.Set<Product>();
        
        public StoreDbContext(IMemoryCache cache,DbContextOptions<StoreDbContext> options, TenantInfo tenant, IServiceProvider serviceProvider) 
            : base(options, tenant, serviceProvider)
        {
            string ab="ab";
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
