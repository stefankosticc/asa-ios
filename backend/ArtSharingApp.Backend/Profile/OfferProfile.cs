using ArtSharingApp.Backend.DTO;
using ArtSharingApp.Backend.Models;

namespace ArtSharingApp.Backend.Profile;

public class OfferProfile : AutoMapper.Profile
{
    public OfferProfile()
    {
        CreateMap<OfferRequestDTO, Offer>()
            .ForMember(dest => dest.Auction, opt => opt.Ignore())
            .ForMember(dest => dest.User, opt => opt.Ignore());

        CreateMap<Offer, OfferResponseDTO>()
            .ForMember(dest => dest.UserName, opt
                => opt.MapFrom(src => src.User.UserName));
    }
}