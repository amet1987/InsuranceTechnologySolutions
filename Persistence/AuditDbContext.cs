using Microsoft.EntityFrameworkCore;
using Persistence.Entities.Auditing;

namespace Persistence;

public class AuditDbContext : DbContext
{
    public DbSet<ClaimAudit> ClaimAudits { get; set; }
    public DbSet<CoverAudit> CoverAudits { get; set; }

    public AuditDbContext(DbContextOptions<AuditDbContext> options) 
        : base(options) {}

    public Task<int> SaveChangesAsync()
    {
        foreach (var entry in ChangeTracker.Entries())
        {
            var entity = entry.Entity;

            if (entity is EntityAudit && entry.State == EntityState.Added)
            {
                var e = entity as EntityAudit;
                e.Created = DateTime.UtcNow;
            }
        }
        return base.SaveChangesAsync();
    }

}
