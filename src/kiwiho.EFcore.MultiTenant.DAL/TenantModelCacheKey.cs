using System;
using kiwiho.EFcore.MultiTenant.DAL.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace kiwiho.EFcore.MultiTenant.DAL
{
    internal sealed class TenantModelCacheKey<TDbContext> : ModelCacheKey
        where TDbContext : DbContext, ITenantDbContext
    {
        private readonly TDbContext context;
        private readonly string identifier;
        public TenantModelCacheKey(TDbContext context, string identifier) : base(context)
        {
            this.context = context;
            this.identifier = identifier;
        }

        protected override bool Equals(ModelCacheKey other)
        {
            return base.Equals(other) && (other as TenantModelCacheKey<TDbContext>)?.identifier == identifier;
        }

        public override int GetHashCode()
        {
            var hashCode = base.GetHashCode();
            if (identifier != null)
            {
                hashCode ^= identifier.GetHashCode();
            }

            return hashCode;
        }
    }
}
