using ArtSharingApp.Backend.Models.Enums;

namespace ArtSharingApp.Backend.DTO;

public class ArtworkResponseDTO
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Story { get; set; }
    public string Image { get; set; }
    public DateOnly Date { get; set; }
    public string TipsAndTricks { get; set; }
    public bool IsPrivate { get; set; }
    public bool IsOnSale { get; set; }
    public decimal? Price { get; set; }
    public Currency Currency { get; set; }
    public int CreatedByArtistId { get; set; }
    public string CreatedByArtistUserName { get; set; }
    public int PostedByUserId { get; set; }
    public string PostedByUserName { get; set; }
    public int? CityId { get; set; }
    public string? CityName { get; set; }
    public int? GalleryId { get; set; }
    public string? GalleryName { get; set; }
    public bool? IsLikedByLoggedInUser { get; set; }
    public string? Color { get; set; }
}

public class ArtworkRequestDTO
{
    public string Title { get; set; }
    public string Story { get; set; }
    public DateOnly Date { get; set; }
    public string TipsAndTricks { get; set; }
    public bool IsPrivate { get; set; }
    public int CreatedByArtistId { get; set; }
    public int PostedByUserId { get; set; }
    public int? CityId { get; set; }
    public int? GalleryId { get; set; }
    public string? Color { get; set; }
}

public class ChangeArtworkVisibilityDTO
{
    public bool IsPrivate { get; set; }
}

public class PutArtworkOnSaleDTO
{
    public bool IsOnSale { get; set; }
    public decimal Price { get; set; }
    public Currency Currency { get; set; }
}

public class ArtworkPreviewDTO
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Image { get; set; }
    public DateOnly Date { get; set; }
    public bool IsPrivate { get; set; }
    public int PostedByUserId { get; set; }
    public string PostedByUserName { get; set; }
}

public class ArtworkSearchResponseDTO
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Image { get; set; }
    public bool IsOnSale { get; set; }
    public int PostedByUserId { get; set; }
    public string PostedByUserName { get; set; }
    public int? CityId { get; set; }
    public string? CityName { get; set; }
    public string? Country { get; set; }
    public int? GalleryId { get; set; }
    public string? GalleryName { get; set; }
}

public class FollowedUserArtworkDTO
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Image { get; set; }
    public string PostedByUserName { get; set; }
    public string? Color { get; set; }
}

public class DiscoverArtworkDTO
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Image { get; set; }
    public string PostedByUserName { get; set; }
}

public class UserArtworksDTO
{
    public IEnumerable<ArtworkPreviewDTO> PrivateArtworks { get; set; } = Array.Empty<ArtworkPreviewDTO>();
    public IEnumerable<ArtworkPreviewDTO> PublicArtworks { get; set; } = Array.Empty<ArtworkPreviewDTO>();
}