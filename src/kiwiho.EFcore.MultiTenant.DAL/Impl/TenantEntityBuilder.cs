using System;
using kiwiho.EFcore.MultiTenant.DAL.Interface;
using kiwiho.EFcore.MultiTenant.Model;
using kiwiho.EFcore.MultiTenant.Model.Enum;
using Microsoft.EntityFrameworkCore;

namespace kiwiho.EFcore.MultiTenant.DAL.Impl
{
    public class TenantEntityBuilder<TDbContext> : ITenantEntityBuilder<TDbContext>
        where TDbContext : DbContext
    {
        private readonly IEntityScaner<TDbContext> entityScaner;
        private readonly TenantOption tenantOption;

        private readonly TenantInfo tenantInfo;
        public TenantEntityBuilder(IEntityScaner<TDbContext> entityScaner, TenantOption tenantOption, TenantInfo tenantInfo)
        {
            this.tenantInfo = tenantInfo;
            this.tenantOption = tenantOption;
            this.entityScaner = entityScaner;
        }

        public void UpdateEntities(ModelBuilder modelBuilder)
        {
            var dbsetProperties = entityScaner.ScanEntityTypes();

            foreach (var property in dbsetProperties)
            {
                var entity = modelBuilder.Entity(property.PropertyType);
                switch (this.tenantOption.ConnectionType)
                {
                    case ConnectionResolverType.BySchema:
                        var tableName = this.tenantOption.TableNameFunc?.Invoke(this.tenantInfo, property.PropertyName)
                            ?? property.PropertyName;
                        var schemaName = this.tenantOption.SchemaFunc?.Invoke(this.tenantInfo)
                            ?? this.tenantInfo.Name;
                        entity.ToTable(tableName, schemaName);
                        break;
                    case ConnectionResolverType.ByTable:
                        tableName = this.tenantOption.TableNameFunc?.Invoke(this.tenantInfo, property.PropertyName)
                            ?? $"{this.tenantInfo.Name}_{property.PropertyName}";
                        entity.ToTable(tableName);
                        break;
                    default:
                        break;
                }
            }
        }

    }
}
