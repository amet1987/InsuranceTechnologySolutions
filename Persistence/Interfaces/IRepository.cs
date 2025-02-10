using Persistence.Entities;

namespace Persistence.Interfaces;

public interface IRepository<T> where T : Entity
{
    /// <summary>
    /// Create a new record in the database
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    Task<T> CreateAsync(T entity);

    /// <summary>
    /// Get a record from the database
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    Task<T> GetAsync(string id);

    /// <summary>
    /// Get all records from the database
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    Task<IList<T>> GetAsync();

    /// <summary>
    /// Delete a record from the database
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    Task DeleteAsync(T identity);
}
