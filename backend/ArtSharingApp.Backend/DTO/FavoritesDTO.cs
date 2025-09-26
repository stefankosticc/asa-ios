namespace ArtSharingApp.Backend.DTO;

public class FavoritesDTO
{
    public int UserId { get; set; }
    public int ArtworkId { get; set; }
    public string? ArtworkTitle { get; set; }
    public string? ArtworkImage { get; set; }
}
