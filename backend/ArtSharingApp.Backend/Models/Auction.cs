using System.ComponentModel.DataAnnotations;
using ArtSharingApp.Backend.Models.Enums;

namespace ArtSharingApp.Backend.Models;

/// <summary>
/// Represents an auction for an artwork
/// </summary>
public class Auction
{
    /// <summary>
    /// Unique identifier for the auction
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Start time of the auction
    /// </summary>
    public DateTime StartTime { get; set; }

    /// <summary>
    /// End time of the auction
    /// </summary>
    public DateTime EndTime { get; set; }

    /// <summary>
    /// Starting price of the auction
    /// <remarks>
    /// This is the minimum price at which the auction starts.
    /// </remarks>
    /// </summary>
    [Range(0, double.MaxValue)]
    public decimal StartingPrice { get; set; }

    /// <summary>
    /// Currency used for the auction. See <see cref="Currency"/>.
    /// </summary>
    public Currency Currency { get; set; }

    /// <summary>
    /// Unique identifier for the artwork on which the auction is held
    /// </summary>
    public int ArtworkId { get; set; }

    /// <summary>
    /// Navigation property to the artwork associated with the auction
    /// </summary>
    public Artwork Artwork { get; set; }

    /// <summary>
    /// Collection of offers made on the auction
    /// </summary>
    public ICollection<Offer> Offers { get; set; } = new List<Offer>();

    /// <summary>
    /// Checks if the auction is currently active based on the current time.
    /// <returns>
    /// True if the current time is between StartTime and EndTime, otherwise false.
    /// </returns>
    /// </summary>
    public bool IsActive()
    {
        return StartTime <= DateTime.UtcNow && EndTime >= DateTime.UtcNow;
    }

    /// <summary>
    /// Updates the end time of the auction.
    /// </summary>
    /// <param name="newEndTime">The new end time for the auction.</param>
    /// <exception cref="ArgumentException">Thrown if newEndTime is before or equal to StartTime.</exception>
    public void UpdateEndTime(DateTime newEndTime)
    {
        if (newEndTime <= StartTime)
            throw new ArgumentException("Auction end time must be after the start time.");
        EndTime = newEndTime;
    }
}