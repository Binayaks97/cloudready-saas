using CloudReady.Application.Interfaces;
using CloudReady.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CloudReady.Infrastructure.Persistence
{
    public class AppDbContext : DbContext
    {
        private readonly ITenantProvider _tenantProvider;

        public AppDbContext(
            DbContextOptions<AppDbContext> options,
            ITenantProvider tenantProvider)
            : base(options)
        {
            _tenantProvider = tenantProvider;
        }

        public DbSet<User> Users => Set<User>();
        public DbSet<Tenant> Tenants => Set<Tenant>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // 🔒 Tenant isolation with admin bypass
            modelBuilder.Entity<User>()
                .HasQueryFilter(u =>
                    _tenantProvider.IsAdmin() ||
                    u.TenantCode == _tenantProvider.GetTenantCode());

            base.OnModelCreating(modelBuilder);
        }
    }
}
