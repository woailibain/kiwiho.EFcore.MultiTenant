using System;
using kiwiho.EFcore.MultiTenant.DAL.Impl;
using kiwiho.EFcore.MultiTenant.Example.DAL.Entity.Store;
using kiwiho.EFcore.MultiTenant.Model;
using Microsoft.EntityFrameworkCore;

namespace kiwiho.EFcore.MultiTenant.Example.DAL
{
    public class StoreDbContext : TenantBaseDbContext
    {
        public DbSet<Product> Products => this.Set<Product>();
        
        public StoreDbContext(DbContextOptions options, TenantInfo tenant, IServiceProvider serviceProvider) 
            : base(options, tenant, serviceProvider)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
