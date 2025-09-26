namespace ArtSharingApp.Backend.Models.Enums;

/// <summary>
/// Represents the status of an offer made by a user for an artwork.
/// </summary>
public enum OfferStatus
{
    /// <summary>
    /// The offer has been submitted.
    /// </summary>
    SUBMITTED,

    /// <summary>
    /// The offer has been accepted by the seller.
    /// </summary>
    ACCEPTED,

    /// <summary>
    /// The offer has been rejected by the seller.
    /// </summary>
    REJECTED,

    /// <summary>
    /// The offet has been withdrawn by the user who made the offer.
    /// </summary>
    WITHDRAWN
}