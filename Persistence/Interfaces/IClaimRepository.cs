using Persistence.Entities;

namespace Persistence.Interfaces;

public interface IClaimRepository : IRepository<Claim> 
{
    /// <summary>
    /// Get claims by cover id.
    /// </summary>
    /// <param name="coverId"></param>
    /// <returns></returns>
    Task<IList<Claim>> GetByCoverIdAsync(string coverId);
}
