using ArtSharingApp.Backend.Models.Enums;

namespace ArtSharingApp.Backend.DTO;

public class AuctionStartDTO
{
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public decimal StartingPrice { get; set; }
    public Currency Currency { get; set; }
}

public class AuctionResponseDTO
{
    public int Id { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public decimal CurrentPrice { get; set; }
    public int OfferCount { get; set; }
    public Currency Currency { get; set; }
}

public class AuctionUpdateEndDTO
{
    public DateTime EndTime { get; set; }
}

public class HighStakesAuctionDTO
{
    public int AuctionId { get; set; }
    public int ArtworkId { get; set; }
    public string ArtworkTitle { get; set; }
    public decimal CurrentPrice { get; set; }
    public int OfferCount { get; set; }
    public Currency Currency { get; set; }
}