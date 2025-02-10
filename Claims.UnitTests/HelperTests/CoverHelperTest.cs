using Application.Helpers;
using FluentAssertions;
using Shared.Classes;

namespace Claims.UnitTests.HelperTests;

public class CoverHelperTest
{
    [Fact]
    public void ComputePremium_for_Yacht_10_days_period()
    {
        // Arrange
        var starDate = DateTime.UtcNow;
        var endDate = DateTime.UtcNow.AddDays(10);

        // Act
        var result = CoverHelper.ComputePremium(starDate, endDate, CoverType.Yacht);

        // Assert
        result.Should().Be(15125.0M);
    }

    [Fact]
    public void ComputePremium_for_Yacht_5_months_period()
    {
        // Arrange
        var starDate = DateTime.UtcNow;
        var endDate = DateTime.UtcNow.AddMonths(5);

        // Act
        var result = CoverHelper.ComputePremium(starDate, endDate, CoverType.Yacht);

        // Assert
        result.Should().Be(199306.250M);
    }

    [Fact]
    public void ComputePremium_for_Yacht_7_months_period()
    {
        // Arrange
        var starDate = DateTime.UtcNow;
        var endDate = DateTime.UtcNow.AddMonths(7);

        // Act
        var result = CoverHelper.ComputePremium(starDate, endDate, CoverType.Yacht);

        // Assert
        result.Should().Be(279000.56250M);
    }

    [Fact]
    public void ComputePremium_for_PassengerShip_10_days_period()
    {
        // Arrange
        var starDate = DateTime.UtcNow;
        var endDate = DateTime.UtcNow.AddDays(10);

        // Act
        var result = CoverHelper.ComputePremium(starDate, endDate, CoverType.PassengerShip);

        // Assert
        result.Should().Be(16500.0M);
    }

    [Fact]
    public void ComputePremium_for_PassengerShip_5_months_period()
    {
        // Arrange
        var starDate = DateTime.UtcNow;
        var endDate = DateTime.UtcNow.AddMonths(5);

        // Act
        var result = CoverHelper.ComputePremium(starDate, endDate, CoverType.PassengerShip);

        // Assert
        result.Should().Be(222870.000M);
    }

    [Fact]
    public void ComputePremium_for_PassengerShip_7_months_period()
    {
        // Arrange
        var starDate = DateTime.UtcNow;
        var endDate = DateTime.UtcNow.AddMonths(7);

        // Act
        var result = CoverHelper.ComputePremium(starDate, endDate, CoverType.PassengerShip);

        // Assert
        result.Should().Be(313524.90000M);
    }
}
