using System;
using Microsoft.EntityFrameworkCore;

namespace kiwiho.EFcore.MultiTenant.DAL.Interface
{
    public interface ITenantEntityBuilder
    {
        void UpdateEntities(ModelBuilder modelBuilder);
    }
    public interface ITenantEntityBuilder<TDbContext> : ITenantEntityBuilder
        where TDbContext : DbContext
    {
        
    }
}
