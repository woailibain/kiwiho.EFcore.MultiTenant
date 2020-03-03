using System;
using kiwiho.EFcore.MultiTenant.DAL.Impl;
using kiwiho.EFcore.MultiTenant.Example.DAL.Entity.Customer;
using kiwiho.EFcore.MultiTenant.Model;
using Microsoft.EntityFrameworkCore;

namespace kiwiho.EFcore.MultiTenant.Example.DAL
{
    public class CustomerDbContext : TenantBaseDbContext
    {
        public DbSet<Instruction> Instructions => this.Set<Instruction>();
        public CustomerDbContext(DbContextOptions<CustomerDbContext> options, TenantInfo tenant, IServiceProvider serviceProvider) 
            : base(options, tenant, serviceProvider)
        {
        }
    }
}
