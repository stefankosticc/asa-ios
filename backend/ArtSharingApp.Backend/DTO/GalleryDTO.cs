namespace ArtSharingApp.Backend.DTO;

public class GalleryResponseDTO
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string? Address { get; set; }
    public int CityId { get; set; }
    public string? CityName { get; set; }
}

public class GalleryRequestDTO
{
    public string Name { get; set; }
    public string? Address { get; set; }
    public int CityId { get; set; }
}
