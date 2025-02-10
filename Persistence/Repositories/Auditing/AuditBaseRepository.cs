using Microsoft.EntityFrameworkCore;
using Persistence.Entities.Auditing;
using Persistence.Interfaces.Auditing;

namespace Persistence.Repositories.Auditing;

public class AuditBaseRepository<T> : IAuditRepository<T> where T : EntityAudit
{
    protected readonly IDbContextFactory<AuditDbContext> _contextFactory;

    public AuditBaseRepository(IDbContextFactory<AuditDbContext> contextFactory)
    {
        _contextFactory = contextFactory;
    }

    public async Task<T> CreateAsync(T entity)
    {
        using var dbContext = _contextFactory.CreateDbContext();

        _ = await dbContext.Set<T>().AddAsync(entity);
        await dbContext.SaveChangesAsync();

        return entity;
    }
}
