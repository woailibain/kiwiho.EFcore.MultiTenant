using System;
using kiwiho.EFcore.MultiTenant.Model;

namespace kiwiho.EFcore.MultiTenant.Core.Interface
{
    public interface IConnectionGenerator
    {
        string TenantKey { get;}

        bool MatchTenantKey(string tenantKey);

        string GetConnection(TenantOption option, TenantInfo tenantInfo);
    }
}
