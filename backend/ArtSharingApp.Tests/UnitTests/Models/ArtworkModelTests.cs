using ArtSharingApp.Backend.Models;
using ArtSharingApp.Backend.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace ArtSharingApp.Tests.UnitTests.Models;

public class ArtworkModelTests
{
    [Fact]
    public void ChangeVisibility_SetsIsPrivateCorrectly()
    {
        var artwork = new Artwork { IsPrivate = false };
        artwork.ChangeVisibility(true);
        Assert.True(artwork.IsPrivate);

        artwork.ChangeVisibility(false);
        Assert.False(artwork.IsPrivate);
    }

    [Fact]
    public void PutOnSale_SetsPropertiesCorrectly()
    {
        var artwork = new Artwork { IsOnSale = false, Price = null, Currency = Currency.EUR };
        artwork.PutOnSale(100, Currency.USD);

        Assert.True(artwork.IsOnSale);
        Assert.Equal(100, artwork.Price);
        Assert.Equal(Currency.USD, artwork.Currency);
    }

    [Fact]
    public void PutOnSale_NegativePrice_ThrowsException()
    {
        var artwork = new Artwork();
        Assert.Throws<ArgumentOutOfRangeException>(() => artwork.PutOnSale(-1, Currency.EUR));
    }

    [Fact]
    public void RemoveFromSale_ResetsSalePropertiesCorrectly()
    {
        var artwork = new Artwork { IsOnSale = true, Price = 50 };
        artwork.RemoveFromSale();

        Assert.False(artwork.IsOnSale);
        Assert.Null(artwork.Price);
    }

    [Fact]
    public void TransferOwnership_UpdatesPostedByUserId()
    {
        var artwork = new Artwork { PostedByUserId = 1 };
        artwork.TransferOwnership(42);

        Assert.Equal(42, artwork.PostedByUserId);
    }

    [Theory]
    [InlineData("#FFF")]
    [InlineData("#ffffff")]
    [InlineData("#123456")]
    public void Color_ValidHex_PassesValidation(string color)
    {
        var artwork = new Artwork { Title = "Test", Image = new byte[1], Color = color };
        var ctx = new ValidationContext(artwork);
        Validator.ValidateObject(artwork, ctx, validateAllProperties: true);
    }

    [Theory]
    [InlineData("red")] // Named color
    [InlineData("#FFFFF")] // Invalid hex (too short)
    [InlineData("rgba(255, 0, 0, 1)")] // Invalid format
    [InlineData("#1234567")] // Invalid hex (too long)
    [InlineData("123456")] // Missing hash
    public void Color_InvalidHex_FailsValidation(string color)
    {
        var artwork = new Artwork { Title = "Test", Image = new byte[1], Color = color };
        var ctx = new ValidationContext(artwork);
        Assert.Throws<ValidationException>(() => Validator.ValidateObject(artwork, ctx, true));
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void Title_Required_FailsValidationIfNullOrEmpty(string? title)
    {
        var artwork = new Artwork { Title = title, Image = new byte[1] };
        var ctx = new ValidationContext(artwork);
        Assert.Throws<ValidationException>(() => Validator.ValidateObject(artwork, ctx, true));
    }

    [Fact]
    public void Title_TooLong_FailsValidation()
    {
        var artwork = new Artwork { Title = new string('a', 201), Image = new byte[1] };
        var ctx = new ValidationContext(artwork);
        Assert.Throws<ValidationException>(() => Validator.ValidateObject(artwork, ctx, true));
    }

    [Fact]
    public void Title_MaxLength_PassesValidation()
    {
        var artwork = new Artwork { Title = new string('a', 200), Image = new byte[1] };
        var ctx = new ValidationContext(artwork);
        Validator.ValidateObject(artwork, ctx, validateAllProperties: true);
    }

    [Fact]
    public void Image_Required_FailsValidationIfNull()
    {
        var artwork = new Artwork { Title = "Test" };
        var ctx = new ValidationContext(artwork);
        Assert.Throws<ValidationException>(() => Validator.ValidateObject(artwork, ctx, true));
    }

    [Fact]
    public void Image_Required_PassesValidationIfNotNull()
    {
        var artwork = new Artwork { Title = "Test", Image = new byte[1] };
        var ctx = new ValidationContext(artwork);
        Validator.ValidateObject(artwork, ctx, validateAllProperties: true);
    }

    [Fact]
    public void Price_FailsValidationIfNegative()
    {
        var artwork = new Artwork
        {
            Title = "Test",
            Image = new byte[1],
            IsOnSale = true,
            Price = -1,
            Currency = Currency.EUR
        };
        var ctx = new ValidationContext(artwork);
        Assert.Throws<ValidationException>(() => Validator.ValidateObject(artwork, ctx, true));
    }

    [Theory]
    [InlineData(0)] // Zero price
    [InlineData(100)] // Positive price
    public void Price_PassesValidationIfZeroOrPositive(decimal price)
    {
        var artwork = new Artwork
        {
            Title = "Test",
            Image = new byte[1],
            IsOnSale = true,
            Price = price,
            Currency = Currency.EUR
        };
        var ctx = new ValidationContext(artwork);
        Validator.ValidateObject(artwork, ctx, validateAllProperties: true);
    }

    [Fact]
    public void Price_PassesValidationIfMaxValue()
    {
        var artwork = new Artwork
        {
            Title = "Test",
            Image = new byte[1],
            IsOnSale = true,
            Price = decimal.MaxValue,
            Currency = Currency.EUR
        };
        var ctx = new ValidationContext(artwork);
        Validator.ValidateObject(artwork, ctx, validateAllProperties: true);
    }
}