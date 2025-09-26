using ArtSharingApp.Backend.Models.Enums;

namespace ArtSharingApp.Backend.DTO;

public class OfferRequestDTO
{
    public decimal Amount { get; set; }
}

public class OfferResponseDTO
{
    public int Id { get; set; }
    public decimal Amount { get; set; }
    public int UserId { get; set; }
    public string UserName { get; set; }
    public OfferStatus Status { get; set; }
}
