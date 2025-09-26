using System.ComponentModel.DataAnnotations;

namespace ArtSharingApp.Backend.Models;

/// <summary>
/// Represents a city where galleries and artworks are located
/// </summary>
public class City
{
    /// <summary>
    /// Unique identifier for the city
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Name of the city
    /// </summary>
    [Required]
    public string Name { get; set; }

    /// <summary>
    /// Country where the city is located
    /// </summary>
    [Required]
    public string Country { get; set; }

    /// <summary>
    /// Collection of galleries located in the city
    /// </summary>
    public ICollection<Gallery> Galleries { get; set; } = new List<Gallery>();

    /// <summary>
    /// Collection of artworks located in the city
    /// </summary>
    public ICollection<Artwork> Artworks { get; set; } = new List<Artwork>();
}