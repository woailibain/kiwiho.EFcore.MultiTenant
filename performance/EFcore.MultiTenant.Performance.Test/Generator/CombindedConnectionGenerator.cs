using System;
using kiwiho.EFcore.MultiTenant.Core.Interface;
using kiwiho.EFcore.MultiTenant.Model;
using Microsoft.Extensions.Configuration;

namespace EFcore.MultiTenant.Performance.Test.Generator
{
    public class CombindedConnectionGenerator : IConnectionGenerator
    {
        private string dbcontainerFormat;

        private int containerCount = 5;
        public string TenantKey => "";

        public CombindedConnectionGenerator(IConfiguration configuration)
        {
            dbcontainerFormat = configuration.GetConnectionString("mysql_containers");
        }


        public string GetConnection(TenantOption option, TenantInfo tenantInfo)
        {
            var span = tenantInfo.Name.AsSpan();
            if (span.Length > 4 && int.TryParse(span.Slice(5, span.Length - 5).ToString(), out var number))
            {
                return String.Format(dbcontainerFormat, number % containerCount);
            }
            throw new NotSupportedException("tenant invalid");
        }

        public bool MatchTenantKey(string tenantKey)
        {
            return true;
        }
    }
}