using ArtSharingApp.Backend.DTO;
using ArtSharingApp.Backend.Models;

namespace ArtSharingApp.Backend.Profile;

public class UserProfile : AutoMapper.Profile
{
    public UserProfile()
    {
        CreateMap<User, UserResponseDTO>()
            .ForMember(dest => dest.RoleName, opt =>
                opt.MapFrom(src => src.Role != null ? src.Role.Name : null))
            .ForMember(dest => dest.ProfilePhoto, opt =>
                opt.MapFrom(src => $"/api/user/{src.Id}/profile-photo"));

        CreateMap<UserRequestDTO, User>()
            .ForMember(dest => dest.Role, opt => opt.Ignore());

        CreateMap<UserRegisterDTO, User>()
            .ForMember(dest => dest.Role, opt => opt.Ignore());

        CreateMap<User, UserSearchResponseDTO>()
            .ForMember(dest => dest.ProfilePhoto, opt =>
                opt.MapFrom(src => $"/api/user/{src.Id}/profile-photo"));

        CreateMap<User, TopArtistResponseDTO>()
            .ForMember(dest => dest.ProfilePhoto, opt =>
                opt.MapFrom(src => $"/api/user/{src.Id}/profile-photo"));

        CreateMap<User, UserByUserNameResponseDTO>()
        .ForMember(dest => dest.ProfilePhoto, opt =>
                opt.MapFrom(src => $"/api/user/{src.Id}/profile-photo")); ;

        CreateMap<UpdateUserProfileRequestDTO, User>();
    }
}