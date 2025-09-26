using ArtSharingApp.Backend.DTO;
using ArtSharingApp.Backend.Exceptions;
using UnauthorizedAccessException = ArtSharingApp.Backend.Exceptions.UnauthorizedAccessException;

namespace ArtSharingApp.Backend.Service.ServiceInterface;

/// <summary>
/// Service interface for managing artworks.
/// Provides business logic for artwork-related operations.
/// </summary>
public interface IArtworkService
{
    /// <summary>
    /// Retrieves all artworks.
    /// </summary>
    /// <returns>A collection of <see cref="ArtworkResponseDTO"/> representing all artworks.</returns>
    Task<IEnumerable<ArtworkResponseDTO>> GetAllAsync();

    /// <summary>
    /// Retrieves an artwork by its ID.
    /// </summary>
    /// <param name="id">The artwork ID.</param>
    /// <param name="loggedInUserId">The ID of the logged-in user.</param>
    /// <returns>The <see cref="ArtworkResponseDTO"/> for the specified artwork.</returns>
    /// <exception cref="NotFoundException">Thrown if the artwork is not found.</exception>
    Task<ArtworkResponseDTO?> GetByIdAsync(int id, int loggedInUserId);

    /// <summary>
    /// Adds a new artwork.
    /// </summary>
    /// <param name="artworkDto">The artwork data.</param>
    /// <param name="artworkImage">The image file for the artwork.</param>
    /// <exception cref="BadRequestException">Thrown if parameters are invalid.</exception>
    Task AddAsync(ArtworkRequestDTO artworkDto, IFormFile artworkImage);

    /// <summary>
    /// Updates an existing artwork.
    /// </summary>
    /// <param name="id">The artwork ID.</param>
    /// <param name="artworkDto">The updated artwork data.</param>
    /// <param name="artworkImage">The new image file for the artwork (optional).</param>
    /// <exception cref="BadRequestException">Thrown if parameters are invalid.</exception>
    /// <exception cref="NotFoundException">Thrown if the artwork is not found.</exception>
    Task UpdateAsync(int id, ArtworkRequestDTO artworkDto, IFormFile? artworkImage);

    /// <summary>
    /// Deletes an artwork by its ID.
    /// </summary>
    /// <param name="id">The artwork ID.</param>
    /// <exception cref="NotFoundException">Thrown if the artwork is not found.</exception>
    Task DeleteAsync(int id);

    /// <summary>
    /// Searches artworks by title.
    /// </summary>
    /// <param name="title">The title to search for.</param>
    /// <returns>A collection of <see cref="ArtworkSearchResponseDTO"/> matching the title.</returns>
    /// <exception cref="BadRequestException">Thrown if the title is invalid.</exception>
    /// <exception cref="NotFoundException">Thrown if no artworks are found.</exception>
    Task<IEnumerable<ArtworkSearchResponseDTO>?> SearchByTitle(string title);

    /// <summary>
    /// Changes the visibility of an artwork.
    /// </summary>
    /// <param name="id">The artwork ID.</param>
    /// <param name="isPrivate">True to set the artwork as private, false for public.</param>
    /// <exception cref="NotFoundException">Thrown if the artwork is not found.</exception>
    Task ChangeVisibilityAsync(int id, bool isPrivate);

    /// <summary>
    /// Puts an artwork on sale with fixed price.
    /// </summary>
    /// <param name="id">The artwork ID.</param>
    /// <param name="loggedInUserId">The ID of the logged-in user.</param>
    /// <param name="request">The sale details.</param>
    /// <exception cref="NotFoundException">Thrown if the artwork is not found.</exception>
    /// <exception cref="UnauthorizedAccessException">Thrown if the user is not authorized to put the artwork on sale.</exception>
    /// <exception cref="BadRequestException">Thrown if sale operation fails.</exception>
    Task PutOnSaleAsync(int id, int loggedInUserId, PutArtworkOnSaleDTO request);

    /// <summary>
    /// Removes an artwork from sale.
    /// </summary>
    /// <param name="id">The artwork ID.</param>
    /// <param name="loggedInUserId">The ID of the logged-in user.</param>
    /// <exception cref="NotFoundException">Thrown if the artwork is not found.</exception>
    /// <exception cref="UnauthorizedAccessException">Thrown if the user is not authorized to remove the artwork from sale.</exception>
    Task RemoveFromSaleAsync(int id, int loggedInUserId);

    /// <summary>
    /// Transfers ownership of an artwork to another user.
    /// </summary>
    /// <param name="artworkId">The artwork ID.</param>
    /// <param name="fromUserId">The current owner's user ID.</param>
    /// <param name="toUserId">The new owner's user ID.</param>
    /// <exception cref="NotFoundException">Thrown if artwork or user is not found.</exception>
    /// <exception cref="UnauthorizedAccessException">Thrown if the user is not authorized to transfer the artwork.</exception>
    /// <exception cref="BadRequestException">Thrown if transfer is not needed.</exception>
    Task TransferToUserAsync(int artworkId, int fromUserId, int toUserId);

    /// <summary>
    /// Gets all artworks for a user, including private artworks if the user is the logged-in user.
    /// </summary>
    /// <param name="userId">The user ID whose artworks to retrieve.</param>
    /// <param name="loggedInUserId">The ID of the logged-in user.</param>
    /// <returns>A <see cref="UserArtworksDTO"/> containing public and private artworks.</returns>
    /// <exception cref="NotFoundException">Thrown if the user is not found.</exception>
    Task<UserArtworksDTO> GetUserArtworksAsync(int userId, int loggedInUserId);

    /// <summary>
    /// Retrieves the image and content type for an artwork.
    /// </summary>
    /// <param name="id">The artwork ID.</param>
    /// <returns>A tuple containing the image bytes and content type.</returns>
    /// <exception cref="NotFoundException">Thrown if the image is not found.</exception>
    Task<(byte[] Image, string ContentType)> GetArtworkImageAsync(int id);

    /// <summary>
    /// Extracts the dominant color from an image file.
    /// </summary>
    /// <param name="image">The image file.</param>
    /// <returns>The extracted color as a hex string, or null if extraction fails.</returns>
    /// <exception cref="BadRequestException">Thrown if the image is invalid.</exception>
    Task<string?> ExtractColorAsync(IFormFile image);

    /// <summary>
    /// Retrieves artworks for discovery, excluding those owned or liked by the logged-in user.
    /// </summary>
    /// <param name="loggedInUserId">The ID of the logged-in user.</param>
    /// <param name="skip">The number of artworks to skip.</param>
    /// <param name="take">The number of artworks to take.</param>
    /// <returns>A collection of <see cref="DiscoverArtworkDTO"/> for discovery.</returns>
    Task<IEnumerable<DiscoverArtworkDTO>?> GetDiscoverArtworksAsync(int loggedInUserId, int skip, int take);
}