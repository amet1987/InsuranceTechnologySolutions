using Application.Commands;
using Application.Handlers.CommandHandlers;
using FluentAssertions;
using MassTransit;
using Microsoft.Extensions.Logging;
using Moq;
using Persistence.Entities;
using Persistence.Interfaces;

namespace Claims.UnitTests.HandlerTests;

public class DeleteClaimCommandHandlerTests : BaseTests
{
    private readonly DeleteClaimCommandHandler _commandHandler;
    private readonly Mock<IClaimRepository> _coverRepositoryMock;
    private readonly Mock<IBus> _busMock;
    private readonly Mock<ILogger<DeleteClaimCommandHandler>> _logger;

    public DeleteClaimCommandHandlerTests()
    {
        _coverRepositoryMock = new Mock<IClaimRepository>();
        _busMock = new Mock<IBus>();
        _logger = new Mock<ILogger<DeleteClaimCommandHandler>>();
        _commandHandler = new DeleteClaimCommandHandler(_coverRepositoryMock.Object, _busMock.Object, _logger.Object);
    }

    [Fact]
    public async void Should_not_throw_exception()
    {
        // Arrange
        var claim = GetClaim();

        _coverRepositoryMock.Setup(x => x.GetAsync(It.IsAny<string>())).ReturnsAsync(claim);
        _coverRepositoryMock.Setup(x => x.DeleteAsync(It.IsAny<Claim>()));

        // Act
        var assert = async () => await _commandHandler.Handle(new DeleteClaimCommand(claim.Id), CancellationToken.None);

        // Assert
        await assert.Should().NotThrowAsync();
    }

    [Fact]
    public async void Should_throw_exception_if_claim_not_found()
    {
        // Arrange
        var claim = GetClaim();

        _coverRepositoryMock.Setup(x => x.GetAsync(claim.Id)).ReturnsAsync(claim);
        _coverRepositoryMock.Setup(x => x.DeleteAsync(It.IsAny<Claim>()));

        // Act
        var assert = async () => await _commandHandler.Handle(new DeleteClaimCommand(Guid.NewGuid().ToString()), CancellationToken.None);

        // Assert
        await assert.Should().ThrowAsync<KeyNotFoundException>();
    }
}
