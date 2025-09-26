using ArtSharingApp.Backend.Controllers.Common;
using ArtSharingApp.Backend.Service.ServiceInterface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ArtSharingApp.Backend.Controllers;

[ApiController]
[Authorize]
[Route("api")]
public class FollowersController : AuthenticatedUserBaseController
{
    private readonly IFollowersService _followersService;

    public FollowersController(IFollowersService followersService)
    {
        _followersService = followersService;
    }

    [HttpPost("follow/{userId}")]
    public async Task<IActionResult> FollowUser(int userId)
    {
        var loggedInUserId = GetLoggedInUserId();
        var followed = await _followersService.FollowUserAsync(loggedInUserId, userId);
        if (followed)
            return Ok(new { message = "User followed successfully." });
        return BadRequest(new { message = "Failed to follow user." });
    }

    [HttpDelete("unfollow/{userId}")]
    public async Task<IActionResult> UnfollowUser(int userId)
    {
        var loggedInUserId = GetLoggedInUserId();
        var unfollowed = await _followersService.UnfollowUserAsync(loggedInUserId, userId);
        if (unfollowed)
            return Ok(new { message = "User unfollowed successfully." });
        return BadRequest(new { message = "Failed to unfollow user." });
    }

    [HttpGet("user/{userId}/followers")]
    public async Task<IActionResult> GetFollowers(int userId, [FromQuery] int skip, [FromQuery] int take)
    {
        var followers = await _followersService.GetFollowersAsync(userId, skip, take);
        return Ok(followers);
    }

    [HttpGet("user/{userId}/following")]
    public async Task<IActionResult> GetFollowing(int userId, [FromQuery] int skip, [FromQuery] int take)
    {
        var following = await _followersService.GetFollowingAsync(userId, skip, take);
        return Ok(following);
    }

    [HttpGet("followed-users/artworks")]
    public async Task<IActionResult> GetFollowedUsersArtworks([FromQuery] int skip, [FromQuery] int take)
    {
        var loggedInUserId = GetLoggedInUserId();
        var artworks = await _followersService.GetFollowedUsersArtworksAsync(loggedInUserId, skip, take);
        return Ok(artworks);
    }
}