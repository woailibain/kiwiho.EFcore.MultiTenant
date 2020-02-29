using System;
using kiwiho.EFcore.MultiTenant.Core.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace kiwiho.EFcore.MultiTenant.Core.Impl
{
    public class SqlServerDbContextManager : ITenantDbContextManager
    {
        public DbContextOptionsBuilder ConfigureDatabase<TBuilder, TExtension>(DbContextOptionsBuilder optionsBuilder,
            string connectionString,
            Action<RelationalDbContextOptionsBuilder<TBuilder, TExtension>> sqlServerOptionsAction)
            where TBuilder : RelationalDbContextOptionsBuilder<TBuilder, TExtension>
            where TExtension : RelationalOptionsExtension, new()
        {
            optionsBuilder.UseSqlServer(connectionString, sqlServerBuilder =>
            {
                sqlServerOptionsAction(sqlServerBuilder as RelationalDbContextOptionsBuilder<TBuilder, TExtension>);
            });

            return optionsBuilder;
        }
    }
}
