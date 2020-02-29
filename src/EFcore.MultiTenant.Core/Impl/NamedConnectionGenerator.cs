using System;
using kiwiho.EFcore.MultiTenant.Core.Interface;
using kiwiho.EFcore.MultiTenant.Model;
using kiwiho.EFcore.MultiTenant.Model.Enum;
using Microsoft.Extensions.Configuration;

namespace kiwiho.EFcore.MultiTenant.Core.Impl
{
    public class NamedConnectionGenerator : IConnectionGenerator
    {
        private readonly IConfiguration configuration;
        public string TenantKey => "unknow";

        public NamedConnectionGenerator(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public string GetConnection(TenantOption option, TenantInfo tenantInfo)
        {
            string connectionString = null;
            switch (option.ConnectionType)
            {
                case ConnectionResolverType.ByDatabase:
                    connectionString = configuration.GetConnectionString($"{option.ConnectionPrefix}{tenantInfo.Name}");
                    break;
                case ConnectionResolverType.ByTable:
                case ConnectionResolverType.BySchema:
                    connectionString = configuration.GetConnectionString(option.ConnectionName);
                    break;
            }

            return connectionString;
        }

        public bool MatchTenantKey(string tenantKey)
        {
            return false;
        }
    }
}
