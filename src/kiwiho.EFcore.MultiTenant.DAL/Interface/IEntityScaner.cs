using System;
using System.Collections.Generic;
using kiwiho.EFcore.MultiTenant.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace kiwiho.EFcore.MultiTenant.DAL.Interface
{
    public interface IEntityScaner<out TDbContext>
        where TDbContext: DbContext
    {
        IList<DbsetProperty> ScanEntityTypes();
        
        // IList<EntityTypeBuilder> ScanEntities(IList<Type> types, ModelBuilder modelBuilder);
    }
}
