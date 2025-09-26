using ArtSharingApp.Backend.Models;
using ArtSharingApp.Backend.Service.ServiceInterface;
using ArtSharingApp.Backend.DataAccess.Repository.RepositoryInterface;
using ArtSharingApp.Backend.Exceptions;
using ArtSharingApp.Backend.DTO;
using AutoMapper;

namespace ArtSharingApp.Backend.Service;

/// <summary>
/// Provides business logic for managing chat messages and conversations between users.
/// </summary>
public class ChatService : IChatService
{
    private readonly IChatRepository _chatRepository;
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    /// <summary>
    /// Initializes a new instance of the <see cref="ChatService"/> class.
    /// </summary>
    /// <param name="chatRepository">Repository for chat message data access.</param>
    /// <param name="userRepository">Repository for user data access.</param>
    /// <param name="mapper">AutoMapper instance for DTO mapping.</param>
    public ChatService(IChatRepository chatRepository, IUserRepository userRepository, IMapper mapper)
    {
        _chatRepository = chatRepository;
        _userRepository = userRepository;
        _mapper = mapper;
    }

    /// <inheritdoc />
    public async Task<ChatMessageResponseDTO> SendMessageAsync(int senderId, int receiverId, string message)
    {
        if (receiverId == senderId)
            throw new BadRequestException("Cannot send messages to yourself.");
        if (string.IsNullOrWhiteSpace(message))
            throw new BadRequestException("Message cannot be empty.");

        var sender = await _userRepository.GetByIdAsync(senderId);
        if (sender == null)
            throw new NotFoundException("Sender not found.");

        var receiver = await _userRepository.GetByIdAsync(receiverId);
        if (receiver == null)
            throw new NotFoundException("Receiver not found.");

        var chatMessage = ChatMessage.Create(senderId, receiverId, message);

        await _chatRepository.AddAsync(chatMessage);
        await _chatRepository.SaveAsync();
        return _mapper.Map<ChatMessageResponseDTO>(chatMessage);
    }

    /// <inheritdoc />
    public async Task MarkAsReadAsync(int messageId, int userId)
    {
        var message = await _chatRepository.GetByIdAsync(messageId);
        if (message != null && message.ReceiverId == userId && !message.IsRead)
        {
            message.MarkAsRead();
            _chatRepository.Update(message);
            await _chatRepository.SaveAsync();
        }
    }

    /// <inheritdoc />
    public async Task<IEnumerable<ChatMessageResponseDTO>> GetChatHistoryAsync(int userId, int otherUserId, int skip,
        int take)
    {
        var messages = await _chatRepository.GetChatHistoryAsync(userId, otherUserId, skip, take);
        return _mapper.Map<IEnumerable<ChatMessageResponseDTO>>(messages);
    }

    /// <inheritdoc />
    public async Task<IEnumerable<UserConversationDTO>?> GetConversationsAsync(int userId, int skip, int take)
    {
        var conversations = await _chatRepository.GetConversationsAsync(userId);
        var mapped = _mapper.Map<IEnumerable<UserConversationDTO>>(conversations, opt =>
            opt.Items["CurrentUserId"] = userId);
        return mapped.OrderByDescending(c => c.LastMessageDateTime).Skip(skip).Take(take);
    }
}