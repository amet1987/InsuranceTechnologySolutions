using Application.Mappers;
using FluentAssertions;

namespace Claims.UnitTests.MapperTests;

public class ClaimMapperTests : BaseTests
{
    [Fact]
    public void Mapping_Claim_to_ClaimDto()
    {
        // Arrange
        var claim = GetClaim();

        // Act
        var result = claim.MapToDto();

        // Assert
        result?.Id.Should().Be(claim.Id);
        result?.CoverId.Should().Be(claim.CoverId);
        result?.Created.Should().Be(claim.Created);
        result?.Name.Should().Be(claim.Name);
        result?.Type.Should().Be(claim.Type);
        result?.DamageCost.Should().Be(claim.DamageCost);
    }
}
