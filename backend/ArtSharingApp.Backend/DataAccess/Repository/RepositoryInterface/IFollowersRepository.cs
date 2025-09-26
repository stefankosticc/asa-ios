using ArtSharingApp.Backend.DTO;
using ArtSharingApp.Backend.Models;

namespace ArtSharingApp.Backend.DataAccess.Repository.RepositoryInterface;

public interface IFollowersRepository : IGenericRepository<Followers>
{
    Task<bool> IsFollowing(int loggedInUserId, int userId);
    Task DeleteAsync(int loggedInUserId, int userId);
    Task<IEnumerable<Followers>> GetFollowersAsync(int userId, int skip, int take);
    Task<IEnumerable<Followers>> GetFollowingAsync(int userId, int skip, int take);
    Task<int> GetFollowersCountAsync(int loggedInUserId);
    Task<int> GetFollowingCountAsync(int loggedInUserId);
    Task<IEnumerable<Artwork>?> GetFollowedUsersArtworksAsync(int loggedInUserId, int skip, int take);
}