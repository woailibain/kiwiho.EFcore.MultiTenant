using System;

namespace kiwiho.EFcore.MultiTenant.Core.Interface
{
    public interface ITenantConnectionResolver<TDbContext>
    {
        string GetConnection();
    }
}
