using System;
using System.Threading.Tasks;
using kiwiho.EFcore.MultiTenant.Core.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace kiwiho.EFcore.MultiTenant.Core
{
    public class TenantInfoMiddleware
    {
        private readonly RequestDelegate _next;

        public TenantInfoMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, ITenantInfoGenerator tenantInfoGenerator)
        {
            tenantInfoGenerator.GenerateTenant(this, context);

            // Call the next delegate/middleware in the pipeline
            await _next(context);
        }
    }
}
