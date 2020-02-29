using System;

namespace kiwiho.EFcore.MultiTenant.Model.Enum
{
    public enum ConnectionResolverType
    {
        Default = 0,
        ByDatabase = 1,
        ByTable = 2,
        BySchema = 3
        
    }
}
