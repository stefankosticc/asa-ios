using ArtSharingApp.Backend.DataAccess.Repository.RepositoryInterface;
using ArtSharingApp.Backend.Models;
using ArtSharingApp.Backend.Models.Enums;
using Microsoft.EntityFrameworkCore;

namespace ArtSharingApp.Backend.DataAccess.Repository;

public class OfferRepository : GenericRepository<Offer>, IOfferRepository
{
    public OfferRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<decimal> GetMaxOfferAmountAsync(int auctionId)
    {
        var maxOfferAmount = await _dbSet
            .Where(o => o.AuctionId == auctionId)
            .MaxAsync(o => (decimal?)o.Amount);
        return maxOfferAmount ?? 0;
    }

    public async Task<IEnumerable<Offer>> GetOffersByAuctionIdAsync(int auctionId)
    {
        return await _dbSet.Where(o => o.AuctionId == auctionId && o.Status != OfferStatus.REJECTED)
            .Include(o => o.User)
            .OrderBy(o => o.Status == OfferStatus.ACCEPTED ? 0 : o.Status == OfferStatus.SUBMITTED ? 1 : 2)
            .ThenByDescending(o => o.Timestamp)
            .ToListAsync();
    }

    public void UpdateOfferStatus(Offer offer)
    {
        _context.Attach(offer);
        _context.Entry(offer).Property(o => o.Status).IsModified = true;
    }

    public async Task<int> GetOfferCountByAuctionIdAsync(int auctionId)
    {
        return await _dbSet.Where(o => o.AuctionId == auctionId).CountAsync();
    }

    public async Task<bool> AuctionHasAcceptedOffer(int auctionId)
    {
        return await _dbSet.AnyAsync(o => o.AuctionId == auctionId && o.Status == OfferStatus.ACCEPTED);
    }
}