using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using kiwiho.EFcore.MultiTenant.DAL.Interface;
using kiwiho.EFcore.MultiTenant.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace kiwiho.EFcore.MultiTenant.DAL.Impl
{
    public class SimpleEntityScaner<TDbContext> : IEntityScaner<TDbContext>
        where TDbContext : DbContext
    {
        private static List<DbsetProperty> dbProperties;
        private static object locker = new object();

        public IList<DbsetProperty> ScanEntityTypes()
        {
            if (dbProperties == null)
            {
                lock (locker)
                {
                    if (dbProperties == null)
                    {
                        var contextType = typeof(TDbContext);
                        var properties = contextType.GetProperties();
                        var entityProperties = properties.Where(r => IsDbSet(r)).ToList();

                        dbProperties = new List<DbsetProperty>();
                        foreach (var property in entityProperties)
                        {
                            dbProperties.Add(new DbsetProperty
                            {
                                PropertyName = property.Name,
                                PropertyType = property.PropertyType.GetGenericArguments()[0]
                            });
                        }
                    }
                }
            }
            return dbProperties;
        }

        // public IList<EntityTypeBuilder> ScanEntities(IList<Type> types, ModelBuilder modelBuilder)
        // {
        //     return types?.Select(r => modelBuilder.Entity(r)).ToList() ?? new List<EntityTypeBuilder>();
        // }

        bool IsDbSet(PropertyInfo property)
        {
            if (property.CanRead && property.PropertyType.IsGenericType
                && typeof(DbSet<>).GetGenericTypeDefinition().Equals(property.PropertyType.GetGenericTypeDefinition()))
            {
                return true;
            }
            return false;
        }
    }
}
