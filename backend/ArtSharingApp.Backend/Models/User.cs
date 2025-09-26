using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace ArtSharingApp.Backend.Models;

/// <summary>
/// Represents a user in the application.
/// Inherits from <see cref="IdentityUser{TKey}"/> with integer key.
/// </summary>
public class User : IdentityUser<int>
{
    /// <summary>
    /// Name of the user.
    /// <remarks>
    /// This property is used to store the full name of the user.
    /// </remarks>
    /// </summary>
    [Required]
    [MaxLength(200)]
    public string Name { get; set; }

    /// <summary>
    /// Biography of the user.
    /// </summary>
    public string? Biography { get; set; }

    /// <summary>
    /// Unique identifier for the role assigned to the user.
    /// </summary>
    public int RoleId { get; set; }

    /// <summary>
    /// Navigation property to the role assigned to the user.
    /// </summary>
    public Role? Role { get; set; }

    /// <summary>
    /// Refresh token used for re-authentication.
    /// <remarks>
    /// This token is used to obtain a new access token without requiring the user to log in again.
    /// </remarks>
    /// </summary>
    public string? RefreshToken { get; set; }

    /// <summary>
    /// Date and time when the refresh token expires.
    /// <remarks>
    /// This property is used to determine when the refresh token is no longer valid.
    /// </remarks>
    /// </summary>
    public DateTime? RefreshTokenExpiresAt { get; set; }

    /// <summary>
    /// User's profile photo in byte array format.
    /// </summary>
    public byte[]? ProfilePhoto { get; set; }

    /// <summary>
    /// Content type of the profile photo.
    /// <remarks>
    /// This is used to determine how the profile photo should be displayed in the UI.
    /// </remarks>
    /// <example>
    /// "image/jpeg"
    /// </example>
    /// </summary>
    public string? ContentType { get; set; }

    /// <summary>
    /// Collection of artworks created by the user.
    /// </summary>
    public ICollection<Artwork> CreatedArtworks { get; set; } = new List<Artwork>();

    /// <summary>
    /// Collection of artworks posted by the user.
    /// </summary>
    public ICollection<Artwork> PostedArtworks { get; set; } = new List<Artwork>();

    /// <summary>
    /// Collection of favorite artworks liked by the user.
    /// </summary>
    public ICollection<Favorites> Favorites { get; set; } = new List<Favorites>();

    /// <summary>
    /// Collection of users that follow this user
    /// </summary>
    public ICollection<Followers> Followers { get; set; } = new List<Followers>();

    /// <summary>
    /// Collection of users that this user is following
    /// </summary>
    public ICollection<Followers> Following { get; set; } = new List<Followers>();

    /// <summary>
    /// Collection of notifications sent to the user.
    /// </summary>
    public ICollection<Notification> Notifications { get; set; } = new List<Notification>();

    /// <summary>
    /// Collection of offers made by the user on auctions.
    /// </summary>
    public ICollection<Offer> Offers { get; set; } = new List<Offer>();

    /// <summary>
    /// Collection of chat messages sent by the user.
    /// </summary>
    public ICollection<ChatMessage> SentMessages { get; set; } = new List<ChatMessage>();

    /// <summary>
    /// Collection of chat messages that the user has received.
    /// </summary>
    public ICollection<ChatMessage> ReceivedMessages { get; set; } = new List<ChatMessage>();

    /// <summary>
    /// Updates the user's biography.
    /// </summary>
    /// <param name="biography">The new biography text.</param>
    /// <exception cref="ArgumentException">
    /// Thrown when the biography is null or empty.
    /// </exception>
    public void UpdateBiography(string biography)
    {
        if (string.IsNullOrWhiteSpace(biography))
            throw new ArgumentException("Biography cannot be null or empty.", nameof(biography));
        Biography = biography;
    }

    /// <summary>
    /// Updates the user's profile photo and content type.
    /// </summary>
    /// <param name="photo">The profile photo as a byte array.</param>
    /// <param name="contentType">The content type of the photo (e.g., "image/jpeg").</param>
    /// <exception cref="ArgumentException">
    /// Thrown when the photo is null or empty.
    /// </exception>
    public void UpdateProfilePhoto(byte[] photo, string contentType)
    {
        if (photo == null || photo.Length == 0)
            throw new ArgumentException("Profile photo cannot be null or empty.", nameof(photo));
        ProfilePhoto = photo;
        ContentType = contentType;
    }

    /// <summary>
    /// Removes the user's profile photo and content type.
    /// </summary>
    public void RemoveProfilePhoto()
    {
        ProfilePhoto = null;
        ContentType = null;
    }

    /// <summary>
    /// Sets the user's refresh token and its expiration.
    /// </summary>
    /// <param name="token">The refresh token string.</param>
    /// <param name="expiresAt">The date and time when the refresh token expires.</param>
    /// <exception cref="ArgumentException">
    /// Thrown when the token is null or empty, or when the expiration date is in the past.
    /// </exception>
    public void SetRefreshToken(string token, DateTime expiresAt)
    {
        if (string.IsNullOrWhiteSpace(token))
            throw new ArgumentException("Refresh token cannot be null or empty.", nameof(token));

        if (expiresAt < DateTime.UtcNow)
            throw new ArgumentException("Refresh token expiration must be in the future.", nameof(expiresAt));

        RefreshToken = token;
        RefreshTokenExpiresAt = expiresAt;
    }

    /// <summary>
    /// Clears the user's refresh token and its expiration.
    /// </summary>
    public void ClearRefreshToken()
    {
        RefreshToken = null;
        RefreshTokenExpiresAt = null;
    }

    /// <summary>
    /// Checks if the refresh token is valid.
    /// A refresh token is considered valid if it is not null or empty and has not expired.
    /// </summary>
    /// <returns>
    /// True if the refresh token is valid; otherwise, false.
    /// </returns>
    public bool IsRefreshTokenValid()
    {
        return !string.IsNullOrWhiteSpace(RefreshToken) && RefreshTokenExpiresAt.HasValue &&
               RefreshTokenExpiresAt > DateTime.UtcNow;
    }
}