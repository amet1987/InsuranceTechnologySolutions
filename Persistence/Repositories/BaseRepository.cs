using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using Persistence.Entities;
using Persistence.Interfaces;

namespace Persistence.Repositories;

public class BaseRepository<T> : IRepository<T> where T : Entity
{
    protected readonly PlatformDbContext _platformDbContext;

    public BaseRepository(PlatformDbContext platformDbContext)
    {
        _platformDbContext = platformDbContext;
    }

    public async Task<T> CreateAsync(T entity)
    {
        await _platformDbContext.Set<T>().AddAsync(entity);
        await _platformDbContext.SaveChangesAsync();

        return entity;
    }

    public async Task<T> GetAsync(string id)
    {
        var query = _platformDbContext.Set<T>().AsQueryable();
        var result = await query.SingleOrDefaultAsync(x => x.Id == id);

        return result;
    }

    public async Task<IList<T>> GetAsync()
    {
        var query = _platformDbContext.Set<T>().AsQueryable();
        var result = await query.ToListAsync();

        return result;
    }

    public async Task DeleteAsync(T entity)
    {
        _ = _platformDbContext.Remove(entity);
        await _platformDbContext.SaveChangesAsync();
    }
}
