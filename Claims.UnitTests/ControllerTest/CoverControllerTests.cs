using Application.Commands;
using Application.Models.Dto;
using Application.Queries;
using Claims.Controllers;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Shared.Classes;

namespace Claims.UnitTests.ControllerTest;

public class CoverControllerTests
{
    private readonly Mock<IMediator> _mockMediator;
    private readonly CoversController _controller;

    public CoverControllerTests()
    {
        _mockMediator = new Mock<IMediator>();
        _controller = new CoversController(_mockMediator.Object);
    }

    [Fact]
    public async void GetAsync_should_return_covers()
    {
        // Arrange
        var expectedData = GetCovers();

        _mockMediator.Setup(m => m.Send(new GetCoversQuery(), default)).ReturnsAsync(expectedData);

        // Act
        var result = await _controller.GetAsync();

        // Assert
        result.Should().BeOfType<ActionResult<IEnumerable<CoverDto>>>();
    }

    [Fact]
    public async void GetAsync_by_id_should_return_cover()
    {
        // Arrange
        var expectedData = GetCovers().First();

        _mockMediator.Setup(m => m.Send(It.IsAny<GetCoverQuery>(), default)).ReturnsAsync(expectedData);

        // Act
        var result = await _controller.GetAsync(expectedData.Id);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
    }

    [Fact]
    public async void GetAsync_by_id_should_return_not_found()
    {
        // Arrange
        var coverId = Guid.NewGuid().ToString();

        _mockMediator.Setup(m => m.Send(new GetCoverQuery(coverId), default)).ReturnsAsync((CoverDto?)null);

        // Act
        var result = await _controller.GetAsync(coverId);

        // Assert
        result.Should().BeOfType<NotFoundResult>();
    }

    [Fact]
    public async void CreateAsync_should_return_created_cover()
    {
        // Arrange
        var expectedData = GetCovers().First();
        var command = new CreateCoverCommand(expectedData.StartDate, expectedData.EndDate, expectedData.Type);

        _mockMediator.Setup(m => m.Send(command, default)).ReturnsAsync(expectedData);

        // Act
        var result = await _controller.CreateAsync(command);

        // Assert
        result.Should().BeOfType<ActionResult<CoverDto>>();
    }

    [Fact]
    public async void DeleteAsync_should_return_ok()
    {
        // Arrange
        var id = Guid.NewGuid().ToString();

        _mockMediator.Setup(m => m.Send(new DeleteCoverCommand(id), default));

        // Act
        var result = await _controller.DeleteAsync(id);

        // Assert
        result.Should().BeOfType<OkResult>();
    }

    private List<CoverDto> GetCovers()
    {
        return new List<CoverDto>{
            new(){ Id = Guid.NewGuid().ToString(), StartDate = DateTime.UtcNow, EndDate = DateTime.UtcNow.AddMonths(7), Type = CoverType.Yacht, Premium = 10000 },
            new(){ Id = Guid.NewGuid().ToString(), StartDate = DateTime.UtcNow, EndDate = DateTime.UtcNow.AddMonths(8), Type = CoverType.Tanker, Premium = 20000 },
            new(){ Id = Guid.NewGuid().ToString(), StartDate = DateTime.UtcNow, EndDate = DateTime.UtcNow.AddMonths(9), Type = CoverType.PassengerShip, Premium = 30000 },
        };
    }
}
