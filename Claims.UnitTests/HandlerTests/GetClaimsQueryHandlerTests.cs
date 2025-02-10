using Application.Handlers.QueryHandlers;
using Application.Queries;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Persistence.Entities;
using Persistence.Interfaces;
using Shared.Classes;

namespace Claims.UnitTests.HandlerTests;

public class GetClaimsQueryHandlerTests : BaseTests
{
    private readonly GetClaimsQueryHandler _commandHandler;
    private readonly Mock<IClaimRepository> _claimRepositoryMock;
    private readonly Mock<ILogger<GetClaimsQueryHandler>> _logger;

    public GetClaimsQueryHandlerTests()
    {
        _claimRepositoryMock = new Mock<IClaimRepository>();
        _logger = new Mock<ILogger<GetClaimsQueryHandler>>();
        _commandHandler = new GetClaimsQueryHandler(_claimRepositoryMock.Object, _logger.Object);
    }

    [Fact]
    public async void Should_return_all_claims()
    {
        // Arrange
        var expectedData = GetClaims();

        _claimRepositoryMock.Setup(x => x.GetAsync()).ReturnsAsync(expectedData);

        // Act
        var result = await _commandHandler.Handle(new GetClaimsQuery(), CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(2);
    }

    [Fact]
    public async void Should_return_empty_list()
    {
        // Arrange
        _claimRepositoryMock.Setup(x => x.GetAsync()).ReturnsAsync(new List<Claim>());

        // Act
        var result = await _commandHandler.Handle(new GetClaimsQuery(), CancellationToken.None);

        // Assert
        result.Should().HaveCount(0);
    }
}
