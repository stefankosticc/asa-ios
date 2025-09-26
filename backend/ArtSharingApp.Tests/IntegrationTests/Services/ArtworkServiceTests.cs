using ArtSharingApp.Backend.DTO;
using ArtSharingApp.Backend.Exceptions;
using ArtSharingApp.Backend.Models.Enums;
using ArtSharingApp.Backend.Service.ServiceInterface;
using ArtSharingApp.Tests.IntegrationTests.Services.Utils;
using Microsoft.Extensions.DependencyInjection;

namespace ArtSharingApp.Tests.IntegrationTests.Services;

public class ArtworkServiceTests : IntegrationTestBase
{
    [Fact]
    public async Task GetAllAsync_ReturnsEmpty_WhenNoArtworksExist()
    {
        // Arrange
        var service = ServiceProvider!.GetRequiredService<IArtworkService>();

        // Act
        var result = await service.GetAllAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Fact]
    public async Task GetAllAsync_ReturnsArtworks_WhenArtworksExist()
    {
        // Arrange
        var service = ServiceProvider!.GetRequiredService<IArtworkService>();
        var context = DbContext!;

        context.Users.Add(CreateHelper.CreateUser(1));
        context.Users.Add(CreateHelper.CreateUser(2));
        await context.SaveChangesAsync();
        var artwork1 = CreateHelper.CreateArtwork("Test1", 1, 1);
        var artwork2 = CreateHelper.CreateArtwork("Test2", 2, 2);
        context.Artworks.Add(artwork1);
        context.Artworks.Add(artwork2);
        await context.SaveChangesAsync();

        // Act
        var result = (await service.GetAllAsync()).ToList();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
        Assert.Contains(result, a => a.Title == "Test1");
        Assert.Contains(result, a => a.Title == "Test2");
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsArtwork_WhenExists()
    {
        // Arrange
        var service = ServiceProvider!.GetRequiredService<IArtworkService>();
        var context = DbContext!;
        context.Users.Add(CreateHelper.CreateUser(1));
        var artwork = CreateHelper.CreateArtwork("TestGetById", 1, 1);
        context.Artworks.Add(artwork);
        await context.SaveChangesAsync();

        // Act
        var result = await service.GetByIdAsync(artwork.Id, 1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("TestGetById", result.Title);
    }

    [Fact]
    public async Task GetByIdAsync_ThrowsNotFound_WhenArtworkDoesNotExist()
    {
        var service = ServiceProvider!.GetRequiredService<IArtworkService>();
        await Assert.ThrowsAsync<NotFoundException>(async () =>
            await service.GetByIdAsync(999, 1));
    }

    [Fact]
    public async Task AddAsync_CreatesArtwork()
    {
        // Arrange
        var service = ServiceProvider!.GetRequiredService<IArtworkService>();
        var context = DbContext!;
        context.Users.Add(CreateHelper.CreateUser(1));
        await context.SaveChangesAsync();
        var artworkDto = new ArtworkRequestDTO
        {
            Title = "Added",
            Story = "Story",
            TipsAndTricks = "Tips",
            CreatedByArtistId = 1,
            PostedByUserId = 1
        };
        using (var ms = new System.IO.MemoryStream(new byte[] { 1 }))
        {
            var image = new Microsoft.AspNetCore.Http.FormFile(ms, 0, ms.Length, "image", "image.jpg")
            {
                Headers = new Microsoft.AspNetCore.Http.HeaderDictionary(),
                ContentType = "image/jpeg"
            };

            // Act
            await service.AddAsync(artworkDto, image);
        }

        // Assert
        Assert.True(context.Artworks.Any(a => a.Title == "Added"));
    }

    [Fact]
    public async Task UpdateAsync_UpdatesArtwork()
    {
        // Arrange
        var service = ServiceProvider!.GetRequiredService<IArtworkService>();
        var context = DbContext!;
        context.Users.Add(CreateHelper.CreateUser(1));
        var artwork = CreateHelper.CreateArtwork("ToUpdate", 1, 1);
        context.Artworks.Add(artwork);
        await context.SaveChangesAsync();
        var dto = new ArtworkRequestDTO
        {
            Title = "Updated",
            Story = "Story",
            TipsAndTricks = "Tips",
            CreatedByArtistId = 1,
            PostedByUserId = 1
        };

        // Act
        await service.UpdateAsync(artwork.Id, dto, null);
        var updated = await context.Artworks.FindAsync(artwork.Id);

        // Assert
        Assert.NotNull(updated);
        Assert.Equal("Updated", updated.Title);
    }

    [Fact]
    public async Task DeleteAsync_RemovesArtwork()
    {
        // Arrange
        var service = ServiceProvider!.GetRequiredService<IArtworkService>();
        var context = DbContext!;
        context.Users.Add(CreateHelper.CreateUser(1));
        var artwork = CreateHelper.CreateArtwork("ToDelete", 1, 1);
        context.Artworks.Add(artwork);
        await context.SaveChangesAsync();

        // Act
        await service.DeleteAsync(artwork.Id);

        // Assert
        Assert.False(context.Artworks.Any(a => a.Id == artwork.Id));
    }

    [Fact]
    public async Task SearchByTitle_ReturnsArtwork()
    {
        // Arrange
        var service = ServiceProvider!.GetRequiredService<IArtworkService>();
        var context = DbContext!;
        context.Users.Add(CreateHelper.CreateUser(1));
        var artwork = CreateHelper.CreateArtwork("SearchTitle", 1, 1);
        context.Artworks.Add(artwork);
        await context.SaveChangesAsync();

        // Act
        var result = await service.SearchByTitle("SearchTitle");

        // Assert
        Assert.NotNull(result);
        Assert.Contains(result, a => a.Title == "SearchTitle");
    }

    [Fact]
    public async Task SearchByTitle_ThrowsNotFound_WhenNoArtworksMatch()
    {
        // Arrange
        var service = ServiceProvider!.GetRequiredService<IArtworkService>();
        var context = DbContext!;
        context.Users.Add(CreateHelper.CreateUser(1));
        var artwork = CreateHelper.CreateArtwork("AnotherTitle", 1, 1);
        context.Artworks.Add(artwork);
        await context.SaveChangesAsync();

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(async () =>
            await service.SearchByTitle("NonExistentTitle"));
    }

    [Fact]
    public async Task ChangeVisibilityAsync_UpdatesVisibility()
    {
        // Arrange
        var service = ServiceProvider!.GetRequiredService<IArtworkService>();
        var context = DbContext!;
        context.Users.Add(CreateHelper.CreateUser(1));
        var artwork = CreateHelper.CreateArtwork("Visibility", 1, 1, isPrivate: false);
        context.Artworks.Add(artwork);
        await context.SaveChangesAsync();

        // Act
        await service.ChangeVisibilityAsync(artwork.Id, true);
        var updated = await context.Artworks.FindAsync(artwork.Id);

        // Assert
        Assert.NotNull(updated);
        Assert.True(updated.IsPrivate);
    }

    [Fact]
    public async Task PutOnSaleAsync_UpdatesSaleProperties()
    {
        // Arrange
        var service = ServiceProvider!.GetRequiredService<IArtworkService>();
        var context = DbContext!;
        context.Users.Add(CreateHelper.CreateUser(1));
        var artwork = CreateHelper.CreateArtwork("Sale", 1, 1);
        context.Artworks.Add(artwork);
        await context.SaveChangesAsync();
        var dto = new PutArtworkOnSaleDTO { Price = 100, Currency = Currency.USD };

        // Act
        await service.PutOnSaleAsync(artwork.Id, 1, dto);
        var updated = await context.Artworks.FindAsync(artwork.Id);

        // Assert
        Assert.NotNull(updated);
        Assert.True(updated.IsOnSale);
        Assert.Equal(100, updated.Price);
    }

    [Fact]
    public async Task RemoveFromSaleAsync_RemovesSale()
    {
        // Arrange
        var service = ServiceProvider!.GetRequiredService<IArtworkService>();
        var context = DbContext!;
        context.Users.Add(CreateHelper.CreateUser(1));
        var artwork = CreateHelper.CreateArtwork("RemoveSale", 1, 1, isOnSale: true, price: 100);
        context.Artworks.Add(artwork);
        await context.SaveChangesAsync();

        // Act
        await service.RemoveFromSaleAsync(artwork.Id, 1);
        var updated = await context.Artworks.FindAsync(artwork.Id);

        // Assert
        Assert.NotNull(updated);
        Assert.False(updated.IsOnSale);
        Assert.Null(updated.Price);
    }

    [Fact]
    public async Task TransferToUserAsync_TransfersOwnership()
    {
        // Arrange
        var service = ServiceProvider!.GetRequiredService<IArtworkService>();
        var context = DbContext!;
        context.Users.Add(CreateHelper.CreateUser(1));
        context.Users.Add(CreateHelper.CreateUser(2));
        var artwork = CreateHelper.CreateArtwork("Transfer", 1, 1);
        context.Artworks.Add(artwork);
        await context.SaveChangesAsync();

        // Act
        await service.TransferToUserAsync(artwork.Id, 1, 2);
        var updated = await context.Artworks.FindAsync(artwork.Id);

        // Assert
        Assert.NotNull(updated);
        Assert.Equal(2, updated.PostedByUserId);
    }

    [Theory]
    [InlineData(1, 2)] // Viewing another user's artworks
    [InlineData(1, 1)] // Viewing own artworks
    public async Task GetUserArtworksAsync_ReturnsArtworks(int userId, int loggedInUserId)
    {
        // Arrange
        var service = ServiceProvider!.GetRequiredService<IArtworkService>();
        var context = DbContext!;

        context.Users.Add(CreateHelper.CreateUser(userId));
        if (userId != loggedInUserId)
            context.Users.Add(CreateHelper.CreateUser(loggedInUserId));

        var publicArtwork = CreateHelper.CreateArtwork("UserArtworks", userId, userId, isPrivate: false);
        context.Artworks.Add(publicArtwork);
        var privateArtwork = CreateHelper.CreateArtwork("PrivateArtworks", userId, userId, isPrivate: true);
        context.Artworks.Add(privateArtwork);

        await context.SaveChangesAsync();

        // Act
        var result = await service.GetUserArtworksAsync(userId, loggedInUserId);

        // Assert
        Assert.NotNull(result);
        Assert.NotEmpty(result.PublicArtworks);
        if (userId == loggedInUserId)
            Assert.NotEmpty(result.PrivateArtworks);
        else
            Assert.Empty(result.PrivateArtworks);
    }

    [Fact]
    public async Task GetArtworkImageAsync_ReturnsImage()
    {
        // Arrange
        var service = ServiceProvider!.GetRequiredService<IArtworkService>();
        var context = DbContext!;
        context.Users.Add(CreateHelper.CreateUser(1));
        var artwork = CreateHelper.CreateArtwork("Image", 1, 1, image: new byte[] { 5 }, contentType: "image/png");
        context.Artworks.Add(artwork);
        await context.SaveChangesAsync();

        // Act
        var (image, contentType) = await service.GetArtworkImageAsync(artwork.Id);

        // Assert
        Assert.Equal(new byte[] { 5 }, image);
        Assert.Equal("image/png", contentType);
    }
}