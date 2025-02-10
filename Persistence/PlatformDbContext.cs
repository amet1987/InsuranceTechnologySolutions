using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using MongoDB.EntityFrameworkCore.Extensions;
using Persistence.Entities;

namespace Persistence;

public class PlatformDbContext : DbContext
{
    public DbSet<Claim> Claims { get; init; }
    public DbSet<Cover> Covers { get; init; }


    public PlatformDbContext(DbContextOptions options)
        : base(options) {}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Claim>().ToCollection("claims");
        modelBuilder.Entity<Cover>().ToCollection("covers");
    }
}
