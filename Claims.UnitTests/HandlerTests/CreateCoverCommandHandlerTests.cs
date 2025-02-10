using Application.Commands;
using Application.Handlers.CommandHandlers;
using Application.Models.Dto;
using FluentAssertions;
using MassTransit;
using Microsoft.Extensions.Logging;
using Moq;
using Persistence.Entities;
using Persistence.Interfaces;
using Shared.Exceptions;

namespace Claims.UnitTests.HandlerTests;

public class CreateCoverCommandHandlerTests : BaseTests
{
    private readonly CreateCoverCommandHandler _commandHandler;
    private readonly Mock<ICoverRepository> _coverRepositoryMock;
    private readonly Mock<IBus> _busMock;
    private readonly Mock<ILogger<CreateCoverCommandHandler>> _logger;

    public CreateCoverCommandHandlerTests()
    {
        _coverRepositoryMock = new Mock<ICoverRepository>();
        _busMock = new Mock<IBus>();
        _logger = new Mock<ILogger<CreateCoverCommandHandler>>();
        _commandHandler = new CreateCoverCommandHandler(_coverRepositoryMock.Object, _busMock.Object, _logger.Object);
    }

    [Fact]
    public async void Should_return_created_cover()
    {
        // Arrange
        var expectedData = GetCover();

        _coverRepositoryMock.Setup(x => x.CreateAsync(It.IsAny<Cover>())).ReturnsAsync(expectedData);

        // Act
        var result = await _commandHandler.Handle(new CreateCoverCommand(expectedData.StartDate, expectedData.EndDate,
            expectedData.Type), CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<CoverDto>();
        result.Should().BeEquivalentTo(expectedData);
    }

    [Fact]
    public async void Should_throw_validation_exception()
    {
        // Arrange
        var expectedData = GetCover();

        // Act
        var assert = async () => await _commandHandler.Handle(new CreateCoverCommand(expectedData.StartDate.AddMonths(-2), expectedData.EndDate,
            expectedData.Type), CancellationToken.None);

        // Assert
        await assert.Should().ThrowAsync<ValidationException>();
    }
}
