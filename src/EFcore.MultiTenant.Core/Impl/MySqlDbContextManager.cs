using System;
using kiwiho.EFcore.MultiTenant.Core.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace kiwiho.EFcore.MultiTenant.Core.Impl
{
    public class MySqlDbContextManager : ITenantDbContextManager
    {
        public DbContextOptionsBuilder ConfigureDatabase<TBuilder, TExtension>(DbContextOptionsBuilder optionsBuilder, 
            string connectionString, 
            Action<RelationalDbContextOptionsBuilder<TBuilder, TExtension>> mySqlOptionsAction)
            where TBuilder : RelationalDbContextOptionsBuilder<TBuilder, TExtension>
            where TExtension : RelationalOptionsExtension, new()
        {
            optionsBuilder.UseMySql(connectionString, mySqlBuilder =>
            {
                mySqlOptionsAction(mySqlBuilder as RelationalDbContextOptionsBuilder<TBuilder, TExtension>);
            });

            return optionsBuilder;
        }
    }

    
}
