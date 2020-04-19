using System;
using kiwiho.EFcore.MultiTenant.Model;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Controllers;

namespace kiwiho.EFcore.MultiTenant.Example.Read_Write.Provider
{
    public class ReadWriteActionInvokerProvider : IActionInvokerProvider
    {
        public int Order => 10;

        public void OnProvidersExecuted(ActionInvokerProviderContext context)
        {
        }

        public void OnProvidersExecuting(ActionInvokerProviderContext context)
        {
            if (context.ActionContext.ActionDescriptor is ControllerActionDescriptor descriptor)
            {
                var serviceProvider = context.ActionContext.HttpContext.RequestServices;
                var isWrite = descriptor.MethodInfo.GetCustomAttributes(typeof(IsWriteAttribute), false)?.Length > 0;

                var tenantInfo = serviceProvider.GetService(typeof(TenantInfo)) as TenantInfo;
                tenantInfo.Name = isWrite ? "WRITE" : "READ";
                (tenantInfo as dynamic).IsWrite = isWrite;
            }
        }
    }
}
