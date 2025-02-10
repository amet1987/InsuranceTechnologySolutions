using Persistence.Entities.Auditing;

namespace Persistence.Interfaces.Auditing;

public interface IAuditRepository<T> where T : EntityAudit
{
    /// <summary>
    /// Creates a new audit record in the database
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    Task<T> CreateAsync(T entity);
}
