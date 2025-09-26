using ArtSharingApp.Backend.DataAccess.Repository.RepositoryInterface;
using ArtSharingApp.Backend.DTO;
using ArtSharingApp.Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace ArtSharingApp.Backend.DataAccess.Repository;

public class AuctionRepository : GenericRepository<Auction>, IAuctionRepository
{
    public AuctionRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<bool> IsAuctionScheduledAsync(int artworkId, DateTime requestStartTime, DateTime requestEndTime)
    {
        return await _dbSet.AnyAsync(a => a.ArtworkId == artworkId &&
                                          a.StartTime < requestEndTime &&
                                          a.EndTime > requestStartTime);
    }

    public async Task<bool> HasFutureAuctionScheduledAsync(int artworkId, DateTime fromTime)
    {
        return await _dbSet.AnyAsync(a => a.ArtworkId == artworkId && a.EndTime >= fromTime);
    }

    public async Task<Auction?> GetActiveAuctionByArtworkIdAsync(int artworkId, DateTime now)
    {
        return await _dbSet
            .Where(a => a.ArtworkId == artworkId && a.StartTime <= now && a.EndTime >= now)
            .FirstOrDefaultAsync();
    }

    public void UpdateEndTime(Auction auction)
    {
        _context.Attach(auction);
        _context.Entry(auction).Property(a => a.EndTime).IsModified = true;
    }

    public async Task<IEnumerable<HighStakesAuctionDTO>?> GetHighStakesAuctionsAsync(int count, DateTime now)
    {
        return await _dbSet
            .Where(a => a.StartTime <= now && a.EndTime >= now && !a.Artwork.IsPrivate)
            .Select(a => new
            {
                a.Id,
                ArtworkId = a.Artwork.Id,
                ArtworkTitle = a.Artwork.Title,
                a.StartingPrice,
                MaxOffer = a.Offers.Any() ? a.Offers.Max(o => o.Amount) : 0,
                OfferCount = a.Offers.Count,
                a.Currency
            })
            .Where(x => x.OfferCount >= 5)
            .Select(x => new HighStakesAuctionDTO
            {
                AuctionId = x.Id,
                ArtworkId = x.ArtworkId,
                ArtworkTitle = x.ArtworkTitle,
                CurrentPrice = Math.Max(x.StartingPrice, x.MaxOffer),
                OfferCount = x.OfferCount,
                Currency = x.Currency
            })
            .OrderByDescending(x => x.CurrentPrice)
            .Take(count)
            .ToListAsync();
    }
}