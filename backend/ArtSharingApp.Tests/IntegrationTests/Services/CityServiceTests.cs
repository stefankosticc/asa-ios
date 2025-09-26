using ArtSharingApp.Backend.DTO;
using ArtSharingApp.Backend.Exceptions;
using ArtSharingApp.Backend.Models;
using ArtSharingApp.Backend.Service.ServiceInterface;
using ArtSharingApp.Tests.IntegrationTests.Services.Utils;
using Microsoft.Extensions.DependencyInjection;

namespace ArtSharingApp.Tests.IntegrationTests.Services;

public class CityServiceTests : IntegrationTestBase
{
    [Fact]
    public async Task GetAllAsync_ReturnsEmpty_WhenNoCitiesExist()
    {
        // Arrange
        var service = ServiceProvider!.GetRequiredService<ICityService>();

        // Act
        var result = await service.GetAllAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Fact]
    public async Task GetAllAsync_ReturnsCities_WhenCitiesExist()
    {
        // Arrange
        var service = ServiceProvider!.GetRequiredService<ICityService>();
        var context = DbContext!;
        context.Cities.Add(new City { Name = "Paris", Country = "France" });
        context.Cities.Add(new City { Name = "Berlin", Country = "Germany" });
        await context.SaveChangesAsync();

        // Act
        var result = (await service.GetAllAsync()).ToList();

        // Assert
        Assert.Equal(2, result.Count);
        Assert.Contains(result, c => c.Name == "Paris");
        Assert.Contains(result, c => c.Name == "Berlin");
    }

    [Fact]
    public async Task AddAsync_CreatesCity()
    {
        // Arrange
        var service = ServiceProvider!.GetRequiredService<ICityService>();
        var context = DbContext!;
        var dto = new CityRequestDTO { Name = "London", Country = "UK" };

        // Act
        await service.AddAsync(dto);

        // Assert
        Assert.True(context.Cities.Any(c => c.Name == "London" && c.Country == "UK"));
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsCity_WhenExists()
    {
        // Arrange
        var service = ServiceProvider!.GetRequiredService<ICityService>();
        var context = DbContext!;
        var city = new City { Name = "London", Country = "UK" };
        context.Cities.Add(city);
        await context.SaveChangesAsync();

        // Act
        var result = await service.GetByIdAsync(city.Id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("London", result.Name);
    }

    [Fact]
    public async Task GetByIdAsync_ThrowsNotFound_WhenCityDoesNotExist()
    {
        // Arrange
        var service = ServiceProvider!.GetRequiredService<ICityService>();

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => service.GetByIdAsync(999));
    }

    [Fact]
    public async Task UpdateAsync_UpdatesCity()
    {
        // Arrange
        var service = ServiceProvider!.GetRequiredService<ICityService>();
        var context = DbContext!;
        var city = new City { Name = "OldName", Country = "OldCountry" };
        context.Cities.Add(city);
        await context.SaveChangesAsync();

        var dto = new CityRequestDTO { Name = "NewName", Country = "NewCountry" };

        // Act
        await service.UpdateAsync(city.Id, dto);

        // Assert
        var updated = await context.Cities.FindAsync(city.Id);
        Assert.NotNull(updated);
        Assert.Equal("NewName", updated.Name);
        Assert.Equal("NewCountry", updated.Country);
    }

    [Fact]
    public async Task UpdateAsync_ThrowsNotFound_WhenCityDoesNotExist()
    {
        // Arrange
        var service = ServiceProvider!.GetRequiredService<ICityService>();
        var dto = new CityRequestDTO { Name = "Name", Country = "Country" };

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => service.UpdateAsync(999, dto));
    }

    [Fact]
    public async Task DeleteAsync_RemovesCity()
    {
        // Arrange
        var service = ServiceProvider!.GetRequiredService<ICityService>();
        var context = DbContext!;
        var city = new City { Name = "DeleteMe", Country = "Nowhere" };
        context.Cities.Add(city);
        await context.SaveChangesAsync();

        // Act
        await service.DeleteAsync(city.Id);

        // Assert
        Assert.False(context.Cities.Any(c => c.Id == city.Id));
    }

    [Fact]
    public async Task DeleteAsync_ThrowsNotFound_WhenCityDoesNotExist()
    {
        // Arrange
        var service = ServiceProvider!.GetRequiredService<ICityService>();

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => service.DeleteAsync(999));
    }

    [Fact]
    public async Task GetCitiesByName_ReturnsCities()
    {
        // Arrange
        var service = ServiceProvider!.GetRequiredService<ICityService>();
        var context = DbContext!;
        context.Cities.Add(new City { Name = "Madrid", Country = "Spain" });
        context.Cities.Add(new City { Name = "Madrid", Country = "Colombia" });
        await context.SaveChangesAsync();

        // Act
        var result = (await service.GetCitiesByName("Madrid"))!.ToList();

        // Assert
        Assert.Equal(2, result.Count);
        Assert.All(result, c => Assert.Equal("Madrid", c.Name));
    }

    [Fact]
    public async Task GetCitiesByName_ReturnsEmpty_WhenNoMatch()
    {
        // Arrange
        var service = ServiceProvider!.GetRequiredService<ICityService>();

        // Act
        var result = await service.GetCitiesByName("NonExistentCity");

        // Assert
        if (result != null) Assert.Empty(result);
        else Assert.Null(result);
    }

    [Fact]
    public async Task GetArtworksByCityId_ReturnsArtworks()
    {
        // Arrange
        var service = ServiceProvider!.GetRequiredService<ICityService>();
        var context = DbContext!;
        context.Users.Add(CreateHelper.CreateUser(1));

        var city = new City { Name = "London", Country = "UK" };
        context.Cities.Add(city);
        await context.SaveChangesAsync();

        var artwork = CreateHelper.CreateArtwork("CityArtwork", 1, 1, cityId: city.Id);
        context.Artworks.Add(artwork);
        await context.SaveChangesAsync();

        // Act
        var result = (await service.GetArtworksByCityId(city.Id))!.ToList();

        // Assert
        Assert.Single(result);
        Assert.Equal("CityArtwork", result[0].Title);
    }

    [Fact]
    public async Task GetArtworksByCityId_ThrowsNotFound_WhenCityDoesNotExist()
    {
        // Arrange
        var service = ServiceProvider!.GetRequiredService<ICityService>();

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => service.GetArtworksByCityId(999));
    }

    [Fact]
    public async Task GetGalleriesByCityId_ReturnsGalleries()
    {
        // Arrange
        var service = ServiceProvider!.GetRequiredService<ICityService>();
        var context = DbContext!;
        var city = new City { Name = "GalleryCity", Country = "GalleryLand" };
        context.Cities.Add(city);
        await context.SaveChangesAsync();

        var gallery = CreateHelper.CreateGallery("MainGallery", "Main Gallery Adress", city.Id);
        context.Galleries.Add(gallery);
        await context.SaveChangesAsync();

        // Act
        var result = (await service.GetGalleriesByCityId(city.Id))!.ToList();

        // Assert
        Assert.Single(result);
        Assert.Equal("MainGallery", result[0].Name);
    }

    [Fact]
    public async Task GetGalleriesByCityId_ThrowsNotFound_WhenCityDoesNotExist()
    {
        // Arrange
        var service = ServiceProvider!.GetRequiredService<ICityService>();

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => service.GetGalleriesByCityId(999));
    }
}