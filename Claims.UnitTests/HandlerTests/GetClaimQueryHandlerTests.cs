using Application.Handlers.QueryHandlers;
using Application.Models.Dto;
using Application.Queries;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Persistence.Interfaces;

namespace Claims.UnitTests.HandlerTests;

public class GetClaimQueryHandlerTests : BaseTests
{
    private readonly GetClaimQueryHandler _commandHandler;
    private readonly Mock<IClaimRepository> _claimRepositoryMock;
    private readonly Mock<ILogger<GetClaimQueryHandler>> _logger;

    public GetClaimQueryHandlerTests()
    {
        _claimRepositoryMock = new Mock<IClaimRepository>();
        _logger = new Mock<ILogger<GetClaimQueryHandler>>();
        _commandHandler = new GetClaimQueryHandler(_claimRepositoryMock.Object, _logger.Object);
    }

    [Fact]
    public async void Should_return_claim_by_id()
    {
        // Arrange
        var expectedData = GetClaim();

        _claimRepositoryMock.Setup(x => x.GetAsync(It.IsAny<string>())).ReturnsAsync(expectedData);

        // Act
        var result = await _commandHandler.Handle(new GetClaimQuery(expectedData.Id), CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<ClaimDto>();
        result.Should().BeEquivalentTo(expectedData);
    }

    [Fact]
    public async void Should_return_null_when_claim_not_found()
    {
        // Arrange
        var expectedData = GetClaim();

        _claimRepositoryMock.Setup(x => x.GetAsync(expectedData.Id)).ReturnsAsync(expectedData);

        // Act
        var result = await _commandHandler.Handle(new GetClaimQuery(Guid.NewGuid().ToString()), CancellationToken.None);

        // Assert
        result.Should().BeNull();
    }
}
