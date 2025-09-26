using ArtSharingApp.Backend.DTO;
using ArtSharingApp.Backend.Models;

namespace ArtSharingApp.Backend.Profile;

public class AuctionProfile : AutoMapper.Profile
{
    public AuctionProfile()
    {
        CreateMap<AuctionStartDTO, Auction>()
            .ForMember(dest => dest.Artwork, opt => opt.Ignore());
    }
}