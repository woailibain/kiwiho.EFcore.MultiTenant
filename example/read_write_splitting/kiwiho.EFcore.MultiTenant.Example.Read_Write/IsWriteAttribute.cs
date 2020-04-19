using System;
using System.Diagnostics;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;

namespace kiwiho.EFcore.MultiTenant.Example.Read_Write
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public class IsWriteAttribute : Attribute
    {
        public IsWriteAttribute()
        {
        }
    }
}
