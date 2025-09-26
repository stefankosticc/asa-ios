using ArtSharingApp.Backend.DTO;
using ArtSharingApp.Backend.Models;

namespace ArtSharingApp.Backend.Profile;

public class NotificationProfile : AutoMapper.Profile
{
    public NotificationProfile()
    {
        CreateMap<NotificationRequestDTO, Notification>()
            .ForMember(dest => dest.Recipient, opt => opt.Ignore());

        CreateMap<Notification, NotificationResponseDTO>();
    }
}