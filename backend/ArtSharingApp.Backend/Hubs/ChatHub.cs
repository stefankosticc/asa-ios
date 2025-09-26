using ArtSharingApp.Backend.Service.ServiceInterface;
using ArtSharingApp.Backend.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace ArtSharingApp.Backend.Hubs;

[Authorize]
public class ChatHub : Hub
{
    private readonly IChatService _chatService;

    public ChatHub(IChatService chatService)
    {
        _chatService = chatService;
    }

    // Send a private message to a user and persist it
    public async Task SendMessage(SendChatMessageRequestDTO request)
    {
        var senderId = GetLoggedInUserId();

        var chatMessage = await _chatService.SendMessageAsync(senderId, request.ReceiverId, request.Message);

        await Clients.User(request.ReceiverId.ToString()).SendAsync("ReceiveMessage", chatMessage);

        await Clients.Caller.SendAsync("MessageSent", chatMessage);
    }

    // Mark a message as read
    public async Task MarkAsRead(int messageId)
    {
        var userId = GetLoggedInUserId();
        await _chatService.MarkAsReadAsync(messageId, userId);
    }

    // Get chat history with another user
    public async Task<IEnumerable<object>> GetChatHistory(int otherUserId, int? skip = 0, int? take = 50)
    {
        var userId = GetLoggedInUserId();
        var messages = await _chatService.GetChatHistoryAsync(userId, otherUserId, skip ?? 0, take ?? 50);
        return messages;
    }

    private int GetLoggedInUserId()
    {
        return int.Parse(Context.User.FindFirstValue(ClaimTypes.NameIdentifier)!);
    }
}