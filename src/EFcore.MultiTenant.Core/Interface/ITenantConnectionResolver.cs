using System;

namespace kiwiho.EFcore.MultiTenant.Core.Interface
{
    internal interface ITenantConnectionResolver<TDbContext>
    {
        string GetConnection();
    }
}
