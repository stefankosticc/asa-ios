using ArtSharingApp.Backend.DTO;
using ArtSharingApp.Backend.Exceptions;

namespace ArtSharingApp.Backend.Service.ServiceInterface;

/// <summary>
/// Defines methods for managing cities.
/// </summary>
public interface ICityService
{
    /// <summary>
    /// Retrieves all cities.
    /// </summary>
    /// <returns>A collection of <see cref="CityResponseDTO"/> representing all cities.</returns>
    Task<IEnumerable<CityResponseDTO>> GetAllAsync();

    /// <summary>
    /// Retrieves a city by its ID.
    /// </summary>
    /// <param name="id">The city ID.</param>
    /// <returns>The <see cref="CityResponseDTO"/> for the specified city.</returns>
    /// <exception cref="NotFoundException">Thrown if the city is not found.</exception>
    Task<CityResponseDTO?> GetByIdAsync(int id);

    /// <summary>
    /// Adds a new city.
    /// </summary>
    /// <param name="cityDto">The city data.</param>
    /// <exception cref="BadRequestException">Thrown if parameters are invalid.</exception>
    Task AddAsync(CityRequestDTO cityDto);

    /// <summary>
    /// Updates an existing city.
    /// </summary>
    /// <param name="id">The city ID.</param>
    /// <param name="cityDto">The updated city data.</param>
    /// <exception cref="BadRequestException">Thrown if parameters are invalid.</exception>
    /// <exception cref="NotFoundException">Thrown if the city is not found.</exception>
    Task UpdateAsync(int id, CityRequestDTO cityDto);

    /// <summary>
    /// Deletes a city by its ID.
    /// </summary>
    /// <param name="id">The city ID.</param>
    /// <exception cref="NotFoundException">Thrown if the city is not found.</exception>
    Task DeleteAsync(int id);

    /// <summary>
    /// Searches cities by name.
    /// </summary>
    /// <param name="name">The name to search for.</param>
    /// <returns>A collection of <see cref="CityResponseDTO"/> matching the name, or null if none found.</returns>
    /// <exception cref="BadRequestException">Thrown if the name is not provided.</exception>
    Task<IEnumerable<CityResponseDTO>?> GetCitiesByName(string name);

    /// <summary>
    /// Retrieves artworks associated with a city by city ID.
    /// </summary>
    /// <param name="id">The city ID.</param>
    /// <returns>A collection of <see cref="ArtworkResponseDTO"/> for the city.</returns>
    /// <exception cref="NotFoundException">Thrown if the city is not found.</exception>
    Task<IEnumerable<ArtworkResponseDTO>?> GetArtworksByCityId(int id);

    /// <summary>
    /// Retrieves galleries associated with a city by city ID.
    /// </summary>
    /// <param name="id">The city ID.</param>
    /// <returns>A collection of <see cref="GalleryResponseDTO"/> for the city.</returns>
    /// <exception cref="NotFoundException">Thrown if the city is not found.</exception>
    Task<IEnumerable<GalleryResponseDTO>?> GetGalleriesByCityId(int id);
}