using ArtSharingApp.Backend.DTO;
using ArtSharingApp.Backend.Models;

namespace ArtSharingApp.Backend.DataAccess.Repository.RepositoryInterface;

public interface IAuctionRepository : IGenericRepository<Auction>
{
    Task<bool> IsAuctionScheduledAsync(int artworkId, DateTime requestStartTime, DateTime requestEndTime);
    Task<bool> HasFutureAuctionScheduledAsync(int artworkId, DateTime fromTime);
    Task<Auction?> GetActiveAuctionByArtworkIdAsync(int artworkId, DateTime now);
    void UpdateEndTime(Auction auction);
    Task<IEnumerable<HighStakesAuctionDTO>?> GetHighStakesAuctionsAsync(int count, DateTime now);
}