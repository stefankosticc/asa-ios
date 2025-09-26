using System.ComponentModel.DataAnnotations;
using ArtSharingApp.Backend.Models.Enums;

namespace ArtSharingApp.Backend.Models;

/// <summary>
/// Represents an offer made by a user on an auction
/// </summary>
public class Offer
{
    /// <summary>
    /// Unique identifier for the offer
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Amount made in the offer
    /// <remarks>
    /// This is the monetary value that the user is offering for the auction item.
    /// </remarks>
    /// </summary>
    [Range(0.01, double.MaxValue)]
    public decimal Amount { get; set; }

    /// <summary>
    /// Date and time when the offer was made
    /// </summary>
    public DateTime Timestamp { get; set; }

    /// <summary>
    /// Status of the offer. See <see cref="OfferStatus"/>.
    /// </summary>
    public OfferStatus Status { get; set; }

    /// <summary>
    /// Unique identifier for the auction on which the offer is made
    /// </summary>
    public int AuctionId { get; set; }

    /// <summary>
    /// Navigation property to the auction on which the offer is made
    /// </summary>
    public Auction Auction { get; set; }

    /// <summary>
    /// Unique identifier for the user who made the offer
    /// </summary>
    public int UserId { get; set; }

    /// <summary>
    /// Navigation property to the user who made the offer
    /// </summary>
    public User User { get; set; }

    /// <summary>
    /// Marks the offer as accepted.
    /// </summary>
    public void Accept()
    {
        Status = OfferStatus.ACCEPTED;
    }

    /// <summary>
    /// Marks the offer as rejected.
    /// </summary>
    public void Reject()
    {
        Status = OfferStatus.REJECTED;
    }

    /// <summary>
    /// Marks the offer as withdrawn.
    /// </summary>
    public void Withdraw()
    {
        Status = OfferStatus.WITHDRAWN;
    }

    /// <summary>
    /// Returns true if the offer can be modified (i.e., it is in the SUBMITTED state).
    /// </summary>
    public bool CanBeModified()
    {
        return Status == OfferStatus.SUBMITTED;
    }
}