using System;
using kiwiho.EFcore.MultiTenant.Core.Interface;
using kiwiho.EFcore.MultiTenant.Model;
using Microsoft.AspNetCore.Http;

namespace kiwiho.EFcore.MultiTenant.Core.Impl
{
    public class SimpleHeaderTenantInfoGenerator : ITenantInfoGenerator
    {
        private readonly TenantInfo tenantInfo;
        public SimpleHeaderTenantInfoGenerator(TenantInfo tenantInfo)
        {
            this.tenantInfo = tenantInfo;
        }

        public TenantInfo GenerateTenant(HttpContext httpContext)
        {
            if (!this.tenantInfo.IsPresent)
            {
                var tenantName = httpContext?.Request?.Headers["TenantName"];

                if (!string.IsNullOrEmpty(tenantName))
                {
                    this.tenantInfo.IsPresent = true;
                    this.tenantInfo.Name = tenantName;
                    this.tenantInfo.GeneratorType = this.GetType();
                }
            }

            return this.tenantInfo;
        }
    }
}
