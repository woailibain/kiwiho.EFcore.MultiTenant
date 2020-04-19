using System;
using kiwiho.EFcore.MultiTenant.Core.Interface;
using kiwiho.EFcore.MultiTenant.Model;
using Microsoft.Extensions.Configuration;

namespace kiwiho.EFcore.MultiTenant.Example.Read_Write.Generator
{
    public class ReadWriteConnectionGenerator : IConnectionGenerator
    {

        static Lazy<Random> random = new Lazy<Random>();
        private readonly IConfiguration configuration;
        public string TenantKey => "";

        public ReadWriteConnectionGenerator(IConfiguration configuration)
        {
            this.configuration = configuration;
        }


        public string GetConnection(TenantOption option, TenantInfo tenantInfo)
        {
            dynamic info = tenantInfo;
            if (info?.IsWrite == true)
            {
                return configuration.GetConnectionString($"{option.ConnectionPrefix}write");
            }
            else
            {
                var mod = random.Value.Next(1000) % 2;
                return configuration.GetConnectionString($"{option.ConnectionPrefix}read{(mod + 1)}");
            }
        }

        public bool MatchTenantKey(string tenantKey)
        {
            return true;
        }
    }
}
