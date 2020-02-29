using System;
using System.Dynamic;

namespace kiwiho.EFcore.MultiTenant.Model
{
    public class TenantInfo : DynamicObject
    {
        public string Name { get; set; }

        public bool IsPresent { get; set; }

        public Type GeneratorType { get; set; }
    }
}
