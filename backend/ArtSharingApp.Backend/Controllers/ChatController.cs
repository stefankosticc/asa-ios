using ArtSharingApp.Backend.Controllers.Common;
using ArtSharingApp.Backend.Service.ServiceInterface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ArtSharingApp.Backend.Controllers;

[ApiController]
[Authorize]
[Route("api")]
public class ChatController : AuthenticatedUserBaseController
{
    private readonly IChatService _chatService;

    public ChatController(IChatService chatService)
    {
        _chatService = chatService;
    }

    /// <summary>
    /// Returns all users that the logged-in user has had conversations with.
    /// This includes users who have sent messages to the logged-in user or vice versa.
    /// </summary>
    /// <param name="skip">Number of conversations to skip (for pagination).</param>
    /// <param name="take">Number of conversations to take (for pagination).</param>
    [HttpGet("chat/conversations")]
    public async Task<IActionResult> GetConversations([FromQuery] int skip = 0, [FromQuery] int take = 50)
    {
        var userId = GetLoggedInUserId();
        var conversations = await _chatService.GetConversationsAsync(userId, skip, take);
        return Ok(conversations);
    }
}