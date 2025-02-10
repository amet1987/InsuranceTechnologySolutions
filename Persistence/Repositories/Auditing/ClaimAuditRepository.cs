using Microsoft.EntityFrameworkCore;
using Persistence.Entities.Auditing;
using Persistence.Interfaces.Auditing;

namespace Persistence.Repositories.Auditing;

public class ClaimAuditRepository : AuditBaseRepository<ClaimAudit>, IClaimAuditRepository
{
    public ClaimAuditRepository(IDbContextFactory<AuditDbContext> contextFactory) : base(contextFactory) {}
}
