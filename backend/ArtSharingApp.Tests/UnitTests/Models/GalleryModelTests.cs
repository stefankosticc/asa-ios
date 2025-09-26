using System.ComponentModel.DataAnnotations;
using ArtSharingApp.Backend.Models;

namespace ArtSharingApp.Tests.UnitTests.Models;

public class GalleryModelTests
{
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void Name_FailsValidation_WhenNullOrEmpty(string name)
    {
        var gallery = new Gallery { Name = name, Address = "123 Main St", CityId = 1 };
        var context = new ValidationContext(gallery);
        Assert.Throws<ValidationException>(() => Validator.ValidateObject(gallery, context, true));
    }

    [Fact]
    public void Name_FailsValidation_WhenTooLong()
    {
        var gallery = new Gallery { Name = new string('a', 201), Address = "123 Main St", CityId = 1 };
        var context = new ValidationContext(gallery);
        Assert.Throws<ValidationException>(() => Validator.ValidateObject(gallery, context, true));
    }

    [Fact]
    public void Name_PassesValidation_WhenValid()
    {
        var gallery = new Gallery { Name = "Art Gallery", Address = "123 Main St", CityId = 1 };
        var context = new ValidationContext(gallery);
        Validator.ValidateObject(gallery, context, true);
    }

    [Fact]
    public void Name_PassesValidation_WhenMaxLength()
    {
        var gallery = new Gallery { Name = new string('a', 200), Address = "123 Main St", CityId = 1 };
        var context = new ValidationContext(gallery);
        Validator.ValidateObject(gallery, context, true);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void Address_FailsValidation_WhenNullOrEmpty(string address)
    {
        var gallery = new Gallery { Name = "Art Gallery", Address = address, CityId = 1 };
        var context = new ValidationContext(gallery);
        Assert.Throws<ValidationException>(() => Validator.ValidateObject(gallery, context, true));
    }

    [Fact]
    public void Address_FailsValidation_WhenTooLong()
    {
        var gallery = new Gallery { Name = "Art Gallery", Address = new string('a', 501), CityId = 1 };
        var context = new ValidationContext(gallery);
        Assert.Throws<ValidationException>(() => Validator.ValidateObject(gallery, context, true));
    }

    [Fact]
    public void Address_PassesValidation_WhenMaxLength()
    {
        var gallery = new Gallery { Name = "Art Gallery", Address = new string('a', 500), CityId = 1 };
        var context = new ValidationContext(gallery);
        Validator.ValidateObject(gallery, context, true);
    }

    [Fact]
    public void Address_PassesValidation_WhenValid()
    {
        var gallery = new Gallery { Name = "Art Gallery", Address = "123 Main St", CityId = 1 };
        var context = new ValidationContext(gallery);
        Validator.ValidateObject(gallery, context, true);
    }
}