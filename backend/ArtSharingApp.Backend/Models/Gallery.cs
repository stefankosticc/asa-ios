using System.ComponentModel.DataAnnotations;

namespace ArtSharingApp.Backend.Models;

/// <summary>
/// Represents a gallery where artworks are displayed
/// </summary>
public class Gallery
{
    /// <summary>
    /// Unique identifier for the gallery
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Name of the gallery
    /// </summary>
    [Required]
    [MaxLength(200)]
    public string Name { get; set; }

    /// <summary>
    /// Address of the gallery
    /// </summary>
    [Required]
    [MaxLength(500)]
    public string Address { get; set; }

    /// <summary>
    /// Identifier for the city where the gallery is located
    /// </summary>
    public int CityId { get; set; }

    /// <summary>
    /// Navigation property for the city where the gallery is located
    /// </summary>
    public City City { get; set; }

    /// <summary>
    /// Collection of artworks displayed in the gallery
    /// </summary>
    public ICollection<Artwork> Artworks { get; set; } = new List<Artwork>();
}