using ArtSharingApp.Backend.DataAccess.Repository.RepositoryInterface;
using ArtSharingApp.Backend.DTO;
using ArtSharingApp.Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace ArtSharingApp.Backend.DataAccess.Repository;

public class FavoritesRepository : GenericRepository<Favorites>, IFavoritesRepository
{
    public FavoritesRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task DeleteAsync(int userId, int artworkId)
    {
        var favorite = await _dbSet.FirstOrDefaultAsync(f => f.UserId == userId && f.ArtworkId == artworkId);
        if (favorite != null)
        {
            _dbSet.Remove(favorite);
        }
    }

    public async Task<IEnumerable<Favorites>> GetLikedArtworks(int userId)
    {
        return await _dbSet
            .Where(f => f.UserId == userId && !f.Artwork.IsPrivate)
            .Include(f => f.Artwork)
            .ToListAsync();
    }

    public async Task<IEnumerable<User>> GetTopArtistsByLikesAsync(int count)
    {
        var topArtistLikes = await _dbSet
            .Where(f => !f.Artwork.IsPrivate)
            .GroupBy(f => f.Artwork.CreatedByArtistId)
            .Select(g => new { ArtistId = g.Key, TotalLikes = g.Count() })
            .OrderByDescending(x => x.TotalLikes)
            .Take(count)
            .ToListAsync();

        var topArtistIds = topArtistLikes.Select(x => x.ArtistId).ToList();

        var artists = await _context.Users.Where(u => topArtistIds.Contains(u.Id)).ToListAsync();

        // Preserve ranking order
        var artistDict = artists.ToDictionary(a => a.Id);
        return topArtistIds.Select(id => artistDict[id]).ToList();
    }

    public async Task<IEnumerable<Artwork>?> GetTrendingArtworksAsync(DateTime fromDate, int count)
    {
        var fromDateOnly = DateOnly.FromDateTime(fromDate);

        // Get top artwork IDs based on favorite count
        var topArtworkIds = await _dbSet
            .Where(f => !f.Artwork.IsPrivate && f.Artwork.Date >= fromDateOnly)
            .GroupBy(f => f.ArtworkId)
            .OrderByDescending(g => g.Count())
            .Take(count)
            .Select(g => g.Key)
            .ToListAsync();

        // Fetch artworks and include navigation properties
        var artworks = await _context.Artworks
            .Where(a => topArtworkIds.Contains(a.Id))
            .Include(a => a.PostedByUser)
            .ToListAsync();

        // Preserve the ordering
        var artworkDict = artworks.ToDictionary(a => a.Id);
        return topArtworkIds
            .Where(id => artworkDict.ContainsKey(id))
            .Select(id => artworkDict[id])
            .ToList();
    }
}