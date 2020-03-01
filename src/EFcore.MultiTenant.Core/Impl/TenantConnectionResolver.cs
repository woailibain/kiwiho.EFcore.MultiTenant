using System;
using System.Collections.Generic;
using System.Linq;
using kiwiho.EFcore.MultiTenant.Core.Interface;
using kiwiho.EFcore.MultiTenant.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace kiwiho.EFcore.MultiTenant.Core.Impl
{
    public class TenantConnectionResolver<TDbContext> : ITenantConnectionResolver<TDbContext>
        where TDbContext : DbContext
    {
        private readonly TenantSettings<TDbContext> setting;
        private readonly TenantInfo tenantInfo;
        private readonly IServiceProvider serviceProvider;
        private readonly TenantOption tenantOption;

        public TenantConnectionResolver(TenantSettings<TDbContext> setting, TenantInfo tenantInfo,
            TenantOption tenantOption, IServiceProvider serviceProvider)
        {
            this.tenantOption = tenantOption;
            this.setting = setting;
            this.tenantInfo = tenantInfo;
            this.serviceProvider = serviceProvider;
        }

        public string GetConnection()
        {
            var connectionGenerator = this.GetConnectionGenerator();
            var option = GenerateOption();

            return connectionGenerator.GetConnection(option, tenantInfo);

        }

        IConnectionGenerator GetConnectionGenerator()
        {
            IConnectionGenerator connectionGenerator = this.setting.ConnectionGenerator?.Invoke()
                ?? serviceProvider.GetServices<IConnectionGenerator>()
                    .FirstOrDefault(r => r.TenantKey == this.setting.Key || r.MatchTenantKey(this.setting.Key))
                ?? serviceProvider.GetService<NamedConnectionGenerator>();

            return connectionGenerator;
        }

        TenantOption GenerateOption()
        {
            tenantOption.Key = setting.Key;
            tenantOption.ConnectionType = setting.ConnectionType;
            tenantOption.DbType = setting.DbType;
            tenantOption.TableNameFunc = setting.TableNameFunc;
            tenantOption.SchemaFunc = setting.SchemaFunc;
            tenantOption.ConnectionPrefix = setting.ConnectionPrefix;
            tenantOption.ConnectionName = setting.ConnectionName;
        
            return this.tenantOption;
        }
    }
}
