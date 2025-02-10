using Application.Commands;
using Application.Handlers.CommandHandlers;
using Application.Models.Dto;
using FluentAssertions;
using MassTransit;
using Microsoft.Extensions.Logging;
using Moq;
using Persistence.Entities;
using Persistence.Interfaces;
using Shared.Classes;
using Shared.Exceptions;

namespace Claims.UnitTests.HandlerTests;

public class CreateClaimCommandHandlerTests : BaseTests
{
    private readonly CreateClaimCommandHandler _commandHandler;
    private readonly Mock<IClaimRepository> _claimRepositoryMock;
    private readonly Mock<ICoverRepository> _coverRepositoryMock;
    private readonly Mock<IBus> _busMock;
    private readonly Mock<ILogger<CreateClaimCommandHandler>> _logger;

    public CreateClaimCommandHandlerTests()
    {
        _claimRepositoryMock = new Mock<IClaimRepository>();
        _coverRepositoryMock = new Mock<ICoverRepository>();
        _busMock = new Mock<IBus>();
        _logger = new Mock<ILogger<CreateClaimCommandHandler>>();
        _commandHandler = new CreateClaimCommandHandler(_claimRepositoryMock.Object, _coverRepositoryMock.Object, _busMock.Object, _logger.Object);
    }

    [Fact]
    public async void Should_return_created_claim()
    {
        // Arrange
        var expectedData = GetClaim();

        var cover = new Cover
        {
            Id = expectedData.CoverId,
            StartDate = DateTime.UtcNow.AddMonths(-2),
            EndDate = DateTime.UtcNow.AddMonths(10),
            Type = CoverType.Yacht,
            Premium = 80000
        };

        _claimRepositoryMock.Setup(x => x.CreateAsync(It.IsAny<Claim>())).ReturnsAsync(expectedData);
        _coverRepositoryMock.Setup(x => x.GetAsync(cover.Id)).ReturnsAsync(cover);

        // Act
        var result = await _commandHandler.Handle(new CreateClaimCommand(expectedData.CoverId, expectedData.Name, 
            expectedData.Type, expectedData.DamageCost), CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<ClaimDto>();
        result.Should().BeEquivalentTo(expectedData);
    }

    [Fact]
    public async void Should_throw_validation_exception()
    {
        // Arrange
        var expectedData = GetClaim();

        var cover = new Cover
        {
            Id = Guid.NewGuid().ToString(),
            StartDate = DateTime.UtcNow.AddMonths(2),
            EndDate = DateTime.UtcNow.AddMonths(12),
            Type = CoverType.Yacht,
            Premium = 1000000
        };

        _claimRepositoryMock.Setup(x => x.CreateAsync(It.IsAny<Claim>())).ReturnsAsync(expectedData);
        _coverRepositoryMock.Setup(x => x.GetAsync(cover.Id)).ReturnsAsync(cover);

        // Act
        var  assert = async () =>  await _commandHandler.Handle(new CreateClaimCommand(expectedData.CoverId, expectedData.Name,
            expectedData.Type, expectedData.DamageCost), CancellationToken.None);

        // Assert
        await assert.Should().ThrowAsync<ValidationException>();
    }
}
