using System;
using kiwiho.EFcore.MultiTenant.Core.Interface;
using kiwiho.EFcore.MultiTenant.Model;
using Microsoft.Extensions.Configuration;

namespace kiwiho.EFcore.MultiTenant.MixMode.Generator
{
    public class CombindedConnectionGenerator : IConnectionGenerator
    {
        private readonly IConfiguration configuration;
        public string TenantKey => "";

        public CombindedConnectionGenerator(IConfiguration configuration)
        {
            this.configuration = configuration;
        }


        public string GetConnection(TenantOption option, TenantInfo tenantInfo)
        {
            var span = tenantInfo.Name.AsSpan();
            if (span.Length > 4 && int.TryParse(span[5].ToString(), out var number))
            {
                return configuration.GetConnectionString($"{option.ConnectionPrefix}container{number % 2 + 1}");
            }
            throw new NotSupportedException("tenant invalid");
        }

        public bool MatchTenantKey(string tenantKey)
        {
            return true;
        }
    }
}