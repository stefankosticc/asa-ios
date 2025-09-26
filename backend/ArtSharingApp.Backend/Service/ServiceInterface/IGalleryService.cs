using ArtSharingApp.Backend.DTO;
using ArtSharingApp.Backend.Exceptions;

namespace ArtSharingApp.Backend.Service.ServiceInterface;

/// <summary>
/// Defines methods for managing galleries.
/// </summary>
public interface IGalleryService
{
    /// <summary>
    /// Retrieves all galleries.
    /// </summary>
    /// <returns>A collection of <see cref="GalleryResponseDTO"/> representing all galleries.</returns>
    Task<IEnumerable<GalleryResponseDTO>> GetAllAsync();

    /// <summary>
    /// Retrieves a gallery by its ID.
    /// </summary>
    /// <param name="id">The gallery ID.</param>
    /// <returns>The <see cref="GalleryResponseDTO"/> for the specified gallery.</returns>
    /// <exception cref="NotFoundException">Thrown if the gallery is not found.</exception>
    Task<GalleryResponseDTO?> GetByIdAsync(int id);

    /// <summary>
    /// Adds a new gallery.
    /// </summary>
    /// <param name="galleryDto">The gallery data.</param>
    /// <exception cref="BadRequestException">Thrown if parameters are invalid.</exception>
    /// <exception cref="NotFoundException">Thrown if the city is not found.</exception>
    Task AddAsync(GalleryRequestDTO galleryDto);

    /// <summary>
    /// Updates an existing gallery.
    /// </summary>
    /// <param name="id">The gallery ID.</param>
    /// <param name="galleryDto">The updated gallery data.</param>
    /// <exception cref="BadRequestException">Thrown if parameters are invalid.</exception>
    /// <exception cref="NotFoundException">Thrown if the gallery or city is not found.</exception>
    Task UpdateAsync(int id, GalleryRequestDTO galleryDto);

    /// <summary>
    /// Deletes a gallery by its ID.
    /// </summary>
    /// <param name="id">The gallery ID.</param>
    /// <exception cref="NotFoundException">Thrown if the gallery is not found.</exception>
    Task DeleteAsync(int id);

    /// <summary>
    /// Retrieves artworks associated with a gallery by gallery ID.
    /// </summary>
    /// <param name="id">The gallery ID.</param>
    /// <returns>A collection of <see cref="ArtworkResponseDTO"/> representing artworks in the gallery.</returns>
    /// <exception cref="NotFoundException">Thrown if the gallery is not found.</exception>
    Task<IEnumerable<ArtworkResponseDTO>?> GetArtworksByGalleryId(int id);

    /// <summary>
    /// Searches galleries by name.
    /// </summary>
    /// <param name="name">The name to search for.</param>
    /// <returns>A collection of <see cref="GalleryResponseDTO"/> matching the name, or null if none found.</returns>
    /// <exception cref="BadRequestException">Thrown if the name is not provided.</exception>
    Task<IEnumerable<GalleryResponseDTO>?> GetGalleriesByName(string name);
}