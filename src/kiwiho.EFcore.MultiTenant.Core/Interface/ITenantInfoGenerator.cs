using System;
using kiwiho.EFcore.MultiTenant.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;

namespace kiwiho.EFcore.MultiTenant.Core.Interface
{
    public interface ITenantInfoGenerator
    {
        TenantInfo GenerateTenant(object sender, HttpContext httpContext);
    }

}
