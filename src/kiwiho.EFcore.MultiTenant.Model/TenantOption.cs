using System;
using kiwiho.EFcore.MultiTenant.Model.Enum;

namespace kiwiho.EFcore.MultiTenant.Model
{
    public class TenantOption
    {
        public string Key { get; set; }

        public TenantInfo TenantInfo { get; set; }

        public ConnectionResolverType ConnectionType { get; set; }

        public DbIntegrationType DbType { get; set; }

        public string ConnectionName { get; set; }

        public string ConnectionPrefix { get; set; }

        public Func<TenantInfo, string, string> TableNameFunc { get; set; }

        public Func<TenantInfo, string> SchemaFunc { get; set; }
    }
}
