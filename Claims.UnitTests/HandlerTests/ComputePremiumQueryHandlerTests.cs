using Application.Handlers.QueryHandlers;
using Application.Queries;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Shared.Classes;

namespace Claims.UnitTests.HandlerTests;

public class ComputePremiumQueryHandlerTests
{
    private readonly ComputePremiumQueryHandler _commandHandler;
    private readonly Mock<ILogger<ComputePremiumQueryHandler>> _logger;

    public ComputePremiumQueryHandlerTests()
    {
        _logger = new Mock<ILogger<ComputePremiumQueryHandler>>();
        _commandHandler = new ComputePremiumQueryHandler(_logger.Object);
    }

    [Fact]
    public async void Should_return_calculated_premium_for_Yacht()
    {
        // Arrange
        var query = new ComputePremiumQuery(DateTime.UtcNow, DateTime.UtcNow.AddMonths(10), CoverType.Yacht);

        // Act
        var result = await _commandHandler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().Be(394303.25000M);
    }

    [Fact]
    public async void Should_return_calculated_premium_for_PassengerShip()
    {
        // Arrange
        var query = new ComputePremiumQuery(DateTime.UtcNow, DateTime.UtcNow.AddMonths(10), CoverType.PassengerShip);

        // Act
        var result = await _commandHandler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().Be(445957.20000M);
    }
}
