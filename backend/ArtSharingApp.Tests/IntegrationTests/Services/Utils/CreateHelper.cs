using ArtSharingApp.Backend.Models;
using ArtSharingApp.Backend.Models.Enums;

namespace ArtSharingApp.Tests.IntegrationTests.Services.Utils;

/// <summary>
/// Helper class for creating test entities.
/// </summary>
public static class CreateHelper
{
    /// <summary>
    /// Creates a test user with specified parameters.
    /// </summary>
    /// <param name="id"> User ID </param>
    /// <param name="roleId"> Role ID, default is 1 (Admin) </param>
    /// <param name="name"> User's full name, default is "Test User" </param>
    /// <param name="userName"> Username, default is "user{id}" </param>
    /// <param name="profilePhoto"> Profile photo in byte array format, default is null </param>
    /// <param name="contentType"> Content type of the profile photo, default is null </param>
    /// <returns> A new User object </returns>
    public static User CreateUser(
        int id,
        int roleId = 1,
        string name = "Test User",
        string? userName = null,
        byte[]? profilePhoto = null,
        string? contentType = null,
        string? biography = "This is a test user.")
    {
        return new User
        {
            Id = id,
            UserName = userName ?? $"user{id}",
            Email = $"user{id}@gmail.com",
            Name = name,
            RoleId = roleId,
            ProfilePhoto = profilePhoto,
            ContentType = contentType,
            Biography = biography
        };
    }

    /// <summary>
    /// Creates a test artwork with specified parameters.
    /// </summary>
    /// <param name="title"> Title of the artwork </param>
    /// <param name="postedByUserId"> ID of the user who posted the artwork </param>
    /// <param name="createdByArtistId"> ID of the artist who created the artwork </param>
    /// <param name="image"> Image data in byte array format, default is a single byte array </param>
    /// <param name="contentType"> Content type of the image, default is "image/jpeg" </param>
    /// <param name="story"> Story behind the artwork, default is "Test story" </param>
    /// <param name="tipsAndTricks"> Techniques or insights used during creation, default is "Test tips" </param>
    /// <param name="isPrivate"> Indicates if the artwork is private, default is false </param>
    /// <param name="isOnSale"> Indicates if the artwork is for sale, default is false </param>
    /// <param name="price"> Price of the artwork if it's for sale, default is null </param>
    /// <param name="currency"> Currency of the price, default is USD </param>
    /// <param name="color"> Dominant color of the artwork, default is null </param>
    /// <param name="cityId"> ID of the city associated with the artwork, default is null </param>
    /// <param name="galleryId"> ID of the gallery where the artwork is displayed, default is null </param>
    /// <returns></returns>
    public static Artwork CreateArtwork(
        string title,
        int postedByUserId,
        int createdByArtistId,
        byte[]? image = null,
        string? contentType = null,
        string? story = null,
        string? tipsAndTricks = null,
        bool isPrivate = false,
        bool isOnSale = false,
        decimal? price = null,
        Currency currency = Currency.USD,
        string? color = null,
        int? cityId = null,
        int? galleryId = null
    )
    {
        return new Artwork
        {
            Title = title,
            PostedByUserId = postedByUserId,
            CreatedByArtistId = createdByArtistId,
            Image = image ?? new byte[] { 1 },
            ContentType = contentType ?? "image/jpeg",
            Date = DateOnly.FromDateTime(System.DateTime.UtcNow),
            Story = story ?? "Test story",
            TipsAndTricks = tipsAndTricks ?? "Test tips",
            IsPrivate = isPrivate,
            IsOnSale = isOnSale,
            Price = price,
            Currency = currency,
            Color = color,
            CityId = cityId,
            GalleryId = galleryId
        };
    }

    public static Gallery CreateGallery(string name, string address, int cityId)
    {
        return new Gallery
        {
            Name = name,
            Address = address,
            CityId = cityId
        };
    }
}