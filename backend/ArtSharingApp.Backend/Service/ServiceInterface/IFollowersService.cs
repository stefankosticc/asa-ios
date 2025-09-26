using ArtSharingApp.Backend.DTO;
using ArtSharingApp.Backend.Exceptions;

namespace ArtSharingApp.Backend.Service.ServiceInterface;

/// <summary>
/// Defines methods for managing user followers and following relationships.
/// </summary>
public interface IFollowersService
{
    /// <summary>
    /// Follows a user.
    /// </summary>
    /// <param name="loggedInUserId">The ID of the user who wants to follow another user.</param>
    /// <param name="userId">The ID of the user to be followed.</param>
    /// <returns>True if the operation succeeds.</returns>
    /// <exception cref="BadRequestException">Thrown if already following or trying to follow self.</exception>
    /// <exception cref="NotFoundException">Thrown if the user to be followed is not found.</exception>
    Task<bool> FollowUserAsync(int loggedInUserId, int userId);

    /// <summary>
    /// Unfollows a user.
    /// </summary>
    /// <param name="loggedInUserId">The ID of the user who wants to unfollow another user.</param>
    /// <param name="userId">The ID of the user to be unfollowed.</param>
    /// <returns>True if the operation succeeds.</returns>
    /// <exception cref="BadRequestException">Thrown if not following or trying to unfollow self.</exception>
    /// <exception cref="NotFoundException">Thrown if the user to be unfollowed is not found.</exception>
    Task<bool> UnfollowUserAsync(int loggedInUserId, int userId);

    /// <summary>
    /// Retrieves the followers of a user.
    /// </summary>
    /// <param name="userId">The ID of the user whose followers to retrieve.</param>
    /// <param name="skip">The number of followers to skip for pagination.</param>
    /// <param name="take">The number of followers to take for pagination.</param>
    /// <returns>A collection of <see cref="FollowersDTO"/> representing the user's followers.</returns>
    /// <exception cref="NotFoundException">Thrown if the user is not found or has no followers.</exception>
    Task<IEnumerable<FollowersDTO>?> GetFollowersAsync(int userId, int skip, int take);

    /// <summary>
    /// Retrieves the users that a user is following.
    /// </summary>
    /// <param name="userId">The ID of the user whose following list to retrieve.</param>
    /// <param name="skip">The number of users to skip for pagination.</param>
    /// <param name="take">The number of users to take for pagination.</param>
    /// <returns>A collection of <see cref="FollowingDTO"/> representing the users being followed.</returns>
    /// <exception cref="NotFoundException">Thrown if the user is not found or not following anyone.</exception>
    Task<IEnumerable<FollowingDTO>?> GetFollowingAsync(int userId, int skip, int take);

    /// <summary>
    /// Retrieves the count of followers for a user.
    /// </summary>
    /// <param name="loggedInUserId">The ID of the user whose followers count to retrieve.</param>
    /// <returns>The number of followers.</returns>
    Task<int> GetFollowersCountAsync(int loggedInUserId);

    /// <summary>
    /// Retrieves the count of users that a user is following.
    /// </summary>
    /// <param name="loggedInUserId">The ID of the user whose following count to retrieve.</param>
    /// <returns>The number of users being followed.</returns>
    Task<int> GetFollowingCountAsync(int loggedInUserId);

    /// <summary>
    /// Retrieves artworks posted by users that the logged-in user is following.
    /// </summary>
    /// <param name="loggedInUserId">The ID of the logged-in user.</param>
    /// <param name="skip">The number of artworks to skip for pagination.</param>
    /// <param name="take">The number of artworks to take for pagination.</param>
    /// <returns>A collection of <see cref="FollowedUserArtworkDTO"/> representing artworks from followed users.</returns>
    Task<IEnumerable<FollowedUserArtworkDTO>?> GetFollowedUsersArtworksAsync(int loggedInUserId, int skip, int take);
}