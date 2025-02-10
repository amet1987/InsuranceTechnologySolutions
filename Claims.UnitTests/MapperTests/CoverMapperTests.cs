using Application.Mappers;
using FluentAssertions;

namespace Claims.UnitTests.MapperTests;

public class CoverMapperTests : BaseTests
{
    [Fact]
    public void Mapping_Claim_to_ClaimDto()
    {
        // Arrange
        var cover = GetCover();

        // Act
        var result = cover.MapToDto();

        // Assert
        result?.Id.Should().Be(cover.Id);
        result?.StartDate.Should().Be(cover.StartDate);
        result?.EndDate.Should().Be(cover.EndDate);
        result?.Type.Should().Be(cover.Type);
        result?.Premium.Should().Be(cover.Premium);
    }
}
