using System;
using System.Collections.Generic;
using kiwiho.EFcore.MultiTenant.Model.Enum;

namespace kiwiho.EFcore.MultiTenant.Core
{
    public class Constants
    {
        internal static List<ConnectionResolverType> specialConnectionTypes = new List<ConnectionResolverType>
        {
            ConnectionResolverType.ByTable, 
            ConnectionResolverType.BySchema
        };

    }
}
