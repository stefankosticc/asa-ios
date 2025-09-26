using ArtSharingApp.Backend.DTO;
using ArtSharingApp.Backend.Exceptions;

namespace ArtSharingApp.Backend.Service.ServiceInterface;

/// <summary>
/// Defines methods for managing roles.
/// </summary>
public interface IRoleService
{
    /// <summary>
    /// Adds a new role.
    /// </summary>
    /// <param name="roleDto">The role data.</param>
    /// <exception cref="BadRequestException">Thrown if parameters are invalid.</exception>
    Task AddRoleAsync(RoleRequestDTO roleDto);

    /// <summary>
    /// Retrieves all roles.
    /// </summary>
    /// <returns>A collection of <see cref="RoleResponseDTO"/> representing all roles.</returns>
    Task<IEnumerable<RoleResponseDTO>> GetAllAsync();

    /// <summary>
    /// Retrieves a role by its ID.
    /// </summary>
    /// <param name="id">The role ID.</param>
    /// <returns>The <see cref="RoleResponseDTO"/> for the specified role.</returns>
    /// <exception cref="NotFoundException">Thrown if the role is not found.</exception>
    Task<RoleResponseDTO?> GetByIdAsync(int id);

    /// <summary>
    /// Updates an existing role.
    /// </summary>
    /// <param name="id">The role ID.</param>
    /// <param name="roleDto">The updated role data.</param>
    /// <exception cref="BadRequestException">Thrown if parameters are invalid.</exception>
    /// <exception cref="NotFoundException">Thrown if the role is not found.</exception>
    Task UpdateAsync(int id, RoleRequestDTO roleDto);

    /// <summary>
    /// Deletes a role by its ID.
    /// </summary>
    /// <param name="id">The role ID.</param>
    /// <exception cref="NotFoundException">Thrown if the role is not found.</exception>
    Task DeleteAsync(int id);
}