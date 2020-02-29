using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace kiwiho.EFcore.MultiTenant.Core.Interface
{
    public interface ITenantDbContextManager
    {
        DbContextOptionsBuilder ConfigureDatabase<TBuilder, TExtension>(DbContextOptionsBuilder optionsBuilder, string connectionString, Action<RelationalDbContextOptionsBuilder<TBuilder, TExtension>> mySqlOptionsAction)
            where TBuilder : RelationalDbContextOptionsBuilder<TBuilder, TExtension>
            where TExtension : RelationalOptionsExtension, new();
    }
}
