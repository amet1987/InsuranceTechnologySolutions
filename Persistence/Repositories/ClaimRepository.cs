using Microsoft.EntityFrameworkCore;
using Persistence.Entities;
using Persistence.Interfaces;

namespace Persistence.Repositories;

public class ClaimRepository : BaseRepository<Claim>, IClaimRepository
{
    public ClaimRepository(PlatformDbContext platformDbContext) 
        : base(platformDbContext) {}

    public async Task<IList<Claim>> GetByCoverIdAsync(string coverId)
    {
        var query = _platformDbContext.Claims.AsQueryable();
        var result = await query.Where(c => c.CoverId == coverId).ToListAsync();

        return result;
    }
}
