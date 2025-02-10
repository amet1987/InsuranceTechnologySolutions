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

public class ClaimControllerTests
{
    private readonly Mock<IMediator> _mockMediator;
    private readonly ClaimsController _controller;

    public ClaimControllerTests()
    {
        _mockMediator = new Mock<IMediator>();
        _controller = new ClaimsController(_mockMediator.Object);
    }

    [Fact]
    public async void GetAsync_should_return_claims()
    {
        // Arrange
        var expectedData = GetClaims();

        _mockMediator.Setup(m => m.Send(It.IsAny<GetClaimsQuery>(), default)).ReturnsAsync(expectedData);

        // Act
        var result = await _controller.GetAsync();

        // Assert
        result.Should().BeOfType<ActionResult<IEnumerable<ClaimDto>>>();
    }

    [Fact]
    public async void GetAsync_by_id_should_return_claim()
    {
        // Arrange
        var expectedData = GetClaims().First();

        _mockMediator.Setup(m => m.Send(It.IsAny<GetClaimQuery>(), default)).ReturnsAsync(expectedData);

        // Act
        var result = await _controller.GetAsync(expectedData.Id);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
    }

    [Fact]
    public async void GetAsync_by_id_should_return_not_found()
    {
        // Arrange
        var claimId = Guid.NewGuid().ToString();

        _mockMediator.Setup(m => m.Send(new GetClaimQuery(claimId), default)).ReturnsAsync((ClaimDto?)null);

        // Act
        var result = await _controller.GetAsync(claimId);

        // Assert
        result.Should().BeOfType<NotFoundResult>();
    }

    [Fact]
    public async void CreateAsync_should_return_created_claim()
    {
        // Arrange
        var expectedData = GetClaims().First();
        var command = new CreateClaimCommand(expectedData.CoverId, expectedData.Name, expectedData.Type, expectedData.DamageCost);

        _mockMediator.Setup(m => m.Send(command, default)).ReturnsAsync(expectedData);

        // Act
        var result = await _controller.CreateAsync(command);

        // Assert
        result.Should().BeOfType<ActionResult<ClaimDto>>();
    }

    [Fact]
    public async void DeleteAsync_should_return_ok()
    {
        // Arrange
        var id = Guid.NewGuid().ToString();

        _mockMediator.Setup(m => m.Send(new DeleteClaimCommand(id), default));

        // Act
        var result = await _controller.DeleteAsync(id);

        // Assert
        result.Should().BeOfType<OkResult>();
    }

    private List<ClaimDto> GetClaims()
    {
        return new List<ClaimDto>{
            new () { Id = Guid.NewGuid().ToString(), CoverId = Guid.NewGuid().ToString(), 
                Created = DateTime.UtcNow, Name = "Test Name 1", Type = ClaimType.Collision, DamageCost = 70000 },
            new () { Id = Guid.NewGuid().ToString(), CoverId = Guid.NewGuid().ToString(),
                Created = DateTime.UtcNow, Name = "Test Name 2", Type = ClaimType.Collision, DamageCost = 80000 },
            new () { Id = Guid.NewGuid().ToString(), CoverId = Guid.NewGuid().ToString(),
                Created = DateTime.UtcNow, Name = "Test Name 3", Type = ClaimType.Collision, DamageCost = 90000 },
        };
    }
}
