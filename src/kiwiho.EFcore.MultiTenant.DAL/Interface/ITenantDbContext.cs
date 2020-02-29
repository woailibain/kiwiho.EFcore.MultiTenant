using System;
using kiwiho.EFcore.MultiTenant.Model;

namespace kiwiho.EFcore.MultiTenant.DAL.Interface
{
    public interface ITenantDbContext
    {
        TenantInfo Tenant { get;}
    }
}
