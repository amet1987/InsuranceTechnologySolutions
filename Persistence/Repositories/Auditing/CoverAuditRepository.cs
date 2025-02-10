using Microsoft.EntityFrameworkCore;
using Persistence.Entities.Auditing;
using Persistence.Interfaces.Auditing;

namespace Persistence.Repositories.Auditing;

public class CoverAuditRepository : AuditBaseRepository<CoverAudit>, ICoverAuditRepository
{
    public CoverAuditRepository(IDbContextFactory<AuditDbContext> contextFactory) : base(contextFactory) {}
}
