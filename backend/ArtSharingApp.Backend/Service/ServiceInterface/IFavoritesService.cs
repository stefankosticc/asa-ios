using ArtSharingApp.Backend.DTO;
using ArtSharingApp.Backend.Exceptions;

namespace ArtSharingApp.Backend.Service.ServiceInterface;

/// <summary>
/// Defines methods for managing artwork favorites (likes).
/// </summary>
public interface IFavoritesService
{
    /// <summary>
    /// Likes an artwork for a user.
    /// </summary>
    /// <param name="userId">The ID of the user liking the artwork.</param>
    /// <param name="artworkId">The ID of the artwork to like.</param>
    /// <returns>True if the operation succeeds.</returns>
    /// <exception cref="BadRequestException">Thrown if the artwork is already liked by the user.</exception>
    /// <exception cref="NotFoundException">Thrown if the user or artwork is not found.</exception>
    Task<bool> LikeArtwork(int userId, int artworkId);

    /// <summary>
    /// Removes a like from an artwork for a user.
    /// </summary>
    /// <param name="userId">The ID of the user disliking the artwork.</param>
    /// <param name="artworkId">The ID of the artwork to dislike.</param>
    /// <returns>True if the operation succeeds.</returns>
    /// <exception cref="BadRequestException">Thrown if the artwork is not liked by the user.</exception>
    /// <exception cref="NotFoundException">Thrown if the user or artwork is not found.</exception>
    Task<bool> DislikeArtwork(int userId, int artworkId);

    /// <summary>
    /// Retrieves all artworks liked by a user.
    /// </summary>
    /// <param name="userId">The ID of the user.</param>
    /// <returns>A collection of <see cref="FavoritesDTO"/> representing liked artworks.</returns>
    /// <exception cref="NotFoundException">Thrown if the user is not found or has no liked artworks.</exception>
    Task<IEnumerable<FavoritesDTO>?> GetLikedArtworks(int userId);

    /// <summary>
    /// Retrieves the top 10 artists by number of likes.
    /// </summary>
    /// <remarks>
    /// Top artists are determined based on the total number of likes their artworks have received.
    /// </remarks>
    /// <returns>A collection of <see cref="TopArtistResponseDTO"/> representing the top artists.</returns>
    Task<IEnumerable<TopArtistResponseDTO>?> GetTop10ArtistsByLikesAsync();

    /// <summary>
    /// Retrieves trending artworks based on likes in the last 30 days.
    /// </summary>
    /// <remarks>
    /// Trending artworks are determined based on the number of likes received in the last 30 days.
    /// </remarks>
    /// <param name="count">The number of trending artworks to retrieve.</param>
    /// <returns>A collection of <see cref="DiscoverArtworkDTO"/> representing trending artworks.</returns>
    Task<IEnumerable<DiscoverArtworkDTO>?> GetTrendingArtworksAsync(int count);
}