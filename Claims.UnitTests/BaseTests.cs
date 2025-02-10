using Persistence.Entities;
using Shared.Classes;

namespace Claims.UnitTests;

public class BaseTests
{
    public static Claim GetClaim()
    {
        return new Claim
        {
            Id = Guid.NewGuid().ToString(),
            CoverId = Guid.NewGuid().ToString(),
            Created = DateTime.UtcNow,
            Name = "Test Name",
            Type = ClaimType.Collision,
            DamageCost = 80000
        };
    }

    public static List<Claim> GetClaims()
    {
        return new List<Claim>
        {
            new()
            {
                 Id = Guid.NewGuid().ToString(),
                CoverId = Guid.NewGuid().ToString(),
                Created = DateTime.UtcNow,
                Name = "Test Name 1",
                Type = ClaimType.Collision,
                DamageCost = 80000
            },
             new()
            {
                 Id = Guid.NewGuid().ToString(),
                CoverId = Guid.NewGuid().ToString(),
                Created = DateTime.UtcNow,
                Name = "Test Name 2",
                Type = ClaimType.Collision,
                DamageCost = 90000
            }
        };
    }

    public static Cover GetCover()
    {
        return new Cover
        {
            Id = Guid.NewGuid().ToString(),
            StartDate = DateTime.UtcNow,
            EndDate = DateTime.UtcNow.AddMonths(10),
            Type = CoverType.Yacht,
            Premium = 80000
        };
    }

    public static List<Cover> GetCovers()
    {
        return new List<Cover>
        {
            new()
            {
                  Id = Guid.NewGuid().ToString(),
            StartDate = DateTime.UtcNow,
            EndDate = DateTime.UtcNow.AddMonths(10),
            Type = CoverType.Yacht,
            Premium = 80000
            },
             new()
            {
                 Id = Guid.NewGuid().ToString(),
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow.AddMonths(10),
                Type = CoverType.Yacht,
                Premium = 90000
            }
        };
    }
}
