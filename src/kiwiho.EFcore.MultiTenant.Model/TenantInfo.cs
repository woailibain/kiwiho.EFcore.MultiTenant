using System;
using System.Collections.Generic;
using System.Dynamic;

namespace kiwiho.EFcore.MultiTenant.Model
{
    public class TenantInfo : DynamicObject
    {
        public string Name { get; set; }

        public bool IsPresent { get; set; }

        public object Generator { get; set; }

        public dynamic Container { get; set; } = new ExpandoObject();

        Dictionary<string, object> dictionary = new Dictionary<string, object>();

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            return dictionary.TryGetValue(binder.Name, out result);
        }

        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            dictionary[binder.Name] = value;
            return true;
        }

        public override bool TrySetIndex(SetIndexBinder binder, object[] indexes, object value)
        {
            if (!(indexes?.Length > 0))
                return false;
            dictionary[indexes[0].ToString()] = value;
            return true;
        }

        public override bool TryGetIndex(GetIndexBinder binder, object[] indexes, out object result)
        {
            result = null;
            if (!(indexes?.Length > 0))
                return false;
            result = dictionary[indexes[0].ToString()];
            return true;
        }
    }
}
