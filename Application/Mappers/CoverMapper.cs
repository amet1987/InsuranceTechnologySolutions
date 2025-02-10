using Application.Models.Dto;
using Persistence.Entities;

namespace Application.Mappers;

public static class CoverMapper
{
    public static CoverDto? MapToDto(this Cover cover)
    {
        if (cover is null) return null;

        return new CoverDto
        {
            Id = cover.Id,
            StartDate = cover.StartDate,
            EndDate = cover.EndDate,
            Type = cover.Type,
            Premium = cover.Premium,
        };
    }

    public static IEnumerable<CoverDto> MapToDto(this IList<Cover> covers)
    {
        return covers.Select(cover => new CoverDto
        {
            Id = cover.Id,
            StartDate = cover.StartDate,
            EndDate = cover.EndDate,
            Type = cover.Type,
            Premium = cover.Premium,
        });
    }
}
