using Application.Commands;
using Application.Handlers.CommandHandlers;
using FluentAssertions;
using MassTransit;
using Microsoft.Extensions.Logging;
using Moq;
using Persistence.Entities;
using Persistence.Interfaces;

namespace Claims.UnitTests.HandlerTests;

public class DeleteCoverCommandHandlerTests : BaseTests
{
    private readonly DeleteCoverCommandHandler _commandHandler;
    private readonly Mock<ICoverRepository> _coverRepositoryMock;
    private readonly Mock<IClaimRepository> _claimRepositoryMock;
    private readonly Mock<IBus> _busMock;
    private readonly Mock<ILogger<DeleteCoverCommandHandler>> _logger;

    public DeleteCoverCommandHandlerTests()
    {
        _coverRepositoryMock = new Mock<ICoverRepository>();
        _claimRepositoryMock = new Mock<IClaimRepository>();
        _busMock = new Mock<IBus>();
        _logger = new Mock<ILogger<DeleteCoverCommandHandler>>();
        _commandHandler = new DeleteCoverCommandHandler(_coverRepositoryMock.Object, _claimRepositoryMock.Object, _busMock.Object, _logger.Object);
    }

    [Fact]
    public async void Should_not_throw_exception()
    {
        // Arrange
        var cover = GetCover();

        var claims = new List<Claim>();

        _coverRepositoryMock.Setup(x => x.GetAsync(It.IsAny<string>())).ReturnsAsync(cover);
        _claimRepositoryMock.Setup(x => x.GetByCoverIdAsync(It.IsAny<string>())).ReturnsAsync(claims);

        // Act
        var assert = async () => await _commandHandler.Handle(new DeleteCoverCommand(cover.Id), CancellationToken.None);

        // Assert
        await assert.Should().NotThrowAsync();
    }

    [Fact]
    public async void Should_throw_exception_if_claim_not_found()
    {
        // Arrange
        var cover = GetCover();

        var claims = GetClaims();

        _coverRepositoryMock.Setup(x => x.GetAsync(It.IsAny<string>())).ReturnsAsync(cover);
        _claimRepositoryMock.Setup(x => x.GetByCoverIdAsync(It.IsAny<string>())).ReturnsAsync(claims);

        // Act
        var assert = async () => await _commandHandler.Handle(new DeleteCoverCommand(cover.Id), CancellationToken.None);

        // Assert
        await assert.Should().ThrowAsync<Exception>();
    }
}
