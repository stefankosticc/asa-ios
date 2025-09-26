using ArtSharingApp.Backend.DataAccess.Repository.RepositoryInterface;
using ArtSharingApp.Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace ArtSharingApp.Backend.DataAccess.Repository;

public class FollowersRepository : GenericRepository<Followers>, IFollowersRepository
{
    public FollowersRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<bool> IsFollowing(int loggedInUserId, int userId)
    {
        return await _dbSet.AnyAsync(f => f.UserId == loggedInUserId && f.FollowerId == userId);
    }

    public async Task DeleteAsync(int loggedInUserId, int userId)
    {
        var follower = await _dbSet.FirstOrDefaultAsync(f => f.UserId == loggedInUserId && f.FollowerId == userId);
        if (follower != null)
        {
            _dbSet.Remove(follower);
        }
    }

    public async Task<IEnumerable<Followers>> GetFollowersAsync(int userId, int skip, int take)
    {
        return await _dbSet
            .Where(f => f.FollowerId == userId)
            .Include(f => f.User)
            .Skip(skip)
            .Take(take)
            .ToListAsync();
    }

    public async Task<IEnumerable<Followers>> GetFollowingAsync(int userId, int skip, int take)
    {
        return await _dbSet
            .Where(f => f.UserId == userId)
            .Include(f => f.Follower)
            .Skip(skip)
            .Take(take)
            .ToListAsync();
    }

    public async Task<int> GetFollowersCountAsync(int loggedInUserId)
    {
        return await _dbSet.CountAsync(f => f.FollowerId == loggedInUserId);
    }

    public Task<int> GetFollowingCountAsync(int loggedInUserId)
    {
        return _dbSet.CountAsync(f => f.UserId == loggedInUserId);
    }

    public async Task<IEnumerable<Artwork>?> GetFollowedUsersArtworksAsync(int loggedInUserId, int skip, int take)
    {
        return await _dbSet
            .Where(f => f.UserId == loggedInUserId)
            .SelectMany(f => f.Follower.PostedArtworks.Where(a => !a.IsPrivate))
            .OrderByDescending(a => a.Date)
            .Include(a => a.PostedByUser)
            .Skip(skip)
            .Take(take)
            .ToListAsync();
    }
}