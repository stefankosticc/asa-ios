using ArtSharingApp.Backend.Models;
using ArtSharingApp.Backend.DTO;

namespace ArtSharingApp.Backend.Profile;

public class FavoritesProfile : AutoMapper.Profile
{
    public FavoritesProfile()
    {
        CreateMap<Favorites, FavoritesDTO>()
            .ForMember(dest => dest.ArtworkTitle, opt =>
                opt.MapFrom(src => src.Artwork.Title))
            .ForMember(dest => dest.ArtworkImage, opt =>
                opt.MapFrom(src => $"/api/artwork/{src.ArtworkId}/image"));
    }
}
