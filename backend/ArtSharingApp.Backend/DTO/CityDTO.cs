namespace ArtSharingApp.Backend.DTO;

public class CityResponseDTO
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string? Country { get; set; }
}

public class CityRequestDTO
{
    public string Name { get; set; }
    public string? Country { get; set; }
}
