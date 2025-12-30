using CloudReady.Application.Interfaces;
using CloudReady.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

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

        public DbSet<Tenant> Tenants => Set<Tenant>();
        public DbSet<User> Users => Set<User>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasQueryFilter(u =>
                    string.IsNullOrEmpty(_tenantProvider.GetTenantCode()) ||
                    u.TenantCode == _tenantProvider.GetTenantCode());

            base.OnModelCreating(modelBuilder);
        }
    }
}
