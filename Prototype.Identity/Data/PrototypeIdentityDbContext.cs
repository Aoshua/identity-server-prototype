using Microsoft.EntityFrameworkCore;
using Prototype.Identity.Data.Models;

namespace Prototype.Identity.Data
{
    public class PrototypeIdentityDbContext : DbContext
    {
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Tenant> Tenants { get; set; }

        public PrototypeIdentityDbContext(DbContextOptions<PrototypeIdentityDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}
