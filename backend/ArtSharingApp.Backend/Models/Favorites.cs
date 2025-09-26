namespace ArtSharingApp.Backend.Models;

/// <summary>
/// Represents likes made by users on artworks
/// </summary>
public class Favorites
{
    /// <summary>
    /// Unique identifier for the user who liked the artwork
    /// </summary>
    public int UserId { get; set; }

    /// <summary>
    /// Navigation property to the user who liked the artwork
    /// </summary>
    public User User { get; set; }

    /// <summary>
    /// Unique identifier for the artwork that was liked
    /// </summary>
    public int ArtworkId { get; set; }

    /// <summary>
    /// Navigation property to the artwork that was liked
    /// </summary>
    public Artwork Artwork { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="Favorites"/> class.
    /// This constructor is used to create a new Favorites object with the specified user and artwork IDs
    /// </summary>
    /// <param name="userId">
    /// Unique identifier for the user who liked the artwork
    /// </param>
    /// <param name="artworkId">
    /// Unique identifier for the artwork that was liked
    /// </param>
    public Favorites(int userId, int artworkId)
    {
        UserId = userId;
        ArtworkId = artworkId;
    }
}