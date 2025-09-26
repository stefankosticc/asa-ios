using ArtSharingApp.Backend.DTO;
using ArtSharingApp.Backend.Exceptions;
using UnauthorizedAccessException = ArtSharingApp.Backend.Exceptions.UnauthorizedAccessException;

namespace ArtSharingApp.Backend.Service.ServiceInterface;

/// <summary>
/// Defines auction-related operations.
/// Provides methods for business logic related to auctions.
/// </summary>
public interface IAuctionService
{
    /// <summary>
    /// Starts a new auction for the specified artwork.
    /// </summary>
    /// <param name="artworkId">The artwork ID.</param>
    /// <param name="userId">The ID of the user starting the auction (must be the one who posted the artwork).</param>
    /// <param name="request">Auction start data.</param>
    /// <exception cref="NotFoundException">Thrown if the artwork is not found.</exception>
    /// <exception cref="UnauthorizedAccessException">Thrown if the user is not authorized.</exception>
    /// <exception cref="BadRequestException">Thrown if auction parameters are invalid or an auction is already scheduled.</exception>
    Task StartAuctionAsync(int artworkId, int userId, AuctionStartDTO request);

    /// <summary>
    /// Makes an offer on an active auction.
    /// </summary>
    /// <param name="auctionId">The auction ID.</param>
    /// <param name="userId">The ID of the user making the offer.</param>
    /// <param name="request">Offer data.</param>
    /// <exception cref="NotFoundException">Thrown if the auction is not found.</exception>
    /// <exception cref="BadRequestException">Thrown if the auction is not active or offer amount is invalid.</exception>
    /// <exception cref="UnauthorizedAccessException">Thrown if the user is making an offer on their own auction.</exception>
    Task MakeAnOfferAsync(int auctionId, int userId, OfferRequestDTO request);

    /// <summary>
    /// Retrieves all offers for a specific auction.
    /// </summary>
    /// <param name="auctionId">The auction ID.</param>
    /// <param name="userId">The ID of the user requesting offers (must be the one who posted the artwork).</param>
    /// <returns>A collection of <see cref="OfferResponseDTO"/> for the auction.</returns>
    /// <exception cref="NotFoundException">Thrown if the auction is not found.</exception>
    /// <exception cref="UnauthorizedAccessException">Thrown if the user is not authorized to view offers for this auction.</exception>
    Task<IEnumerable<OfferResponseDTO>?> GetOffersAsync(int auctionId, int userId);

    /// <summary>
    /// Gets the maximum offer amount for a specific auction.
    /// </summary>
    /// <param name="auctionId">The auction ID.</param>
    /// <returns>The maximum offer amount, or null if no offers exist.</returns>
    /// <exception cref="NotFoundException">Thrown if the auction is not found.</exception>
    Task<decimal?> GetMaxOfferAsync(int auctionId);

    /// <summary>
    /// Accepts an offer for an auction.
    /// </summary>
    /// <param name="offerId">The offer ID.</param>
    /// <param name="userId">The ID of the user accepting the offer (must be the one who posted the artwork).</param>
    /// <exception cref="NotFoundException">Thrown if the offer is not found.</exception>
    /// <exception cref="UnauthorizedAccessException">Thrown if the user is not authorized to accept this offer.</exception>
    /// <exception cref="BadRequestException">Thrown if the offer cannot be accepted.</exception>
    Task AcceptOfferAsync(int offerId, int userId);

    /// <summary>
    /// Rejects an offer for an auction.
    /// </summary>
    /// <param name="offerId">The offer ID.</param>
    /// <param name="userId">The ID of the user rejecting the offer (must be the one who posted the artwork and not the one who made the offer).</param>
    /// <exception cref="NotFoundException">Thrown if the offer is not found.</exception>
    /// <exception cref="UnauthorizedAccessException">Thrown if the user is not authorized to reject this offer.</exception>
    Task RejectOfferAsync(int offerId, int userId);

    /// <summary>
    /// Withdraws an offer made by the user.
    /// </summary>
    /// <param name="offerId">The offer ID.</param>
    /// <param name="userId">The ID of the user withdrawing the offer (must be the one who made the offer and not the one who posted the artwork).</param>
    /// <exception cref="NotFoundException">Thrown if the offer is not found.</exception>
    /// <exception cref="UnauthorizedAccessException">Thrown if the user is not authorized to withdraw this offer.</exception>
    Task WithdrawOfferAsync(int offerId, int userId);

    /// <summary>
    /// Retrieves the active auction for a specific artwork.
    /// </summary>
    /// <param name="artworkId">The artwork ID.</param>
    /// <returns>An <see cref="AuctionResponseDTO"/> for the active auction, or null if none exists.</returns>
    Task<AuctionResponseDTO?> GetActiveAuctionAsync(int artworkId);

    /// <summary>
    /// Updates the end time of an auction.
    /// </summary>
    /// <param name="auctionId">The auction ID.</param>
    /// <param name="userId">The ID of the user updating the auction (must be the one who posted the artwork).</param>
    /// <param name="request">New end time details.</param>
    /// <exception cref="NotFoundException">Thrown if the auction is not found.</exception>
    /// <exception cref="UnauthorizedAccessException">Thrown if the user is not authorized to update this auction.</exception>
    /// <exception cref="BadRequestException">Thrown if the new end time is invalid.</exception>
    Task UpdateAuctionEndTimeAsync(int auctionId, int userId, AuctionUpdateEndDTO request);

    /// <summary>
    /// Retrieves high-stakes auctions (with high and many offers).
    /// </summary>
    /// <param name="count">The number of auctions to retrieve.</param>
    /// <returns>A collection of <see cref="HighStakesAuctionDTO"/>.</returns>
    Task<IEnumerable<HighStakesAuctionDTO>?> GetHighStakesAuctionsAsync(int count);
}