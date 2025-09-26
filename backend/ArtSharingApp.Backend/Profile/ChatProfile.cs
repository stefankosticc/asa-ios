using ArtSharingApp.Backend.DTO;
using ArtSharingApp.Backend.Models;

namespace ArtSharingApp.Backend.Profile;

public class ChatProfile : AutoMapper.Profile
{
    public ChatProfile()
    {
        CreateMap<ChatMessage, ChatMessageResponseDTO>();

        CreateMap<User, UserConversationDTO>()
            .ForMember(dest => dest.UserId,
                opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.ProfilePhoto,
                opt => opt.MapFrom(src => $"/api/user/{src.Id}/profile-photo"))
            .ForMember(dest => dest.UnreadMessageCount,
                opt => opt.MapFrom((src, dest, destMember, context) =>
                {
                    var currentUserId = (int)context.Items["CurrentUserId"];
                    return src.SentMessages.Count(m =>
                        !m.IsRead && m.SenderId == src.Id && m.ReceiverId == currentUserId);
                }))
            .ForMember(dest => dest.LastMessage,
                opt => opt.MapFrom((src, dest, destMember, context) =>
                {
                    var currentUserId = (int)context.Items["CurrentUserId"];
                    var messages = src.ReceivedMessages
                        .Concat(src.SentMessages)
                        .Where(m => m.ReceiverId == currentUserId || m.SenderId == currentUserId)
                        .OrderByDescending(m => m.SentAt)
                        .ToList();
                    return messages.FirstOrDefault().Message;
                }))
            .ForMember(dest => dest.LastMessageDateTime,
                opt => opt.MapFrom((src, dest, destMember, context) =>
                {
                    var currentUserId = (int)context.Items["CurrentUserId"];
                    var messages = src.ReceivedMessages
                        .Concat(src.SentMessages)
                        .Where(m => m.ReceiverId == currentUserId || m.SenderId == currentUserId)
                        .OrderByDescending(m => m.SentAt)
                        .ToList();
                    return messages.FirstOrDefault()?.SentAt;
                }));
    }
}