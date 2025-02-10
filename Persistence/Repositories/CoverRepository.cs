using Microsoft.EntityFrameworkCore;
using Persistence.Entities;
using Persistence.Interfaces;

namespace Persistence.Repositories;

public class CoverRepository : BaseRepository<Cover>, ICoverRepository
{
    public CoverRepository(PlatformDbContext platformDbContext) 
        : base(platformDbContext) {}
}
