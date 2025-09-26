using System.ComponentModel.DataAnnotations;
using ArtSharingApp.Backend.Models;
using ArtSharingApp.Backend.Models.Enums;

namespace ArtSharingApp.Tests.UnitTests.Models;

public class AuctionModelTests
{
    [Fact]
    public void StartingPrice_FailsValidationWhenNegative()
    {
        var auction = new Auction { StartingPrice = -1 };
        var context = new ValidationContext(auction);
        Assert.Throws<ValidationException>(() => Validator.ValidateObject(auction, context, true));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(100.99)]
    public void StartingPrice_PassesValidationWhenZeroOrPositive(decimal price)
    {
        var auction = new Auction { StartingPrice = price };
        var context = new ValidationContext(auction);
        Validator.ValidateObject(auction, context, true);
        Assert.Equal(price, auction.StartingPrice);
    }

    [Fact]
    public void StartingPrice_PassesValidationWhenMaxValue()
    {
        var auction = new Auction { StartingPrice = decimal.MaxValue };
        var context = new ValidationContext(auction);
        Validator.ValidateObject(auction, context, true);
        Assert.Equal(decimal.MaxValue, auction.StartingPrice);
    }

    [Theory]
    [InlineData(-10, 10)] // start time in the past, end time in the future
    [InlineData(0, 20)] // starts now, ends in the future
    [InlineData(-20, 0)] // starts in the past, ends now
    public void IsActive_ReturnsTrue_WhenCurrentTimeIsBetweenStartAndEnd(int startOffset, int endOffset)
    {
        var auction = new Auction
        {
            StartTime = DateTime.UtcNow.AddMinutes(startOffset),
            EndTime = DateTime.UtcNow.AddMinutes(endOffset)
        };

        Assert.True(auction.IsActive());
    }

    [Theory]
    [InlineData(10, 20)] // start time in the future, end time in the future
    [InlineData(-20, -10)] // start time in the past, end time in the past
    [InlineData(1, 10)] // Just before StartTime
    [InlineData(-20, -1)] // Just after EndTime
    public void IsActive_ReturnsFalse_WhenCurrentTimeIsNotBetweenStartAndEnd(int startOffset, int endOffset)
    {
        var auction = new Auction
        {
            StartTime = DateTime.UtcNow.AddMinutes(startOffset),
            EndTime = DateTime.UtcNow.AddMinutes(endOffset)
        };

        Assert.False(auction.IsActive());
    }

    [Fact]
    public void UpdateEndTime_SuccessfullyUpdatesEndTime()
    {
        var auction = new Auction
        {
            StartTime = DateTime.UtcNow.AddMinutes(-10),
            EndTime = DateTime.UtcNow.AddMinutes(10)
        };

        var newEndTime = DateTime.UtcNow.AddMinutes(20);
        auction.UpdateEndTime(newEndTime);
        Assert.Equal(newEndTime, auction.EndTime);
    }

    [Fact]
    public void UpdateEndTime_ThrowsException_WhenNewEndTimeIsBeforeStartTime()
    {
        var auction = new Auction
        {
            StartTime = DateTime.UtcNow.AddMinutes(-10),
            EndTime = DateTime.UtcNow.AddMinutes(10),
        };

        var newEndTime = auction.StartTime.AddMinutes(-30);
        Assert.Throws<ArgumentException>(() => auction.UpdateEndTime(newEndTime));
    }

    [Fact]
    public void UpdateEndTime_ThrowsException_WhenNewEndTimeIsEqualToStartTime()
    {
        var auction = new Auction
        {
            StartTime = DateTime.UtcNow.AddMinutes(-10),
            EndTime = DateTime.UtcNow.AddMinutes(10),
        };
        var newEndTime = auction.StartTime;
        Assert.Throws<ArgumentException>(() => auction.UpdateEndTime(newEndTime));
    }
}