using ArtSharingApp.Backend.DTO;
using ArtSharingApp.Backend.Models;

namespace ArtSharingApp.Backend.Profile;

public class FollowersProfile : AutoMapper.Profile
{
    public FollowersProfile()
    {
        CreateMap<Followers, FollowersDTO>()
            .ForMember(dest => dest.UserName, opt =>
                opt.MapFrom(src => src.User.UserName))
            .ForMember(dest => dest.Name, opt =>
                opt.MapFrom(src => src.User.Name))
            .ForMember(dest => dest.Id, opt =>
                opt.MapFrom(src => src.User.Id))
            .ForMember(dest => dest.ProfilePhoto, opt =>
                opt.MapFrom(src => $"/api/user/{src.User.Id}/profile-photo"));

        CreateMap<Followers, FollowingDTO>()
            .ForMember(dest => dest.UserName, opt =>
                opt.MapFrom(src => src.Follower.UserName))
            .ForMember(dest => dest.Name, opt =>
                opt.MapFrom(src => src.Follower.Name))
            .ForMember(dest => dest.Id, opt =>
                opt.MapFrom(src => src.Follower.Id))
            .ForMember(dest => dest.ProfilePhoto, opt =>
                opt.MapFrom(src => $"/api/user/{src.Follower.Id}/profile-photo"));
    }
}