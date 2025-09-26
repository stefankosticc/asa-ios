using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;

namespace ArtSharingApp.Backend.Controllers.Common;

// Helper class to get the logged-in user
public class AuthenticatedUserBaseController : Controller
{
    protected int GetLoggedInUserId()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out var userId))
            throw new UnauthorizedAccessException("User not authenticated.");
        return userId;
    }
}