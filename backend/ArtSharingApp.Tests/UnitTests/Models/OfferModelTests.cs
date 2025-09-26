using System.ComponentModel.DataAnnotations;
using ArtSharingApp.Backend.Models;
using ArtSharingApp.Backend.Models.Enums;

namespace ArtSharingApp.Tests.UnitTests.Models;

public class OfferModelTests
{
    [Theory]
    [InlineData(-1)]
    [InlineData(0)]
    public void Amount_FailsValidationWhenNegativeOrZero(int amount)
    {
        var offer = new Offer { Amount = -1 };
        var context = new ValidationContext(offer);
        Assert.Throws<ValidationException>(() => Validator.ValidateObject(offer, context, true));
    }

    [Fact]
    public void Amount_PassesValidationWhenPositive()
    {
        var offer = new Offer { Amount = 10.00m };
        var context = new ValidationContext(offer);
        Validator.ValidateObject(offer, context, true);
        Assert.Equal(10.00m, offer.Amount);
    }

    [Fact]
    public void Amount_PassesValidationWhenMaxValue()
    {
        var offer = new Offer { Amount = decimal.MaxValue };
        var context = new ValidationContext(offer);
        Validator.ValidateObject(offer, context, true);
        Assert.Equal(decimal.MaxValue, offer.Amount);
    }

    [Fact]
    public void Acceept_SetsStatusToAccepted()
    {
        var offer = new Offer { Status = OfferStatus.SUBMITTED };
        offer.Accept();
        Assert.Equal(OfferStatus.ACCEPTED, offer.Status);
    }

    [Fact]
    public void Reject_SetsStatusToRejected()
    {
        var offer = new Offer { Status = OfferStatus.SUBMITTED };
        offer.Reject();
        Assert.Equal(OfferStatus.REJECTED, offer.Status);
    }

    [Fact]
    public void Withdraw_SetsStatusToWithdrawn()
    {
        var offer = new Offer { Status = OfferStatus.SUBMITTED };
        offer.Withdraw();
        Assert.Equal(OfferStatus.WITHDRAWN, offer.Status);
    }

    [Fact]
    public void CanBeModified_ReturnsTrueWhenStatusIsSubmitted()
    {
        var offer = new Offer { Status = OfferStatus.SUBMITTED };
        Assert.True(offer.CanBeModified());
    }

    [Fact]
    public void CanBeModified_ReturnsFalseWhenStatusIsNotSubmitted()
    {
        var offer = new Offer { Status = OfferStatus.ACCEPTED };
        Assert.False(offer.CanBeModified());
    }
}