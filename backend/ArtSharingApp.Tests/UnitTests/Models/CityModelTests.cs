using System.ComponentModel.DataAnnotations;
using ArtSharingApp.Backend.Models;

namespace ArtSharingApp.Tests.UnitTests.Models;

public class CityModelTests
{
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void Name_FailsWhenNullOrEmpty(string name)
    {
        var city = new City { Name = name, Country = "TestCountry" };
        var context = new ValidationContext(city);
        Assert.Throws<ValidationException>(() => Validator.ValidateObject(city, context, true));
    }

    [Fact]
    public void Name_SucceedsWhenValid()
    {
        var city = new City { Name = "TestCity", Country = "TestCountry" };
        var context = new ValidationContext(city);
        Validator.ValidateObject(city, context, true);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void Country_FailsWhenNullOrEmpty(string country)
    {
        var city = new City { Name = "TestCity", Country = country };
        var context = new ValidationContext(city);
        Assert.Throws<ValidationException>(() => Validator.ValidateObject(city, context, true));
    }

    [Fact]
    public void Country_SucceedsWhenValid()
    {
        var city = new City { Name = "TestCity", Country = "TestCountry" };
        var context = new ValidationContext(city);
        Validator.ValidateObject(city, context, true);
    }
}