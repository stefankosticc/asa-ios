using ArtSharingApp.Backend.DTO;
using ArtSharingApp.Backend.Models;

namespace ArtSharingApp.Backend.Profile;

public class GalleryProfile : AutoMapper.Profile
{
    public GalleryProfile()
    {
        CreateMap<Gallery, GalleryResponseDTO>()
            .ForMember(dest => dest.CityName, opt =>
                opt.MapFrom(src => src.City != null ? src.City.Name : null));

        CreateMap<GalleryRequestDTO, Gallery>()
            .ForMember(dest => dest.City, opt => opt.Ignore());
    }
}
