using Application.Handlers.QueryHandlers;
using Application.Models.Dto;
using Application.Queries;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Persistence.Interfaces;

namespace Claims.UnitTests.HandlerTests;

public class GetCoverQueryHandlerTests : BaseTests
{
    private readonly GetCoverQueryHandler _commandHandler;
    private readonly Mock<ICoverRepository> _coverRepositoryMock;
    private readonly Mock<ILogger<GetCoverQueryHandler>> _logger;

    public GetCoverQueryHandlerTests()
    {
        _coverRepositoryMock = new Mock<ICoverRepository>();
        _logger = new Mock<ILogger<GetCoverQueryHandler>>();
        _commandHandler = new GetCoverQueryHandler(_coverRepositoryMock.Object, _logger.Object);
    }

    [Fact]
    public async void Should_return_cover_by_id()
    {
        // Arrange
        var expectedData = GetCover();

        _coverRepositoryMock.Setup(x => x.GetAsync(It.IsAny<string>())).ReturnsAsync(expectedData);

        // Act
        var result = await _commandHandler.Handle(new GetCoverQuery(expectedData.Id), CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<CoverDto>();
        result.Should().BeEquivalentTo(expectedData);
    }

    [Fact]
    public async void Should_return_null_when_cover_not_found()
    {
        // Arrange
        var expectedData = GetCover();

        _coverRepositoryMock.Setup(x => x.GetAsync(expectedData.Id)).ReturnsAsync(expectedData);

        // Act
        var result = await _commandHandler.Handle(new GetCoverQuery(Guid.NewGuid().ToString()), CancellationToken.None);

        // Assert
        result.Should().BeNull();
    }
}
