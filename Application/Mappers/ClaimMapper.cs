using Application.Models.Dto;
using Persistence.Entities;

namespace Application.Mappers;

public static class ClaimMapper
{
    public static ClaimDto? MapToDto(this Claim claim)
    {
        if (claim is null) return null;

        return new ClaimDto
        {
            Id = claim.Id,
            CoverId = claim.CoverId,
            Created = claim.Created,
            Name = claim.Name,
            Type = claim.Type,
            DamageCost = claim.DamageCost
        };
    }

    public static IEnumerable<ClaimDto> MapToDto(this IList<Claim> claims)
    {
        return claims.Select(claim => new ClaimDto
        {
            Id = claim.Id,
            CoverId = claim.CoverId,
            Created = claim.Created,
            Name = claim.Name,
            Type = claim.Type,
            DamageCost = claim.DamageCost
        });
    }
}
