using Application.Handlers.QueryHandlers;
using Application.Queries;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Persistence.Entities;
using Persistence.Interfaces;

namespace Claims.UnitTests.HandlerTests;

public class GetCoversQueryHandlerTests : BaseTests
{
    private readonly GetCoversQueryHandler _commandHandler;
    private readonly Mock<ICoverRepository> _covermRepositoryMock;
    private readonly Mock<ILogger<GetCoversQueryHandler>> _logger;

    public GetCoversQueryHandlerTests()
    {
        _covermRepositoryMock = new Mock<ICoverRepository>();
        _logger = new Mock<ILogger<GetCoversQueryHandler>>();
        _commandHandler = new GetCoversQueryHandler(_covermRepositoryMock.Object, _logger.Object);
    }

    [Fact]
    public async void Should_return_all_covers()
    {
        // Arrange
        var expectedData = GetCovers();

        _covermRepositoryMock.Setup(x => x.GetAsync()).ReturnsAsync(expectedData);

        // Act
        var result = await _commandHandler.Handle(new GetCoversQuery(), CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(2);
    }

    [Fact]
    public async void Should_return_empty_list()
    {
        // Arrange
        _covermRepositoryMock.Setup(x => x.GetAsync()).ReturnsAsync(new List<Cover>());

        // Act
        var result = await _commandHandler.Handle(new GetCoversQuery(), CancellationToken.None);

        // Assert
        result.Should().HaveCount(0);
    }
}
