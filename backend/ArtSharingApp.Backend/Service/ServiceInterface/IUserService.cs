using ArtSharingApp.Backend.DTO;
using ArtSharingApp.Backend.Exceptions;

namespace ArtSharingApp.Backend.Service.ServiceInterface
{
    /// <summary>
    /// Defines methods for managing users.
    /// </summary>
    public interface IUserService
    {
        /// <summary>
        /// Adds a new user.
        /// </summary>
        /// <param name="userDto">The user data.</param>
        /// <exception cref="BadRequestException">Thrown if parameters are invalid.</exception>
        Task AddUserAsync(UserRequestDTO userDto);

        /// <summary>
        /// Retrieves a user by their ID.
        /// </summary>
        /// <param name="id">The user ID.</param>
        /// <returns>The <see cref="UserResponseDTO"/> representing the user with the specified ID.</returns>
        /// <exception cref="NotFoundException">Thrown if the user is not found.</exception>
        Task<UserResponseDTO?> GetUserByIdAsync(int id);

        /// <summary>
        /// Retrieves all users.
        /// </summary>
        /// <returns>A collection of <see cref="UserResponseDTO"/> representing all users.</returns>
        Task<IEnumerable<UserResponseDTO>> GetAllUsersAsync();

        /// <summary>
        /// Deletes a user by their ID.
        /// </summary>
        /// <param name="id">The user ID.</param>
        /// <exception cref="NotFoundException">Thrown if the user is not found.</exception>
        Task DeleteAsync(int id);

        /// <summary>
        /// Updates the biography of a user.
        /// </summary>
        /// <param name="userId">The user ID.</param>
        /// <param name="biography">The new biography text.</param>
        /// <exception cref="NotFoundException">Thrown if the user is not found.</exception>
        /// <exception cref="BadRequestException">Thrown if the biography update fails.</exception>
        Task UpdateUserBiographyAsync(int userId, string biography);

        /// <summary>
        /// Searches for users by name or username.
        /// </summary>
        /// <param name="searchString">The search string for name or username.</param>
        /// <returns>A collection of <see cref="UserSearchResponseDTO"/> matching the search criteria.</returns>
        /// <exception cref="BadRequestException">Thrown if the search string is invalid.</exception>
        Task<IEnumerable<UserSearchResponseDTO?>> GetUsersByNameAndUserName(string searchString);

        /// <summary>
        /// Retrieves the profile photo and content type for a user.
        /// </summary>
        /// <param name="id">The user ID.</param>
        /// <returns>A tuple containing the profile photo bytes and content type.</returns>
        /// <exception cref="NotFoundException">Thrown if the profile photo is not found.</exception>
        Task<(byte[] ProfilePhoto, string ContentType)> GetProfilePhotoAsync(int id);

        /// <summary>
        /// Retrieves a user by their username.
        /// </summary>
        /// <param name="username">The username of the user.</param>
        /// <param name="loggedInUserId">The ID of the logged-in user.</param>
        /// <returns>The <see cref="UserByUserNameResponseDTO"/> representing the user with the specified username.</returns>
        /// <exception cref="NotFoundException">Thrown if the user is not found.</exception>
        Task<UserByUserNameResponseDTO?> GetUserByUserNameAsync(string username, int loggedInUserId);

        /// <summary>
        /// Updates an existing user's profile, including profile photo.
        /// </summary>
        /// <param name="userId">The user ID.</param>
        /// <param name="userDto">The updated user profile data.</param>
        /// <param name="profilePhoto">The new profile photo file (optional).</param>
        /// <exception cref="BadRequestException">Thrown if parameters are invalid.</exception>
        /// <exception cref="NotFoundException">Thrown if the user is not found.</exception>
        Task UpdateAsync(int userId, UpdateUserProfileRequestDTO userDto, IFormFile? profilePhoto);
    }
}